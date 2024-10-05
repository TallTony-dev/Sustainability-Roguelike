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

        public AnimatedParticle(Vector2 position, float xvelocity, float yvelocity, float rotation, float scale, float speed, ushort animationIndex, float lifetime, float xAcceleration, float yAcceleration, float drawRotation, bool rotatesParticle) : base(position, xvelocity, yvelocity, rotation, scale, 0, lifetime, xAcceleration, yAcceleration, drawRotation, rotatesParticle)
        {
            animationHandler = new AnimationHandler(animationIndex);
        }
    }
}
