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

        public Enemy(ushort enemyIndex, Vector2 startingPos) : base(1, new Vector2(1,1), 0, new Hitboxes.Hitbox(0,0,0,0), EntityMovement.AIType.enemy)
        {
            string enemyData = "error";
            enemyData = File.ReadLines("Content/EnemyData.txt").Skip((enemyIndex) * 2).Take(1).First();

            string[] enemyDataSplit = enemyData.Split(",");

            position = startingPos;
            //Health,EntitySpeed,TextureIndex,HitboxX,HitboxY,HitboxWidth,HitboxHeight
            health = Convert.ToInt32(enemyDataSplit[0]);
            entitySpeed = Convert.ToInt32(enemyDataSplit[1]);
            SetAnimationData(Convert.ToUInt16(enemyDataSplit[2]));
            if (enemyDataSplit[3] == "melee")
                activeWeapon = new Weapon((ushort)Game1.rand.Next(Weapon.meleeStartingIndex, Weapon.meleeEndIndex + 1));
            else if (enemyDataSplit[3] == "ranged")
                activeWeapon = new Weapon((ushort)Game1.rand.Next(Weapon.rangedStartingIndex, Weapon.rangedEndIndex + 1));
            hitBox = new Hitboxes.Hitbox(startingPos.X, startingPos.Y, Convert.ToInt32(enemyDataSplit[4]), Convert.ToInt32(enemyDataSplit[5]));

        }
    }
}
