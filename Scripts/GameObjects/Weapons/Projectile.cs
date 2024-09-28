using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Weapons
{
    internal class Projectile : GameObject
    {
        string weaponType;
        public float travelAngle { get; private set; }
        private float speed { get; set; }
        private int damage;
        private double lifespanRemaining;
        private float lifespan;
        public Hitboxes.Hitbox hitBox;

        private bool destroyed = false;
        private bool isReplica = false;

        public Projectile(float travelAngle, float speed, int damage, Vector2 startingPosition, ushort textureIndex, float lifespan, int hitBoxWidth, int hitBoxHeight, ushort animationIndex, string weaponType) : base(animationIndex, Vector2.One)
        {
            this.travelAngle = travelAngle;
            this.speed = speed;
            this.textureIndex = textureIndex;
            this.lifespan = lifespan;
            lifespanRemaining = lifespan;
            this.damage = damage;
            hitBox = new Hitboxes.Hitbox(startingPosition.X, startingPosition.Y,hitBoxWidth,hitBoxHeight);
            position = startingPosition;
            animationHandler = new AnimationHandler(animationIndex);
            this.weaponType = weaponType;
            isEnabled = true;
        }

        public void Update(bool isPlayerProjectile)
        {
            if (isEnabled)
            {
                //UpdateAnimation();
                float updatedProjectileSpeed = (float)(speed * Game1.gameTime.ElapsedGameTime.TotalSeconds);
                float newXPos = position.X + (float)Math.Cos(travelAngle) * updatedProjectileSpeed;
                float newYPos = position.Y + (float)Math.Sin(travelAngle) * updatedProjectileSpeed;
                position = new Vector2(newXPos, newYPos);
                hitBox.xPos = newXPos;
                hitBox.yPos = newYPos;
                lifespanRemaining -= Game1.gameTime.ElapsedGameTime.TotalSeconds;

                if (!destroyed)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            (int tileX, int tileY) = TileMap.PosToAbsTileMapPos(position);
                            if (hitBox.Intersects(TileMap.GetTileBounds(tileX, tileY).Item2))
                            {
                                destroyed = true;
                            }
                        }
                    }

                    if (isPlayerProjectile)
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
                    else
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

                    if (lifespanRemaining < 0)
                    {
                        destroyed = true;
                    }
                }
                

                if (weaponType != "melee" || 0.1 > lifespanRemaining)
                {
                    if (destroyed)
                    {
                        Destroy(isPlayerProjectile);
                    }
                }
            }
        }

        public void Destroy(bool isPlayerProj)
        {
            isEnabled = false;
            if (weaponType == "ranged" || weaponType == "melee" || isReplica) 
            {
                if (isPlayerProj)
                    Game1.activePlayerProjectiles.Remove(this);
                else
                    Game1.activeEnemyProjectiles.Remove(this);
            }
            if (weaponType == "6burst" && !isReplica)
            {
                //offsets pos back by how far it moves in 1/20 of a second
                float xPos = position.X - (float)Math.Cos(travelAngle) * speed / 20;
                float yPos = position.Y - (float)Math.Sin(travelAngle) * speed / 20;
                Vector2 offsetPos = new Vector2(xPos, yPos);

                //releases smaller and weaker projectiles upon destruction, make a different small texture for these projectiles
                if (isPlayerProj)
                {
                    Game1.activePlayerProjectiles.Remove(this);
                    for (int i = 0; i < 6; i++)
                    {
                        Game1.activePlayerProjectiles.Add(new Projectile(i * 45, speed / 1.3f, damage / 6, offsetPos, textureIndex, lifespan/1.5f, (int)hitBox.width - 10, (int)hitBox.height - 10, animationHandler.animationIndex, "6burst"));
                        Game1.activePlayerProjectiles.Last().isReplica = true;
                    }
                }
                else
                {
                    Game1.activeEnemyProjectiles.Remove(this);
                    for (int i = 0; i < 6; i++)
                    {
                        Game1.activeEnemyProjectiles.Add(new Projectile(i * 45, speed / 1.3f, damage / 6, offsetPos, textureIndex, lifespan/1.5f, (int)hitBox.width - 10, (int)hitBox.height - 10, animationHandler.animationIndex, "6burst"));
                        Game1.activeEnemyProjectiles.Last().isReplica = true;
                    }
                }
            }

        }
    }
}
