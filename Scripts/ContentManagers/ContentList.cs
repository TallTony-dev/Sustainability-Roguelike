using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    internal class ContentList
    {
        //Note that strings can be the same and will just create references to the loaded texture
        private List<(string textureName, Rectangle sourceRect, ushort index)> AlwaysLoadedToLoad = new List<(string, Rectangle, ushort)>() { };
        private List<(string textureName, Rectangle sourceRect, ushort index)> HomeTexturesToLoad = new List<(string, Rectangle, ushort)>() { };
        private List<(string textureName, Rectangle sourceRect, ushort index)> CityTexturesToLoad = new List<(string, Rectangle, ushort)>() { };

        //Arrays of TileSets (4x4 grids of 32x32 textures)
        private (string textureName, ushort startingIndex)[] AlwaysLoadedTileSets = { ("cobblestone", 0), ("cobblestone", 16) };
        private (string textureName, ushort startingIndex)[] HomeTexturesTileSets = { };
        private (string textureName, ushort startingIndex)[] CityTexturesTileSets = { };

        public List<(string textureName, Rectangle sourceRect, ushort index)> GetAlwaysLoadedTextures()
        {
            PrepTileSet(AlwaysLoadedTileSets, AlwaysLoadedToLoad);
            //AlwaysLoadedToLoad.Add(("ball", new(0,0,64,64), 0)); 

            return AlwaysLoadedToLoad; 
        }
        public List<(string textureName, Rectangle sourceRect, ushort index)> GetHomeTextures()
        {
            PrepTileSet(HomeTexturesTileSets, HomeTexturesToLoad);
            return HomeTexturesToLoad;
        }
        public List<(string textureName, Rectangle sourceRect, ushort index)> GetCityTextures()
        {
            PrepTileSet(CityTexturesTileSets, CityTexturesToLoad);
            return CityTexturesToLoad;
        }

        /// <summary>
        /// Puts a TileSet into a TileList that will be sent off to load, Textures must be 32x32 and in a 4x4 grid. CurrentIndex is inited and stored in the method that gets a list of textures in which this method will be called, as it 
        /// </summary>
        public void PrepTileSet((string textureName, ushort startingIndex)[] tileSetToLoad, List<(string textureName, Rectangle sourceRect, ushort index)> TextureListToLoad)
        {
            foreach ((string textureName, ushort startingIndex) in tileSetToLoad)
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
