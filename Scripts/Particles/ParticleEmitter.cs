using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class ParticleEmitter
    {
        public Vector2 position { get; set; }
        public float particlexVelocity;
        public float particleyVelocity;
        float particleAngle;
        internal float drawRotation;
        float particleScale;
        float lifetime;
        ushort textureIndex;
        float frequency;
        double lastTimeShot;
        Random random;
        double expiryTime;
        public float xAcceleration;
        public float yAcceleration;
        bool rotatesParticle;

        public bool isEnabled = false;
        float spread;

        public void Update()
        {
            if (!(Game1.gameTime.TotalGameTime.TotalSeconds > expiryTime) && isEnabled)
            {
                int timesToFire = (int)((Game1.gameTime.TotalGameTime.TotalSeconds - lastTimeShot) / frequency);
                if (timesToFire > 0)
                {
                    for (int i = 0; i < timesToFire; i++)
                    {
                        float randomVal = (random.NextSingle() - 0.5f) * spread / 57.295f;
                        float angle = particleAngle + randomVal; //converts radians to degrees and halves
                        float rngXVelocity = particlexVelocity + randomVal * 10;
                        float rngYVelocity = particleyVelocity + randomVal * 10;
                        Game1.activeParticles.Add(new Particle(position, rngXVelocity, rngYVelocity, angle, particleScale, textureIndex, lifetime, xAcceleration, yAcceleration, drawRotation, rotatesParticle));
                    }
                    lastTimeShot = Game1.gameTime.TotalGameTime.TotalSeconds;
                }
            }
            else if (Game1.gameTime.TotalGameTime.TotalSeconds > expiryTime)
            {
                Destroy();
            }
            else
                lastTimeShot = Game1.gameTime.TotalGameTime.TotalSeconds;

        }
        public void Destroy()
        {
            Game1.activeParticleEmitters.Remove(this);
        }

        public ParticleEmitter(Vector2 position, float velocity, float angle, float scale, ushort textureIndex, float lifetime, float frequency, float emitterDuration, float spread, float xAccel, float yAccel, float drawRotation, bool rotatesParticle) 
        { 
            this.position = position;
            particlexVelocity = (float)Math.Cos(angle) * velocity;
            particleyVelocity = (float)Math.Sin(angle) * velocity;
            particleAngle = angle;
            particleScale = scale;
            this.lifetime = lifetime;
            this.textureIndex = textureIndex;
            this.frequency = frequency;
            this.drawRotation = drawRotation;
            this.rotatesParticle = rotatesParticle;
            if (Game1.gameTime != null)
            {
                expiryTime = Game1.gameTime.TotalGameTime.TotalSeconds + emitterDuration;
                lastTimeShot = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            else
                expiryTime = emitterDuration;
                
            random = new Random();
            this.spread = spread;
            yAcceleration = yAccel;
            xAcceleration = xAccel;
            if (!Game1.activeParticleEmitters.Contains(this))
                Game1.activeParticleEmitters.Add(this);
        }
    }
}
