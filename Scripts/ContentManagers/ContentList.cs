﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    internal class ContentList
    {
        public static int animationIndexOffset = 0;


        //Note that strings can be the same and will just create references to the loaded texture
        private List<(string textureName, Rectangle sourceRect, ushort index)> AlwaysLoadedToLoad = new List<(string, Rectangle, ushort)>() { };
        private List<(string textureName, Rectangle sourceRect, ushort index)> OtherTexturesToLoad = new List<(string, Rectangle, ushort)>() { };
        private List<(string textureName, Rectangle sourceRect, ushort index)> CityTexturesToLoad = new List<(string, Rectangle, ushort)>() { };

        //Arrays of all textures to load, texturetype indicates how the texture should be loaded, 0 loads as a tileset (currently 4x4 like cooblestone texture, TODO: make type 1 load a spritesheet (figure out how)

        static ushort tileTextureCount = 119;
        private (string textureName, ushort startingIndex, byte textureType)[] AlwaysLoadedTextures = { ("cobblestone", 0, 0), ("cobblestone wall", 30, 0), ("ice floor", 60, 0), ("ice wall", 90, 0), ("electricWeapon", (ushort)(tileTextureCount + 1), 2) };

        private (string textureName, ushort startingIndex, byte textureType)[] OtherTextures = { ("minimapBG", 1, 1), ("minimapBG", 1, 1), ("hotBar", 2, 1), ("quitButton", 3, 3), ("startButton", 7, 3), ("resumeButton", 11, 3), ("menuButton", 15, 3), ("settingsButton", 19, 3)
                                                                                                ,("resolutionBG", 23, 1), ("activeResolutions", 24, 4), ("inactiveResolutions", 34, 4), ("applyButton", 44, 3), ("fullscreenButton", 48, 3), ("minimapIcons", 52, 5)
                                                                                                ,("weaponMeterFront", 58, 1), ("weaponMeterBack", 59, 1), ("healthBarFront", 60, 1), ("healthBarBack", 61, 1), ("plusButtonActive", 62, 1), ("plusButtonInactive", 63, 1), ("minusButtonActive", 64, 1), ("minusButtonInactive", 65, 1),("volumeActive", 66, 1), ("volumeInactive", 67, 1)
                                                                                                ,("VolumeBG", 68, 1), ("potion", 69, 6), ("youLose", 73, 1), ("youWin", 74, 1)};
        private (string textureName, ushort startingIndex, byte textureType)[] CityTextures = { };
        private List<(string textureName, ushort index)> Audio = new List<(string textureName, ushort index)>() { ("Laser_Shoot", 0), ("Explosion", 1) };

        public List<(string textureName, Rectangle sourceRect, ushort index)> GetAlwaysLoadedTextures()
        {
            PrepTextureSet(AlwaysLoadedTextures, AlwaysLoadedToLoad);

            return AlwaysLoadedToLoad; 
        }
        public List<(string textureName, Rectangle sourceRect, ushort index)> GetOtherTextures()
        {
            PrepTextureSet(OtherTextures, OtherTexturesToLoad);

            return OtherTexturesToLoad;
        }
        public List<(string textureName, Rectangle sourceRect, ushort index)> GetCityTextures()
        {
            PrepTextureSet(CityTextures, CityTexturesToLoad);

            return CityTexturesToLoad;
        }
        public List<(string textureName, ushort index)> GetAudio()
        {
            return (Audio);
        }
        private void PrepTextureSet((string textureName, ushort startingIndex, byte textureType)[] textureSetToLoad, List<(string textureName, Rectangle sourceRect, ushort index)> TextureListToLoad)
        {
            foreach ((string textureName, ushort startingIndex, ushort textureType) in textureSetToLoad)
            {
                if (textureType == 0)
                {
                    for (ushort y = 0; y < 4; y++) //selects row
                    {
                        for (ushort x = 0; x < 4; x++) //selects column
                        {
                            TextureListToLoad.Add((textureName, new(x * 32, y * 32, 32, 32), (ushort)(startingIndex + (y * 4) + x)));
                        }
                    }
                    for (ushort y = 0; y < 2; y++) //selects row
                    {
                        for (ushort x = 0; x < 2; x++) //selects column
                        {
                            TextureListToLoad.Add((textureName, new(x * 32 + 128, y * 32, 32, 32), (ushort)(startingIndex + (y * 2) + x + 16)));
                        }
                    }
                }
                else if (textureType == 1)
                {
                    TextureListToLoad.Add((textureName, Rectangle.Empty, (ushort)(startingIndex)));
                }
                else if (textureType == 2)
                {
                    for (ushort y = 0; y < 4; y++) //selects row
                    {
                        for (ushort x = 0; x < 4; x++) //selects column
                        {
                            TextureListToLoad.Add((textureName, new(x * 32, y * 32, 32, 32), (ushort)(startingIndex + (y * 4) + x)));
                        }
                    }
                }
                else if (textureType == 3)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        TextureListToLoad.Add((textureName, new(x * 146, 0, 146, 60), (ushort)(startingIndex + x)));
                    }
                }
                else if (textureType == 4)
                {
                    for (ushort y = 0; y < 5; y++) //selects row
                    {
                        for (ushort x = 0; x < 2; x++) //selects column
                        {
                            TextureListToLoad.Add((textureName, new(x * 36, y * 7, 36, 7), (ushort)(startingIndex + (y * 2) + x)));
                        }
                    }
                }
                else if (textureType == 5)
                {
                    for (ushort y = 0; y < 2; y++) //selects row
                    {
                        for (ushort x = 0; x < 3; x++) //selects column
                        {
                            TextureListToLoad.Add((textureName, new(x * 20, y * 20, 20, 20), (ushort)(startingIndex + (y * 3) + x)));
                        }
                    }
                }
                else if (textureType == 6)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        TextureListToLoad.Add((textureName, new(x * 24, 0, 24, 24), (ushort)(startingIndex + x)));
                    }
                }
            }
        }
    }
}
