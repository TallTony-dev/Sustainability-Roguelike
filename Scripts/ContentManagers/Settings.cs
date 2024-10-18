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


        static int activeResolutionButton;

        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                settingsMenu.EnableMenu();
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                settingsMenu.DisableMenu();
            if (settingsMenu.isActive)
            {
                for (int i = 0; i < 10; i++)
                {
                    Button button = (Button)settingsMenu.elements[1 + i];
                    if (button.IsHovered() || 1 + i == activeResolutionButton)
                    {
                        button.textureIndex = (ushort)(10 + button.startingTextureIndex);
                    }
                    else
                    {
                        button.textureIndex = button.startingTextureIndex;
                    }
                }
                if (settingsMenu.IsButtonPressed(1))
                {
                    resolutionHeight = 2160;
                    resolutionWidth = 3840;
                    activeResolutionButton = 1;
                }
                else if (settingsMenu.IsButtonPressed(2))
                {
                    resolutionHeight = 1080;
                    resolutionWidth = 1920;
                    activeResolutionButton = 2;
                }
                else if (settingsMenu.IsButtonPressed(3))
                {
                    resolutionHeight = 1440;
                    resolutionWidth = 3440;
                    activeResolutionButton = 3;
                }
                else if (settingsMenu.IsButtonPressed(4))
                {
                    resolutionHeight = 900;
                    resolutionWidth = 1600;
                    activeResolutionButton = 4;
                }
                else if (settingsMenu.IsButtonPressed(5))
                {
                    resolutionHeight = 1600;
                    resolutionWidth = 2560;
                    activeResolutionButton = 5;
                }
                else if (settingsMenu.IsButtonPressed(6))
                {
                    resolutionHeight = 900;
                    resolutionWidth = 1440;
                    activeResolutionButton = 6;
                }
                else if (settingsMenu.IsButtonPressed(7))
                {
                    resolutionHeight = 1440;
                    resolutionWidth = 2560;
                    activeResolutionButton = 7;
                }
                else if (settingsMenu.IsButtonPressed(8))
                {
                    resolutionHeight = 768;
                    resolutionWidth = 1366;
                    activeResolutionButton = 8;
                }
                else if (settingsMenu.IsButtonPressed(9))
                {
                    resolutionHeight = 1080;
                    resolutionWidth = 2560;
                    activeResolutionButton = 9;
                }
                else if (settingsMenu.IsButtonPressed(10))
                {
                    resolutionHeight = 768;
                    resolutionWidth = 1360;
                    activeResolutionButton = 10;
                }
                if (settingsMenu.IsButtonPressed(11))
                {
                    ApplySettingsToFile();
                    InitializeSettings();
                }
                if (settingsMenu.IsButtonPressed(12))
                {
                    Game1._graphics.ToggleFullScreen();
                    //isFullScreen = !isFullScreen;
                    //ApplySettingsToFile();
                    //InitializeSettings();
                }
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

            uiScaleX = 1 * ((float)resolutionWidth / 480);
            uiScaleY = 1 * ((float)resolutionHeight / 270);
        }
        public static void UpdateZoom()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
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
}
