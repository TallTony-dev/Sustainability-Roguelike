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
        internal bool isMoving { get; set; }
        internal bool isInAbsMovementMode = false;
        internal bool ignoresCollisions = false;
        internal List<(int, int)> pointToPathfind = new List<(int, int)>();

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
