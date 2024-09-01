using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities.Enemy
{
    internal class Enemy : Entities.Entity
    {
        int health;
        int attackRange;

        public Enemy(int health, int attackRange, float entitySpeed, Vector2 startingPos, bool canDash, ushort textureIndex) : base(entitySpeed, startingPos, canDash, textureIndex, EntityMovement.AIType.enemy)
        {
            this.health = health;
            this.attackRange = attackRange;
        }
    }
}
