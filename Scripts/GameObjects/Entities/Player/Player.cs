using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player
{
    /// <summary>
    /// Contains everything pertaining to the player itself, but not drawing or anything, derived from Entity.
    /// </summary>
    internal class Player : Entity
    {
        HandlePlayerInputs inputHandler = new HandlePlayerInputs();
        public bool isInLevelEditorMode = false;
        public List<Weapon> weapons = new List<Weapon>();
        public short activeWeaponIndex = 0;
        Vector2 entityToFollowPos;

         public Player(int health, float entitySpeed, Vector2 startingTile, Hitboxes.Hitbox hitBox, ushort textureIndex) : base(entitySpeed, startingTile, textureIndex, hitBox, EntityMovement.AIType.none, health)
        {
            isEnabled = true;
            weapons.Add(new Weapon(0));
            weapons.Add(new Weapon(1));
            weapons.Add(new Weapon(2));
            weapons.Add(new Weapon(3));
        }

        public override void Update(Player player)
        {
            if (ContentManagers.Camera.Camera.IsLocked)
                Game1.camera.Follow(this);
            else
            {
                if (Level.LevelGenerator.PosToRoom(position).gameObjects.Count != 0)
                {
                    foreach (Entity entity in Level.LevelGenerator.PosToRoom(position).gameObjects)
                    {
                        if (Vector2.Distance(position, entity.position) < Vector2.Distance(position, entityToFollowPos))
                        {
                            entityToFollowPos = entity.position;
                        }
                    }
                    Game1.camera.Follow(position, entityToFollowPos);
                }
                else
                    ContentManagers.Camera.Camera.IsLocked = true;
            }
            if (isEnabled)
            {

                TakeAction(player);

                activeWeaponIndex = inputHandler.GetWeaponIndex(activeWeaponIndex, weapons.Count);
                activeWeapon = weapons[activeWeaponIndex];
                activeWeapon.Update(inputHandler, player.position);

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
                position = playerNewPos;
            }
        }
    }
}
