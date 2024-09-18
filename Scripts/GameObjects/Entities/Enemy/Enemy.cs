using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities.Enemy
{
    internal class Enemy : Entities.Entity
    {
        int health;

        public Enemy(ushort objectIndex, Vector2 startingPos) : base(1, new Vector2(1,1), 1, new Hitboxes.Hitbox(0,0,0,0), EntityMovement.AIType.enemy)
        {
            string enemyData = "error";
            enemyData = File.ReadLines("Content/EnemyData.txt").Skip((objectIndex) * 2).Take(1).First();

            string[] enemyDataSplit = enemyData.Split(",");

            position = startingPos;
            //Health,EntitySpeed,TextureIndex,HitboxX,HitboxY,HitboxWidth,HitboxHeight
            health = Convert.ToInt32(enemyDataSplit[0]);
            entitySpeed = Convert.ToInt32(enemyDataSplit[1]);
            textureIndex = Convert.ToUInt16(enemyDataSplit[2]);
            hitBox = new Hitboxes.Hitbox(startingPos.X, startingPos.Y, Convert.ToInt32(enemyDataSplit[5]), Convert.ToInt32(enemyDataSplit[6]));

        }
    }
}
