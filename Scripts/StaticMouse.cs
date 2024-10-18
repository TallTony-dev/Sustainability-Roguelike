using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts
{
    /// <summary>
    /// Use isClicked to check for if the mouse has been clicked, but will only count for one frame
    /// </summary>
    public static class StaticMouse
    {
        public static bool isClicked;
        public static bool isContinuouslyPressed;

        static bool wasClickedLastFrame;

        public static void InterruptClick()
        {
            isContinuouslyPressed = false;
        }

        public static void Update()
        {
            MouseState mstate = Mouse.GetState();
            if (!wasClickedLastFrame && mstate.LeftButton == ButtonState.Pressed)
            {
                isClicked = true;
                wasClickedLastFrame = true;
                isContinuouslyPressed = true;
            }
            else if (mstate.LeftButton == ButtonState.Pressed)
            {
                isClicked = false;
            }
            else
            {
                wasClickedLastFrame = false;
                isContinuouslyPressed = false;
                isClicked = false;
            }
        }
    }
}
