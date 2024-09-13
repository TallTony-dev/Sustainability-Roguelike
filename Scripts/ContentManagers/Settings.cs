using System;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    internal static class Settings
    {
        public static bool isFullScreen = false;
        public static int resolutionWidth = 2560;
        public static int resolutionHeight = 1440;
        public static float uiScaleX = 1f;
        public static float uiScaleY = 1f;
        public static float zoomLevel = 2.3f;

        static MouseState mstate;

        public static Menu settingsMenu = new Menu(Menu.MenuType.settings);

        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                settingsMenu.EnableMenu();
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                settingsMenu.DisableMenu();

            if (settingsMenu.IsButtonPressed(0))
            {
                ApplySettingsToFile();
                InitializeSettings();
            }
            if (settingsMenu.IsButtonPressed(1))
            {
                isFullScreen = true;
            }
            if (settingsMenu.IsButtonPressed(2))
            {
                isFullScreen = false;
            }
        }
        public static void ApplySettingsToFile()
        {
            File.WriteAllText("Settings.txt", $"{resolutionHeight},{resolutionWidth},{isFullScreen}");
        }
        public static void InitializeSettings()
        {
            string[] tokens = File.ReadAllText("Settings.txt").Split(",");
            resolutionHeight = Convert.ToInt32(tokens[0]);
            resolutionWidth = Convert.ToInt32(tokens[1]);
            isFullScreen = Convert.ToBoolean(tokens[2]);

            Game1._graphics.IsFullScreen = isFullScreen; 
            Game1._graphics.PreferredBackBufferWidth = resolutionWidth;
            Game1._graphics.PreferredBackBufferHeight = resolutionHeight;
            Game1._graphics.ApplyChanges();

            uiScaleX = 1 * ((float)resolutionWidth / 1920);
            uiScaleY = 1 * ((float)resolutionHeight / 1080);
            foreach (Menu menu in Game1.activeMenus)
            {
                menu.UpdateElementsPositions();
            }
        }
        public static void UpdateZoom()
        {
            MouseState newMouseState = Mouse.GetState();
            int deltaScroll = newMouseState.ScrollWheelValue - mstate.ScrollWheelValue;
            if (deltaScroll > 0 && zoomLevel < 4)
                zoomLevel += 0.06f * (zoomLevel);
            else if (deltaScroll < 0 && zoomLevel > 2)
                zoomLevel -= 0.06f * (zoomLevel);
            mstate = newMouseState;
        }   
    }
}
