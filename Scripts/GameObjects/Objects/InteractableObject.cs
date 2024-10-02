using Monogame_Cross_Platform.Scripts.Hitboxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Objects
{
    internal abstract class InteractableObject : CollidableObject
    {
        public abstract void Interact();


        public InteractableObject(ushort textureIndex, Vector2 startingPos, Vector2 hitboxSize) : base(textureIndex, startingPos, hitboxSize)
        {

        }
    }
}
