using System;
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
        private (string textureName, ushort startingIndex, byte textureType)[] AlwaysLoadedTextures = { ("cobblestone", 0, 0), ("cobblestone wall testing", 30, 0), ("cobblestoneRecolor", 60, 0), ("electricWeapon", 90, 2)};
        private (string textureName, ushort startingIndex, byte textureType)[] OtherTextures = { ("minimapBG", 1, 1), ("hotBar", 2, 1)};
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
                if (textureType == 1)
                {
                    TextureListToLoad.Add((textureName, Rectangle.Empty, (ushort)(startingIndex)));
                }
                if (textureType == 2)
                {
                    for (ushort y = 0; y < 4; y++) //selects row
                    {
                        for (ushort x = 0; x < 4; x++) //selects column
                        {
                            TextureListToLoad.Add((textureName, new(x * 32, y * 32, 32, 32), (ushort)(startingIndex + (y * 4) + x)));
                        }
                    }
                }
            }
        }
    }
}
