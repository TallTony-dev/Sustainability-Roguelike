using System;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    internal class Settings
    {
        public static int resolutionWidth = 2560;
        public static int resolutionHeight = 1440;
        public static float zoomLevel = 2.3f;
        MouseState mstate;
        public void UpdateZoom()
        {
            MouseState newMouseState = Mouse.GetState();
            int deltaScroll = newMouseState.ScrollWheelValue - mstate.ScrollWheelValue;
            if (deltaScroll > 0 && zoomLevel < 4)
                zoomLevel += 0.06f * (zoomLevel);
            else if (deltaScroll < 0 && zoomLevel > 2)
                zoomLevel -= 0.06f * (zoomLevel);
            mstate = newMouseState;
        }
        public Settings() { }
    }
}
