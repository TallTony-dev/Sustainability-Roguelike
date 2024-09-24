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
        public ushort textureIndex { get; private set; }
        private float lifespan;
        public Hitboxes.Hitbox hitBox;
        private double timeCreated;

        public AnimationHandler animationHandler;
        public Projectile(float travelAngle, float speed, int damage, Vector2 startingPosition, ushort textureIndex, float lifespan, int hitBoxWidth, int hitBoxHeight, ushort ?animationIndex)
        {
            this.travelAngle = travelAngle;
            this.speed = speed;
            position = startingPosition;
            this.textureIndex = textureIndex;
            this.lifespan = lifespan;
            this.damage = damage;
            hitBox = new Hitboxes.Hitbox(startingPosition.X, startingPosition.Y,hitBoxWidth,hitBoxHeight);
            timeCreated = Game1.gameTime.TotalGameTime.TotalSeconds;
            if (animationIndex != null)
                animationHandler = new AnimationHandler((ushort)animationIndex);
        }

        public void Update(bool isPlayerProjectile)
        {
            float updatedProjectileSpeed = (float)(speed * Game1.gameTime.ElapsedGameTime.TotalSeconds);
            float newXPos = position.X + (float)Math.Cos(travelAngle) * updatedProjectileSpeed;
            float newYPos = position.Y + (float)Math.Sin(travelAngle) * updatedProjectileSpeed;
            position = new Vector2(newXPos, newYPos);
            hitBox.xPos = newXPos;
            hitBox.yPos = newYPos;

            textureIndex = animationHandler.Update();

            if (Game1.gameTime.TotalGameTime.TotalSeconds - timeCreated > lifespan - speed)
            {
                bool destroyed = false;

                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        (int tileX, int tileY) = TileMap.PosToAbsTileMapPos(position);
                        if (!destroyed && hitBox.Intersects(TileMap.GetTileBounds(tileX, tileY).Item2))
                        {
                            destroyed = true;
                        }
                    }
                }

                if (!destroyed && isPlayerProjectile)
                {
                    foreach (Entity entity in Game1.currentGameObjects)
                    {
                        if (!destroyed && entity.isEnabled && hitBox.Intersects(entity.hitBox) && !(entity is Player))
                        {
                            entity.health -= damage;
                            destroyed = true;
                        }
                    }
                }
                else if (!destroyed)
                {
                    foreach (Entity entity in Game1.currentGameObjects)
                    {
                        if (!destroyed && entity is Player && entity.isEnabled && hitBox.Intersects(entity.hitBox))
                        {
                            entity.health -= damage;
                            destroyed = true;
                        }
                    }
                }

                if (Game1.gameTime.TotalGameTime.TotalSeconds - timeCreated > lifespan) //CAN BE OPTIMIZED HERE
                {
                    destroyed = true;
                }


                if (destroyed)
                {
                    Destroy(isPlayerProjectile);
                }
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
