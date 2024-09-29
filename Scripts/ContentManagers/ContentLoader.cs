using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    /// <summary>
    /// Loads content from files.
    /// </summary>
    internal class ContentLoader
    {
        public static (Texture2D, Rectangle)[] tileTexturesLoaded = new (Texture2D, Rectangle)[64]; //Change this array size to the max number of tile textures and allocate different ranges for different textures
        ContentList contentList = new ContentList();
        Game game;
        ContentManager alwaysLoadedcontentManager;

        public ContentLoader(Game game)
        {
            this.game = game;
            alwaysLoadedcontentManager = new ContentManager(game.Services);
        }
        
        /// <summary>
        /// Loads textures from a given ContentList property given as a string
        /// </summary>
        public void LoadTextures(string contentListToLoad)
        {
            if (contentListToLoad == "AlwaysLoaded")
                foreach ((string, Rectangle, ushort) texture in contentList.GetAlwaysLoadedTextures())
                {
                    tileTexturesLoaded[texture.Item3] = (alwaysLoadedcontentManager.Load<Texture2D>("C:/Users/User/source/repos/Monogame Cross Platform/bin/Debug/net6.0/Content/Graphics/" + texture.Item1), texture.Item2);
                }
            else if (contentListToLoad == "HomeTextures")
                foreach ((string, Rectangle, ushort) texture in contentList.GetHomeTextures())
                {
                    tileTexturesLoaded[texture.Item3] = (game.Content.Load<Texture2D>("C:/Users/User/source/repos/Monogame Cross Platform/bin/Debug/net6.0/Content/Graphics/" + texture.Item1), texture.Item2);
                }
            else if (contentListToLoad == "CityTextures")
                foreach ((string, Rectangle, ushort) texture in contentList.GetCityTextures())
                {
                    tileTexturesLoaded[texture.Item3] = (game.Content.Load<Texture2D>("C:/Users/User/source/repos/Monogame Cross Platform/bin/Debug/net6.0/Content/Graphics/" + texture.Item1), texture.Item2);
                }
            else Console.WriteLine("Invalid string loaded check where you call LoadTextures as you are loading a non existant ContentList");
        }

        public void UnloadTextures()
        {
            game.Content.Unload();
        }

        /// <summary>
        /// Gets a Texture2D of a specific index from loaded textures, BE SURE TO CALL LOADTEXTURES BEFORE CALLING THIS OR ELSE WILL RETURN BAD THINGS (idek what it would be lmao but nasty ass numbers)
        /// </summary>
        public static (Texture2D, Rectangle) GetLoadedTileTexture(ushort index)
        {
            return tileTexturesLoaded[index];
        }
        public static Rectangle GetTextureRectangle(ushort index)
        {
            return tileTexturesLoaded[index].Item2;
        }

    }
}
