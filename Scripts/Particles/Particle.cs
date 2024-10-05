using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class Particle
    {
        internal Vector2 position;
        internal float xVelocity;
        internal float yVelocity;
        internal float rotation;
        public float scale;
        public ushort textureIndex;
        private float lifetime;
        private double timeWhenCreated;
        private float xAcceleration;
        private float yAcceleration;
        bool rotatesParticle;
        public Particle(Vector2 position, float xvelocity, float yvelocity, float rotation, float scale, ushort textureIndex, float lifetime, float xAcceleration, float yAcceleration, float drawRotation, bool rotatesParticle)
        {
            this.position = position;
            this.rotation = drawRotation;
            this.scale = scale;
            this.lifetime = lifetime;
            this.textureIndex = textureIndex;
            timeWhenCreated = Game1.gameTime.TotalGameTime.TotalSeconds;
            Game1.activeParticles.Add(this);
            this.xAcceleration = xAcceleration;
            this.yAcceleration = yAcceleration;
            xVelocity = xvelocity;
            yVelocity = yvelocity;
            this.rotatesParticle = rotatesParticle;
        }
        public void Destroy()
        {
            Game1.activeParticles.Remove(this);
        }
        public virtual void Update()
        {
            float elapsedTime = (float)Game1.gameTime.ElapsedGameTime.TotalSeconds;
            xVelocity += elapsedTime * xAcceleration;
            yVelocity += elapsedTime * yAcceleration; 
            position.X += elapsedTime * xVelocity;
            position.Y += elapsedTime * yVelocity;
            if (rotatesParticle)
            {
                rotation = (float)Math.Atan2(yVelocity, xVelocity) - 1.5708f;
            }

            if (Game1.gameTime.TotalGameTime.TotalSeconds > lifetime + timeWhenCreated)
                Destroy();
        }
    }
}
