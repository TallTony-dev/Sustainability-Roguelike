using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class ParticleEmitter
    {

        public Vector2 position { get; set; }
        public float velocity;
        public float particleAngle;
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
        float? ySettlingOffset = null;

        ushort? particleAnimationIndex = null; //selects the line
        int? particleAnimationNumber = null; //selects from the same line

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
                        float randomVal = (random.NextSingle() - 0.5f) * 2;
                        float angle = particleAngle + randomVal * spread;
                        float rngXVelocity = (float)Math.Cos(angle) * velocity + randomVal;
                        float rngYVelocity = (float)Math.Sin(angle) * velocity + randomVal / 10;
                        float? rngYSettlingOffset = ySettlingOffset + randomVal * 40;

                        if (particleAnimationIndex == null)
                            Game1.activeParticles.Add(new Particle(position, rngXVelocity, rngYVelocity, angle, particleScale, textureIndex, lifetime, xAcceleration, yAcceleration, drawRotation, rotatesParticle, position.Y + rngYSettlingOffset));
                        else
                        {
                            AnimatedParticle animatedParticle = new AnimatedParticle(position, rngXVelocity, rngYVelocity, angle, particleScale, (ushort)particleAnimationIndex, lifetime, xAcceleration, yAcceleration, drawRotation, rotatesParticle, position.Y + rngYSettlingOffset);
                            if (particleAnimationNumber != null)
                                animatedParticle.animationHandler.SetTextureAnimation((int)particleAnimationNumber);
                            Game1.activeParticles.Add(animatedParticle);

                        }
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
        public void SetParticleAnimationNumber(int animationNumber)
        {
            particleAnimationNumber = animationNumber;
        }
        public void Destroy()
        {
            Game1.activeParticleEmitters.Remove(this);
        }

        public ParticleEmitter(Vector2 position, float velocity, float angle, float scale, ushort textureIndex, float lifetime, float frequency, float emitterDuration, float spread, float xAccel, float yAccel, float drawRotation, bool rotatesParticle) 
        { 
            this.position = position;
            this.velocity = velocity;
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

        public ParticleEmitter(Vector2 position, float velocity, float angle, float scale, ushort textureIndex, float lifetime, float frequency, float emitterDuration, float spread, float xAccel, float yAccel, float drawRotation, bool rotatesParticle, float ySettlingOffset)
        {
            this.position = position;
            this.velocity = velocity;
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
            this.ySettlingOffset = ySettlingOffset;
        }
        public ParticleEmitter(Vector2 position, float velocity, float angle, float scale, ushort textureIndex, float lifetime, float frequency, float emitterDuration, float spread, float xAccel, float yAccel, float drawRotation, bool rotatesParticle, float ySettlingOffset, ushort particleAnimIndex)
        {
            this.position = position;
            this.velocity = velocity;
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
            this.ySettlingOffset = ySettlingOffset;
            particleAnimationIndex = particleAnimIndex;
        }
    }
}
