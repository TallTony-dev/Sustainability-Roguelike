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
        public Button(ushort textureIndex, int xOffset, int yOffset, Rectangle hitBox) : base(textureIndex, xOffset, yOffset, hitBox)
        {
        }

        private double timeSincePressed;
        public override bool IsPressed()
        {
            if (isEnabled)
            {
                MouseState mstate = Mouse.GetState();
                Game1.debugText = mstate.X.ToString() + mstate.Y.ToString();
                if ((Game1.gameTime.TotalGameTime.TotalSeconds - timeSincePressed) > 1 && mstate.LeftButton == ButtonState.Pressed
                    && mstate.Y < hitBox.bottom && mstate.Y > hitBox.top && mstate.X < hitBox.right && mstate.X > hitBox.left)
                {
                    timeSincePressed = Game1.gameTime.TotalGameTime.TotalSeconds;
                    return true;
                }
            }
            return false;
        }
       
    }
}
