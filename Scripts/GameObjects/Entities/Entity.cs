using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts;
using Monogame_Cross_Platform.Scripts.Hitboxes;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities
{
    /// <summary>
    /// Entity base class which defines speed, 
    /// </summary>
    internal abstract class Entity : GameObject
    {
        public int health;
        public int maxHealth;
        public float entitySpeed { get; set; }
        internal bool isFlipped { get; set; }
        internal int movesLeft { get; set; } = 1; //Temp at 1
        internal bool isInComboMode = false;
        internal bool ignoresCollisions = false;

        internal float movingSpeed;
        internal bool isMoving = false;
        internal bool isScared = false;

        internal EntityMovement entityMovement;
        internal Hitbox hitBox;
        internal Weapon activeWeapon;
        internal EntityMovement.AIType aiType;

        internal ParticleEmitter sweatParticles;

        public Entity(float entitySpeed, Vector2 startingTile, ushort animationIndex, Hitbox hitBox, EntityMovement.AIType aiType, int health) : base(animationIndex, startingTile)
        {
            this.entitySpeed = entitySpeed;
            entityMovement = new EntityMovement(aiType);
            this.hitBox = hitBox;
            this.aiType = aiType;
            this.health = health;
            maxHealth = health;
            sweatParticles = new ParticleEmitter(Vector2.One, 40, 180, 0.5f, 58, 0.4f, 0.2f, float.PositiveInfinity, 90, 0, 80, 0, true);
        }
        public override void Destroy()
        {
            isEnabled = false;
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                    TileMap.SetPathfindingModeOfTileAtCoord(position.X + x * 16, position.Y + y * 16, false);

            Level.LevelGenerator.PosToRoom(position).gameObjects.Remove(this);
            if (activeWeapon != null)
                activeWeapon.Drop(position);
            Game1.currentGameObjects.Remove(this);
            sweatParticles.Destroy();
        }
        public virtual void Update(Player.Player player)
        {
            if (health < 0)
            {
                Destroy();
            }
            if (isEnabled)
            {
                TakeAction(player);
                CheckRoom();

                if (movingSpeed > 0)
                {
                    isMoving = true;
                    animationHandler.SetTextureAnimation(1);
                    animationHandler.SetAnimationSpeed(1, (int)(10000 / movingSpeed));
                }
                else
                {
                    isMoving = false;
                    animationHandler.SetTextureAnimation(0);
                }

            }
        }
        public virtual void CheckRoom()
        {
            Level.Room room = Level.LevelGenerator.PosToRoom(position);
            (int entityTileX, int entityTileY) = TileMap.PosToAbsTileMapPos(position);
            bool hasPickedUp = false;
            foreach (GameObject gameObject in room.gameObjects)
            {
                if (activeWeapon == null && gameObject is Weapon && !hasPickedUp)
                {
                    Weapon weapon = (Weapon)gameObject;
                    (int weaponTileX, int weaponTileY) = TileMap.PosToAbsTileMapPos(weapon.position);
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            if (entityTileX + x == weaponTileX && entityTileY + y == weaponTileY)
                            {
                                weapon.Pickup(this);
                                break;
                            }
                        }
                    }
                }
            }
        }
        public virtual void TakeAction(Player.Player playerToFollow)
        {
            if (!isInComboMode)
            {
                (isScared, Vector2 entityNewPos) = entityMovement.GetNormalPathfindingMovement(activeWeapon.attackRange, position, entitySpeed, aiType, playerToFollow);
                hitBox.UpdatePosition(entityNewPos.X, entityNewPos.Y);

                activeWeapon.Update(playerToFollow.position, position);

                //checks if should be flipped
                if (entityNewPos.X > position.X)
                    isFlipped = true;
                else
                    isFlipped = false;
                
                sweatParticles.position = position;
                sweatParticles.particlexVelocity = (entityNewPos - position).X * (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * 1000;
                if (isScared)
                {
                    sweatParticles.isEnabled = true;
                }
                else
                    sweatParticles.isEnabled = false;

                //checks if moving for animations
                movingSpeed = (Math.Abs(entityNewPos.X - position.X) + Math.Abs(entityNewPos.Y - position.Y)) * Game1.gameTime.ElapsedGameTime.Milliseconds;
                (int playerTileX, int playerTileY) = TileMap.PosToAbsTileMapPos(playerToFollow.position);
                (int entityTileX, int entityTileY) = TileMap.PosToAbsTileMapPos(entityNewPos);
                float distanceBetween = EntityMovement.DistanceBetween(new Vector2(playerTileX, playerTileY), new Vector2(entityTileX, entityTileY));
                if (distanceBetween < activeWeapon.attackRange + 1.4)
                    activeWeapon.Fire(entityNewPos, false);
                position = entityNewPos;
            }
            else if (movesLeft > 0)
            {

            }
        }
        public int GetMovesPerTurn()
        {
            //for every 16 gets one more move per turn, meaning 1 tile/sec is 2 moves per turn, might wanna change
            return (int)(entitySpeed / 16);
        }
    }
}
