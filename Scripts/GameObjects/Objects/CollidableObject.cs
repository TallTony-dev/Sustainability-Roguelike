using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Objects
{
    internal class CollidableObject : GameObject
    {
        public bool isBarrier = true;
        public CollidableObject(ushort textureIndex, Vector2 startingPos) : base(textureIndex, startingPos)
        {

        }
    }
}
