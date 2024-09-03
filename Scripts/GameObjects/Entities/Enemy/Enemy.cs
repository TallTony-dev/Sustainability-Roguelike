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

        public Enemy(int health, int attackRange, float entitySpeed, Vector2 startingPos, Rectangle hitBox, ushort textureIndex) : base(entitySpeed, startingPos, textureIndex, hitBox, EntityMovement.AIType.enemy)
        {
            this.health = health;
        }
    }
}
