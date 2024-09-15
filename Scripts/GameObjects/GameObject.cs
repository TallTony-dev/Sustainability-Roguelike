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
        internal AnimationHandler animationHandler;
        public GameObject(ushort textureIndex)
        {
            this.textureIndex = textureIndex;
            animationHandler = new AnimationHandler(textureIndex);
            animationHandler.SetAnimation(0);
        }
        public void UpdateAnimation()
        {
            textureIndex = animationHandler.Update();
        }
    }
}
