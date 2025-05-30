﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.GameObjects.Objects;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player
{
    /// <summary>
    /// Contains everything pertaining to the player itself, but not drawing or anything, derived from Entity.
    /// </summary>
    internal class Player : Entity
    {
        public HandlePlayerInputs inputHandler = new HandlePlayerInputs();
        public bool isInLevelEditorMode = false;
        public List<Weapon> weapons = new List<Weapon>();
        public short activeWeaponIndex = 0;
        public int maxWeapons = 8;

        public Player(int health, float entitySpeed, Vector2 startingTile, Hitboxes.Hitbox hitBox, ushort textureIndex) : base(entitySpeed, startingTile, textureIndex, hitBox, EntityMovement.AIType.none, health)
        {
            isEnabled = true;
            weapons.Add(new Weapon(0, this));
            weapons.Add(new Weapon(1, this));
            weapons.Add(new Weapon(2, this));
            weapons.Add(new Weapon(3, this));
            sweatParticles.isEnabled = false;
        }

        double timeWheninteracted = 0;
        public override void CheckRoom()
        {
            Room room = LevelGenerator.PosToRoom(position);
            var kstate = Keyboard.GetState();
            var gstate = GamePad.GetState(PlayerIndex.One);
            bool hasInteracted = false;

            (int playerTileX, int playerTileY) = TileMap.PosToAbsTileMapPos(position);
            for (int i = 0; i < room.gameObjects.Count; i++)
            {
                GameObject gameObject = room.gameObjects[i];
                
                if ((kstate.IsKeyDown(Keys.E) || gstate.IsButtonDown(Buttons.B)) && !hasInteracted && Game1.gameTime.TotalGameTime.TotalSeconds - timeWheninteracted > 0.2)
                {
                    if (gameObject is InteractableObject)
                    {
                        InteractableObject interactableObject = (InteractableObject)gameObject;
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (interactableObject.DoesCollide(TileMap.ForceGetTileBounds(playerTileX - x, playerTileY + y)) && !hasInteracted)
                                {
                                    interactableObject.Interact();
                                    hasInteracted = true;
                                    timeWheninteracted = Game1.gameTime.TotalGameTime.TotalSeconds;
                                }
                            }
                        }
                    }
                    else if (weapons.Count < maxWeapons && gameObject is Weapon)
                    {
                        Weapon weapon = (Weapon)gameObject;
                        (int weaponTileX, int weaponTileY) = TileMap.PosToAbsTileMapPos(weapon.position);
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (!hasInteracted && playerTileX + x == weaponTileX && playerTileY + y == weaponTileY)
                                {
                                    weapon.Pickup(this);
                                    hasInteracted = true;
                                    timeWheninteracted = Game1.gameTime.TotalGameTime.TotalSeconds;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns false if not added, true if did add, wont be added if inventory is full
        /// </summary>
        public bool AddToInventory(Weapon weapon)
        {
            if (weapons.Count < maxWeapons)
            {
                weapons.Add(weapon);
                return true;
            }
            else return false;
        }


        Entity entityToFollow;
        Vector2 entityToFollowPos;
        public override void Update(Player player)
        {
            Room room = LevelGenerator.PosToRoom(new Vector2(position.X + 128, position.Y + 128));
            if (ContentManagers.Camera.Camera.IsLocked || !room.gameObjects.Any(t => t is Entity))
                Game1.camera.Follow(this);

            //follow nearest entity if unlocked
            else
            {
                if (entityToFollow != null && room.gameObjects.Contains(entityToFollow))
                {
                    Game1.camera.Follow(position, entityToFollow.position);
                }
                else if (room.gameObjects.Any(t => t is Entity))
                {
                    entityToFollowPos = new Vector2(-10000, -10000);
                    for (int i = 0; i < room.gameObjects.Count; i++)
                    {
                        if (room.gameObjects[i] is Entity)
                        {
                            Entity entity = (Entity)room.gameObjects[i];
                            if (Vector2.Distance(position, entity.position) < Vector2.Distance(position, entityToFollowPos))
                            {
                                entityToFollow = entity;
                                entityToFollowPos = entity.position;
                            }
                        }
                    }
                    Game1.camera.Follow(position, entityToFollowPos);
                }
                else
                    ContentManagers.Camera.Camera.IsLocked = true;
            }
            if (isEnabled)
            {
                CheckRoom();
                TakeAction(player);

                if (health < 0 && !GameState.loseMenu.isActive)
                {
                    GameState.LoseGame();
                }

                activeWeaponIndex = inputHandler.GetWeaponIndex(activeWeaponIndex, weapons.Count);
                activeWeapon = weapons[activeWeaponIndex];
                activeWeapon.Update(inputHandler, player.position);
                foreach (Weapon weapon in weapons)
                {
                    weapon.Update();
                    if (weapon != activeWeapon)
                    {
                        weapon.isDrawn = false;
                    }
                    else
                        weapon.isDrawn = true;
                }


                if (inputHandler.IsShooting())
                    activeWeapon.Fire(position, true);

                if (movingSpeed > 0)
                {
                    isMoving = true;
                    animationHandler.SetTextureAnimation(1);
                    animationHandler.SetAnimationSpeed(1, (int)(20000 / movingSpeed));
                }
                else
                {
                    isMoving = false;
                    animationHandler.SetTextureAnimation(0);
                }

            }
        }

        public override void TakeAction(Player _)
        {
            //TODO: rework this from abs movement to combo mode
            if (!isInComboMode && isEnabled)
            {
                Vector2 playerNewPos = inputHandler.GetPlayerMovement(position, entitySpeed);
                if (!ignoresCollisions)
                {
                    hitBox.UpdatePosition(playerNewPos.X, playerNewPos.Y);
                    playerNewPos = entityMovement.ValidateMovement(this, playerNewPos);
                }
                hitBox.UpdatePosition(playerNewPos.X, playerNewPos.Y);

                if (playerNewPos.X > position.X)
                    isFlipped = true;
                else
                    isFlipped = false;
                movingSpeed = (Math.Abs(playerNewPos.X - position.X) + Math.Abs(playerNewPos.Y - position.Y)) * Game1.gameTime.ElapsedGameTime.Milliseconds;
                if (movingSpeed > 0)
                    isMoving = true;
                else
                    isMoving = false;

                Game1.audioPlayer.UpdateListener(position, playerNewPos);
                position = playerNewPos;
            }
            else if(isEnabled && movesLeft > 0)
            {
                Vector2 playerNewPos = inputHandler.GetPlayerTurnBasedMovement(position, Game1.gameTime, entitySpeed, movesLeft, isMoving, ignoresCollisions);

                if (playerNewPos.X > position.X)
                    isFlipped = true;
                else
                    isFlipped = false;
                movingSpeed = (Math.Abs(playerNewPos.X - position.X) + Math.Abs(playerNewPos.Y - position.Y)) * Game1.gameTime.ElapsedGameTime.Milliseconds;
                if (movingSpeed > 0)
                    isMoving = true;
                else
                    isMoving = false;

                Game1.audioPlayer.UpdateListener(position, Vector2.Normalize(playerNewPos));
                position = playerNewPos;
            }
        }
    }
}
