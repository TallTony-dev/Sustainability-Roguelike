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

        public AnimatedParticle(Vector2 position, float xVel, float yVel, float rotation, float scale, float speed, ushort animationIndex, float lifetime) : base(position, xVel, yVel, rotation, scale, speed, 0, lifetime)
        {
            animationHandler = new AnimationHandler(animationIndex);
        }
    }
}
