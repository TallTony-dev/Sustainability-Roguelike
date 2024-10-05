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
        public Particle(Vector2 position, float velocity, float rotation, float scale, ushort textureIndex, float lifetime, float xAcceleration, float yAcceleration)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.lifetime = lifetime;
            this.textureIndex = textureIndex;
            timeWhenCreated = Game1.gameTime.TotalGameTime.TotalSeconds;
            Game1.activeParticles.Add(this);
            this.xAcceleration = xAcceleration;
            this.yAcceleration = yAcceleration;
            xVelocity = (float)Math.Cos(rotation) * velocity;
            yVelocity = (float)Math.Sin(rotation) * velocity;
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
            if (Game1.gameTime.TotalGameTime.TotalSeconds > lifetime + timeWhenCreated)
                Destroy();
        }
    }
}
