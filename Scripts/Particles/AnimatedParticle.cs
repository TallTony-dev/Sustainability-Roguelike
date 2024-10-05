using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class AnimatedParticle : Particle
    {
        private AnimationHandler animationHandler;

        public override void Update()
        {
            (textureIndex, position, rotation) = animationHandler.Update(position, rotation);
            base.Update();
        }

        public AnimatedParticle(Vector2 position, float velocity, float rotation, float scale, float speed, ushort animationIndex, float lifetime, float xAcceleration, float yAcceleration) : base(position, velocity, rotation, scale, 0, lifetime, xAcceleration, yAcceleration)
        {
            animationHandler = new AnimationHandler(animationIndex);
        }
    }
}
