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
        private List<(string textureName, Rectangle sourceRect, ushort index)> HomeTexturesToLoad = new List<(string, Rectangle, ushort)>() { };
        private List<(string textureName, Rectangle sourceRect, ushort index)> CityTexturesToLoad = new List<(string, Rectangle, ushort)>() { };

        //Arrays of all textures to load, texturetype indicates how the texture should be loaded, 0 loads as a tileset (currently 4x4 like cooblestone texture, TODO: make type 1 load a spritesheet (figure out how)
        private (string textureName, ushort startingIndex, byte textureType)[] AlwaysLoadedTextures = { ("cobblestone", 0, 0), ("cobblestone wall testing", 16, 0), ("cobblestoneRecolor", 32, 0)};
        private (string textureName, ushort startingIndex, byte textureType)[] HomeTextures = { };
        private (string textureName, ushort startingIndex, byte textureType)[] CityTextures = { };

        public List<(string textureName, Rectangle sourceRect, ushort index)> GetAlwaysLoadedTextures()
        {
            PrepTextureSet(AlwaysLoadedTextures, AlwaysLoadedToLoad);
            //add any solo textures here

            return AlwaysLoadedToLoad; 
        }
        public List<(string textureName, Rectangle sourceRect, ushort index)> GetHomeTextures()
        {
            PrepTextureSet(HomeTextures, HomeTexturesToLoad);
            //add any solo textures here

            return HomeTexturesToLoad;
        }
        public List<(string textureName, Rectangle sourceRect, ushort index)> GetCityTextures()
        {
            PrepTextureSet(CityTextures, CityTexturesToLoad);
            return CityTexturesToLoad;
        }


        public void PrepTextureSet((string textureName, ushort startingIndex, byte textureType)[] textureSetToLoad, List<(string textureName, Rectangle sourceRect, ushort index)> TextureListToLoad)
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
                }
            }
        }
    }
}
