using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects
{
    internal class GameObject
    {  
        public ushort textureIndex { get; set; }
        public GameObject(ushort textureIndex)
        {
            this.textureIndex = textureIndex;
        }
    }
}
