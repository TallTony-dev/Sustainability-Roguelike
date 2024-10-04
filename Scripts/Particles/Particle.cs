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
        internal Vector2 velocity;
        internal float rotation;
        private float scale;
        public ushort textureIndex;
        private float lifetime;
        private double timeWhenCreated;
        public Particle(Vector2 position, float xVel, float yVel, float rotation, float scale, float speed, ushort textureIndex, float lifetime)
        {
            this.position = position;
            velocity = new Vector2 (xVel, yVel);
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
            position.X += (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * velocity.X;
            position.Y += (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * velocity.Y;
            if (Game1.gameTime.TotalGameTime.TotalSeconds > lifetime + timeWhenCreated)
                Destroy();
        }
    }
}
