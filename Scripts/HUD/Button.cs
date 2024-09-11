using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class Button : UiElement
    {
        public Button(ushort textureIndex, Rectangle hitBox, int xOffset, int yOffset) : base(textureIndex, xOffset, yOffset, hitBox)
        {
        }

        private double timeSincePressed;
        public bool IsPressed()
        {
            MouseState mstate = Mouse.GetState();
            if ((Game1.gameTime.TotalGameTime.TotalSeconds - timeSincePressed) > 1 && mstate.LeftButton == ButtonState.Pressed && mstate.Y < hitBox.Bottom && mstate.Y > hitBox.Top && mstate.X < hitBox.Right && mstate.X > hitBox.Left)
            {
                timeSincePressed = Game1.gameTime.TotalGameTime.TotalSeconds;
                return true;
            }
            else return false;
        }
       
    }
}
