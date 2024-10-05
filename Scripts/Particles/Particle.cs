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
        internal float velocity;
        internal float rotation;
        public float scale;
        public ushort textureIndex;
        private float lifetime;
        private double timeWhenCreated;
        public Particle(Vector2 position, float velocity, float rotation, float scale, ushort textureIndex, float lifetime)
        {
            this.position = position;
            this.velocity = velocity;
            this.rotation = rotation;
            this.scale = scale;
            this.lifetime = lifetime;
            this.textureIndex = textureIndex;
            timeWhenCreated = Game1.gameTime.TotalGameTime.TotalSeconds;
            Game1.activeParticles.Add(this);
        }
        public void Destroy()
        {
            Game1.activeParticles.Remove(this);
        }
        public virtual void Update()
        {
            position.X += (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * (float)Math.Cos(rotation) * velocity;
            position.Y += (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * (float)Math.Sin(rotation) * velocity;
            if (Game1.gameTime.TotalGameTime.TotalSeconds > lifetime + timeWhenCreated)
                Destroy();
        }
    }
}
