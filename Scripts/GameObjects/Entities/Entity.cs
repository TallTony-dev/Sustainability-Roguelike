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
        internal bool isFlipped { get; set; }

        internal EntityMovement entityMovement;
        internal HitboxManager hitboxManager;
        internal Weapons.Weapon weapon;
        internal EntityMovement.AIType aiType;
        
        public Entity(float entitySpeed, Vector2 startingPos, ushort textureIndex, Rectangle hitBox, EntityMovement.AIType aiType) : base(textureIndex)
        {
            this.entitySpeed = entitySpeed;
            position = startingPos;
            entityMovement = new EntityMovement(aiType, this);
            hitboxManager = new HitboxManager(hitBox);
            this.aiType = aiType;
        }
        public void Update(Player.Player player)
        {
            Move(player);
            hitboxManager.hitBox = new Rectangle ((int)position.X - 16,(int)position.Y - 16,hitboxManager.hitBox.Width, hitboxManager.hitBox.Height); //INTEGERS HERE MIGHT CAUSE WACKYNESS
        }
        public virtual void Move(Player.Player player)
        {
            Vector2 entityNewPos = entityMovement.GetPathfindingMovement(weapon.attackRange, position, entitySpeed, aiType ,player);

            entityNewPos = entityMovement.ValidateMovement(this, entityNewPos);

            if (entityNewPos.X > position.X)
                isFlipped = true;
            else
                isFlipped = false;
            position = entityNewPos;
        }
    }
}
