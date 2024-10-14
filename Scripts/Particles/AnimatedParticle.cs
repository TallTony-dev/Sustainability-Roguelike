using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class AnimatedParticle : Particle
    {
        public AnimationHandler animationHandler;

        public override void Update()
        {
            (textureIndex, position, rotation) = animationHandler.Update(position, rotation);
            base.Update();
        }

        public AnimatedParticle(Vector2 position, float xvelocity, float yvelocity, float rotation, float scale, ushort animationIndex, float lifetime, float xAcceleration, float yAcceleration, float drawRotation, bool rotatesParticle, float? ySettlingPoint) : base(position, xvelocity, yvelocity, rotation, scale, 0, lifetime, xAcceleration, yAcceleration, drawRotation, rotatesParticle, ySettlingPoint)
        {
            animationHandler = new AnimationHandler(animationIndex);
        }
    }
}
