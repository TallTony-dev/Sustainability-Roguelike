using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Weapons
{
    internal class Weapon
    {
        public static ushort meleeStartingIndex = 0;
        public static ushort rangedStartingIndex = 1;
        public static ushort meleeEndIndex = 0;
        public static ushort rangedEndIndex = 1;


        private string weaponType;
        private ushort animIndex;
        public int attackRange { get; set; }
        public float fireRate;

        public float projectileSpeed { get; set; }
        public float lifespan { get; set; }
        public ushort textureIndex;
        public int damage;
        private int projWidth;
        private int projHeight;

        internal float aimAngle { get; set; }
        internal double timeWhenShot;

        public void Fire(Vector2 entityPos, bool isPlayer)
        {
            if (Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenShot > fireRate)
            {
                entityPos.X += 32 * (float)Math.Cos(aimAngle);
                entityPos.Y += 32 * (float)Math.Sin(aimAngle);
                if (isPlayer)
                {
                    Game1.activePlayerProjectiles.Add(new Projectile(aimAngle, projectileSpeed, damage, entityPos, textureIndex, lifespan, projWidth, projHeight, animIndex, weaponType));
                }
                else
                {
                    Game1.activeEnemyProjectiles.Add(new Projectile(aimAngle, projectileSpeed, damage, entityPos, textureIndex, lifespan, projWidth, projHeight, animIndex, weaponType));
                }

                timeWhenShot = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            
        }

        /// <summary>
        /// Use with non player entities
        /// </summary>
        public void Update(Vector2 targetPos, Vector2 entityPos)
        {
            float y = (targetPos.Y - entityPos.Y);
            float x = (targetPos.X - entityPos.X);
            if (Math.Abs(x) > 0 && Math.Abs(y) > 0)
                aimAngle = (float)(Math.Atan2(y, x));
        }

        /// <summary>
        /// Use only with the player to update
        /// </summary>
        public void Update(HandlePlayerInputs inputHandler)
        {
            aimAngle = inputHandler.GetShootingAngle();
        }

        public Weapon(ushort weaponIndex)
        {
            string weaponData = File.ReadLines("Content/WeaponData.txt").Skip((weaponIndex) * 2).Take(1).First();

            string[] tokens = weaponData.Split(",");
            weaponType = Convert.ToString(tokens[0]);
            attackRange = Convert.ToInt32(tokens[1]);
            textureIndex = Convert.ToUInt16(tokens[2]);
            projectileSpeed = Convert.ToSingle(tokens[3]);
            lifespan = Convert.ToSingle(tokens[4]);
            damage = Convert.ToInt32(tokens[5]);
            projWidth = Convert.ToInt32(tokens[6]);
            projHeight = Convert.ToInt32(tokens[7]);
            fireRate = Convert.ToSingle(tokens[8]);
            animIndex = Convert.ToUInt16(tokens[9]);
        }
    }
}
