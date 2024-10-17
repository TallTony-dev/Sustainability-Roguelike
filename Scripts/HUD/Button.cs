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
            startingTextureIndex = textureIndex;
        }
        ushort startingTextureIndex;

        private double timeSincePressed;
        private double timeSinceUpdatedTexture = 0;
        public override bool IsPressed()
        {
            if (isEnabled)
            {
                MouseState mstate = Mouse.GetState();
                if (mstate.Y < hitBox.bottom && mstate.Y > hitBox.top && mstate.X < hitBox.right && mstate.X > hitBox.left)
                {
                    if (textureIndex < startingTextureIndex + 3)
                    {
                        if (Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceUpdatedTexture > 0.1)
                        {
                            textureIndex++;
                            timeSinceUpdatedTexture = Game1.gameTime.TotalGameTime.TotalSeconds;
                        }
                    }


                    if ((Game1.gameTime.TotalGameTime.TotalSeconds - timeSincePressed) > 1 && mstate.LeftButton == ButtonState.Pressed)
                    {
                        timeSincePressed = Game1.gameTime.TotalGameTime.TotalSeconds;
                        return true;
                    }
                }
                else
                {
                    if (textureIndex > startingTextureIndex)
                    {
                        if (Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceUpdatedTexture > 0.1)
                        {
                            textureIndex--;
                            timeSinceUpdatedTexture = Game1.gameTime.TotalGameTime.TotalSeconds;
                        }
                    }
                }
            }
            return false;

        }
       
    }
}
