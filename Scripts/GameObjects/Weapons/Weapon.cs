using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Weapons
{
    internal class Weapon : GameObject
    {
        public static ushort meleeStartingIndex = 0;
        public static ushort rangedStartingIndex = 1;
        public static ushort meleeEndIndex = 0;
        public static ushort rangedEndIndex = 1;

        private string weaponType;
        private ushort projectileAnimIndex;
        public int attackRange { get; set; }
        public float fireRate;
        public int durability = 10;

        public float projectileSpeed { get; set; }
        public float lifespan { get; set; }
        public int damage;
        private int projWidth;
        private int projHeight;

        internal double timeWhenShot;

        public void Fire(Vector2 entityPos, bool isPlayer)
        {
            if (Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenShot > fireRate)
            {
                entityPos.X += 32 * (float)Math.Cos(rotation);
                entityPos.Y += 32 * (float)Math.Sin(rotation);

                float xTrans = (float)Math.Cos(rotation) * damage * projectileSpeed / 3000f;
                float yTrans = (float)Math.Sin(rotation) * damage * projectileSpeed / 3000f;
                float rotTrans = (float)Math.Cos(rotation) * damage * projectileSpeed / 3000000f;
                if (isPlayer)
                {
                    Game1.activePlayerProjectiles.Add(new Projectile(rotation, projectileSpeed, damage, entityPos, lifespan, projWidth, projHeight, projectileAnimIndex, weaponType));

                    Game1.camera.cameraAnimationsToPlay.Add((xTrans, yTrans, rotTrans, 0.2f));
                }
                else
                {
                    Game1.activeEnemyProjectiles.Add(new Projectile(rotation, projectileSpeed, damage, entityPos, lifespan, projWidth, projHeight, projectileAnimIndex, weaponType));
                }
                animationHandler.AddToMovementAnims(-xTrans * 5, -yTrans * 5, -rotTrans * 50, 0.2f);

                timeWhenShot = Game1.gameTime.TotalGameTime.TotalSeconds;
                animationHandler.SetTextureAnimation(1);
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
                rotation = (float)(Math.Atan2(y, x));

            position = new Vector2(entityPos.X, entityPos.Y + 8);
            UpdateAnimation();
            if (Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenShot > fireRate)
                animationHandler.SetTextureAnimation(0);
        }

        /// <summary>
        /// Use only with the player to update
        /// </summary>
        public void Update(HandlePlayerInputs inputHandler, Vector2 playerPos)
        {
            rotation = inputHandler.GetShootingAngle(playerPos);
            position = new Vector2(playerPos.X, playerPos.Y + 8);
            UpdateAnimation();
            if (Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenShot > fireRate)
                animationHandler.SetTextureAnimation(0);
        }

        public Weapon(ushort weaponIndex) : base(0,new Vector2(0,0))
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
            projectileAnimIndex = Convert.ToUInt16(tokens[9]);
            animationHandler.SetTextureAnimationData(Convert.ToUInt16(tokens[10]));

            isEnabled = true;
        }
    }
}
