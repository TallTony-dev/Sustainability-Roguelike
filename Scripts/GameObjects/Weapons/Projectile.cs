using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
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
        private bool isPlayer = false;

        public Projectile(float travelAngle, float speed, int damage, Vector2 startingPosition, float lifespan, int hitBoxWidth, int hitBoxHeight, ushort animationIndex, string weaponType, bool isPlayer) : base(animationIndex, Vector2.One)
        {
            this.travelAngle = travelAngle;
            rotation = travelAngle;
            this.speed = speed;
            this.lifespan = lifespan;
            lifespanRemaining = lifespan;
            this.damage = damage;
            hitBox = new Hitboxes.Hitbox(startingPosition.X, startingPosition.Y,hitBoxWidth,hitBoxHeight);
            position = startingPosition;
            animationHandler = new AnimationHandler(animationIndex);
            this.weaponType = weaponType;
            isEnabled = true;
            this.isPlayer = isPlayer;
            UpdateAnimation();
        }

        public void Update(bool isPlayerProjectile)
        {
            if (isEnabled)
            {
                UpdateAnimation();
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
                            if (!(tileX + x < 0 || tileX + x > 512 || tileY + y < 0 || tileY + y > 512))
                            {
                                if (hitBox.Intersects(TileMap.GetTileBounds(tileX + x, tileY + y).Item2))
                                {
                                    destroyed = true;
                                }
                            }

                        }
                    }

                    if (isPlayerProjectile)
                    {
                        foreach (GameObject gameObject in Game1.currentGameObjects)
                        {
                            if (gameObject is Entity)
                            {
                                Entity entity = (Entity)gameObject;
                                if (!destroyed && entity.isEnabled && hitBox.Intersects(entity.hitBox) && !(entity is Player))
                                {
                                    entity.health -= damage;
                                    destroyed = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject gameObject in Game1.currentGameObjects)
                        {
                            if (gameObject is Entity)
                            {
                                Entity entity = (Entity)gameObject;
                                if (!destroyed && entity is Player && entity.isEnabled && hitBox.Intersects(entity.hitBox))
                                {
                                    entity.health -= damage;
                                    destroyed = true;
                                }
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
                        Destroy();
                    }
                }
            }
        }

        public override void Destroy()
        {
            isEnabled = false;
            base.Destroy();
            if (weaponType == "ranged" || weaponType == "melee" || isReplica) 
            {
                Game1.activeParticleEmitters.Add(new ParticleEmitter(position, 80, travelAngle, 0.5f, 5, 3f, 0.05f, 1f, 360, 0, 50f));
                if (isPlayer)
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
                Game1.audioPlayer.PlaySoundEffect2D(1, position);
                //releases smaller and weaker projectiles upon destruction, make a different small texture for these projectiles
                if (isPlayer)
                {
                    Game1.activePlayerProjectiles.Remove(this);
                    for (int i = 0; i < 6; i++)
                    {
                        Game1.activePlayerProjectiles.Add(new Projectile(i * 45, speed / 1.3f, damage / 6, offsetPos, lifespan/1.5f, (int)hitBox.width - 10, (int)hitBox.height - 10, animationHandler.animationIndex, "6burst", true));
                        Game1.activePlayerProjectiles.Last().isReplica = true;
                    }
                }
                else
                {
                    Game1.activeEnemyProjectiles.Remove(this);
                    for (int i = 0; i < 6; i++)
                    {
                        Game1.activeEnemyProjectiles.Add(new Projectile(i * 45, speed / 1.3f, damage / 6, offsetPos, lifespan/1.5f, (int)hitBox.width - 10, (int)hitBox.height - 10, animationHandler.animationIndex, "6burst", false));
                        Game1.activeEnemyProjectiles.Last().isReplica = true;
                    }
                }
            }

        }
    }
}
