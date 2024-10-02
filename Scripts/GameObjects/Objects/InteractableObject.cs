using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Objects
{
    internal class InteractableObject : GameObject
    {
        //double timeWhenPressed = 0;
        //public void CheckIfPressed(Vector2 playerPos)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.E))
        //    {

        //    }
        //}

        public InteractableObject(ushort textureIndex, Vector2 startingPos) : base(textureIndex, startingPos)
        {

        }
    }
}
