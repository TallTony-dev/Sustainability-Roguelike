using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.Entities;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities
{
    /// <summary>
    /// Entity base class which defines speed, 
    /// </summary>
    internal abstract class Entity : GameObject
    {
        public float entitySpeed { get; set; }
        public Vector2 position { get; internal set; }
        internal bool canDash { get; }
        internal bool isFlipped { get; set; }

        internal EntityMovement entityMovement;
        internal HitboxManager hitboxManager;
        internal Weapons.Weapon weapon;
        public Entity(float entitySpeed, Vector2 startingPos, bool canDash, ushort textureIndex, EntityMovement.AIType aiType) : base(textureIndex)
        {
            this.entitySpeed = entitySpeed;
            this.position = startingPos;
            this.canDash = canDash;
            entityMovement = new EntityMovement(aiType, this);
            hitboxManager = new HitboxManager();
        }
        public void Update()
        {
            Move();
        }
        public virtual void Move()
        {
            Vector2 entityNewPos = entityMovement.GetPathfindingMovement(weapon.attackRange, position, entitySpeed);

            entityNewPos = entityMovement.ValidateMovement(this, entityNewPos);

            if (entityNewPos.X > position.X)
                isFlipped = true;
            else
                isFlipped = false;
            position = entityNewPos;
        }
    }
}
