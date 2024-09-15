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
        public float entitySpeed { get; set; }
        public Vector2 position { get; internal set; }
        internal bool isFlipped { get; set; }
        internal int movesLeft { get; set; } = 1; //Temp at 1
        internal bool isInAbsMovementMode = false;
        internal bool ignoresCollisions = false;
        internal List<(int, int)> pointToPathfind = new List<(int, int)>();

        internal float movingSpeed;
        internal bool isMoving = false;

        internal EntityMovement entityMovement;
        internal Hitbox hitBox;
        internal Weapons.Weapon weapon1;
        internal Weapons.Weapon weapon2;
        internal EntityMovement.AIType aiType;
        
        public Entity(float entitySpeed, Vector2 startingTile, ushort textureIndex, Hitbox hitBox, EntityMovement.AIType aiType) : base(textureIndex)
        {
            this.entitySpeed = entitySpeed;
            position = TileMap.TileMapPosToPos((int)startingTile.X, (int)startingTile.Y);
            entityMovement = new EntityMovement(aiType, this);
            this.hitBox = hitBox;
            this.aiType = aiType;
        }
        public void Update(Player.Player player)
        {
            Move(player);
            if (movingSpeed > 0)
            {
                isMoving = true;
                animationHandler.SetAnimation(1);
                animationHandler.SetAnimationSpeed(1, (int)(20000/movingSpeed));
            }
            else
            {
                isMoving = false;
                animationHandler.SetAnimation(0);
            }

        }
        public virtual void Move(Player.Player playerToFollow)
        {
            if (!isInAbsMovementMode)
            {
                Vector2 entityNewPos = entityMovement.GetPathfindingMovement(weapon1.attackRange, position, entitySpeed, aiType, playerToFollow);
                if (!ignoresCollisions)
                {
                    hitBox.UpdatePosition(entityNewPos.X, entityNewPos.Y);
                    entityNewPos = entityMovement.ValidateMovement(this, entityNewPos);
                }
                hitBox.UpdatePosition(entityNewPos.X, entityNewPos.Y);

                if (entityNewPos.X > position.X)
                    isFlipped = true;
                else
                    isFlipped = false;
                movingSpeed = (Math.Abs(entityNewPos.X - position.X) + Math.Abs(entityNewPos.Y - position.Y)) * Game1.gameTime.ElapsedGameTime.Milliseconds;
                if (movingSpeed > 0)
                    isMoving = true;
                else
                    isMoving = false;
                position = entityNewPos;
            }
            else
            {

            }
        }
        public virtual void TurnBasedMove(Player.Player playerToFollow)
        {

        }
    }
}
