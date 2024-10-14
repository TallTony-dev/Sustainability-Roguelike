using Monogame_Cross_Platform.Scripts.Hitboxes;
using Monogame_Cross_Platform.Scripts.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Objects
{
    internal class BreakableObject : GameObject
    {
        public Hitbox Hitbox { get => new Hitbox(position.X, position.Y, animationHandler.GetRectangleSize().X, animationHandler.GetRectangleSize().Y); }
        public void Break()
        {
            ParticleEmitter particleEmitter = new ParticleEmitter(position, 60, -90, 0.4f, textureIndex, 1, 0.02f, 0.4f, 180, 0, 100, 0, true, 10, ObjectIndex);
            particleEmitter.SetParticleAnimationNumber(1);
            particleEmitter.isEnabled = true;
            Destroy();
        }

        public BreakableObject(ushort animIndex, Vector2 startingTile) : base(animIndex, startingTile)
        {
            ObjectIndex = animIndex; //object index doubles as this
        }
    }
}
