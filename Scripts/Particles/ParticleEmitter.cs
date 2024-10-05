using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class ParticleEmitter
    {
        Vector2 position;
        float particleVelocity;
        float particleAngle;
        float particleScale;
        float lifetime;
        ushort textureIndex;
        float frequency;
        double lastTimeShot;
        Random random;
        double expiryTime;

        float spread;
        float minAngle { get => particleAngle - spread / 114.59f; }
        float maxAngle { get => particleAngle + spread / 114.59f; }

        public void Update()
        {
            if (!(Game1.gameTime.TotalGameTime.TotalSeconds > expiryTime))
            {
                int timesToFire = (int)((Game1.gameTime.TotalGameTime.TotalSeconds - lastTimeShot) / frequency);
                if (timesToFire > 0)
                {
                    for (int i = 0; i < timesToFire; i++)
                    {
                        float angle = particleAngle + (random.NextSingle() - 0.5f) * spread / 2;
                        Game1.activeParticles.Add(new Particle(position, particleVelocity, angle, particleScale, textureIndex, lifetime));
                    }
                    lastTimeShot = Game1.gameTime.TotalGameTime.TotalSeconds;
                }
            }
            else
            {

            }
            
        }
        public void Destroy()
        {
            Game1.activeParticleEmitters.Remove(this);
        }

        public ParticleEmitter(Vector2 position, float velocity, float angle, float scale, ushort textureIndex, float lifetime, float frequency, float emitterDuration, float spread) 
        { 
            this.position = position;
            particleVelocity = velocity;
            particleAngle = angle;
            particleScale = scale;
            this.lifetime = lifetime;
            this.textureIndex = textureIndex;
            this.frequency = frequency;
            expiryTime = Game1.gameTime.TotalGameTime.TotalSeconds + emitterDuration;
            lastTimeShot = Game1.gameTime.TotalGameTime.TotalSeconds;
            random = new Random();
            this.spread = spread;
        }
    }
}
