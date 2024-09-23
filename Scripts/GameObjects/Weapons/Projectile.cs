using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Weapons
{
    internal class Projectile
    {
        public float travelAngle { get; private set; }
        private float speed { get; set; }
        public Vector2 position { get; set; }
        private int damage;
        public ushort textureIndex { get; }
        private float lifespan;
        public Hitboxes.Hitbox hitBox;
        private double timeCreated;
        public Projectile(float travelAngle, float speed, int damage, Vector2 startingPosition, ushort textureIndex, float lifespan, int hitBoxWidth, int hitBoxHeight)
        {
            this.travelAngle = travelAngle;
            this.speed = speed;
            position = startingPosition;
            this.textureIndex = textureIndex;
            this.lifespan = lifespan;
            this.damage = damage;
            hitBox = new Hitboxes.Hitbox(startingPosition.X, startingPosition.Y,hitBoxWidth,hitBoxHeight);
            timeCreated = Game1.gameTime.TotalGameTime.TotalSeconds;
        }

        public void Update(bool isPlayerProjectile)
        {
            float updatedProjectileSpeed = (float)(speed * Game1.gameTime.ElapsedGameTime.TotalSeconds);
            float newXPos = position.X + (float)Math.Cos(travelAngle) * updatedProjectileSpeed;
            float newYPos = position.Y + (float)Math.Sin(travelAngle) * updatedProjectileSpeed;
            position = new Vector2(newXPos, newYPos);
            hitBox.xPos = newXPos;
            hitBox.yPos = newYPos;
            bool destroy = false;

            for(int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    (int tileX, int tileY) = TileMap.PosToAbsTileMapPos(position);
                    if (hitBox.Intersects(TileMap.GetTileBounds(tileX, tileY).Item2))
                    {
                        destroy = true;
                    }
                }
            }

            if (!destroy && isPlayerProjectile)
            {
                foreach (Entity entity in Game1.currentGameObjects)
                {
                    if (entity.isEnabled && hitBox.Intersects(entity.hitBox) && !(entity is Player))
                    {
                        entity.health -= damage;
                        destroy = true;
                    }
                }
            }
            else if (!destroy)
            {
                foreach (Entity entity in Game1.currentGameObjects)
                {
                    if (entity is Player && entity.isEnabled && hitBox.Intersects(entity.hitBox))
                    {
                        entity.health -= damage;
                        destroy = true;
                    }
                }
            }

            if (Game1.gameTime.TotalGameTime.TotalSeconds - timeCreated > lifespan) //CAN BE OPTIMIZED HERE
            {
                destroy = true;
            }


            if (destroy)
            {
                Destroy(isPlayerProjectile);
            }
        }

        public void Destroy(bool isPlayerProj)
        {
            if (isPlayerProj)
                Game1.activePlayerProjectiles.Remove(this);
            else
                Game1.activeEnemyProjectiles.Remove(this);
        }

    }
}
