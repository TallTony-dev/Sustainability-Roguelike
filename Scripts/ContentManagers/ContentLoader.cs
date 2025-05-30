﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts;
using System.IO;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    /// <summary>
    /// Loads content from files.
    /// </summary>
    public class ContentLoader
    {
        private static (Texture2D albedoMap, Rectangle rectangleOfTexture, Texture2D normalMap)[] tileTexturesLoaded = new (Texture2D, Rectangle, Texture2D)[150]; //Change this array size to the max number of tile textures and allocate different ranges for different textures
        private static (Texture2D albedoMap, Rectangle rectangleOfTexture, Texture2D normalMap)[] otherTexturesLoaded = new (Texture2D, Rectangle, Texture2D)[120];
        public static SoundEffect[] audioLoaded { get; private set; } = new SoundEffect[2];
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
                    tileTexturesLoaded[texture.Item3] = (alwaysLoadedcontentManager.Load<Texture2D>("Content/Graphics/" + texture.Item1), texture.Item2, null);
                    if (Exists("NormalMaps/" + texture.Item1 + "_normal"))
                    {
                        tileTexturesLoaded[texture.Item3].normalMap = alwaysLoadedcontentManager.Load<Texture2D>("Content/NormalMaps/" + texture.Item1 + "_normal");
                    }
                }
            else if (contentListToLoad == "OtherTextures")
                foreach ((string, Rectangle, ushort) texture in contentList.GetOtherTextures())
                {
                    otherTexturesLoaded[texture.Item3] = (game.Content.Load<Texture2D>("Other/" + texture.Item1), texture.Item2, null);
                    if (Exists("NormalMaps/" + texture.Item1 + "_normal"))
                    {
                        otherTexturesLoaded[texture.Item3].normalMap = alwaysLoadedcontentManager.Load<Texture2D>("Content/NormalMaps/" + texture.Item1 + "_normal");
                    }
                }
            else if (contentListToLoad == "Audio")
            {
                foreach ((string, ushort) audio in contentList.GetAudio())
                {
                    audioLoaded[audio.Item2] = game.Content.Load<SoundEffect>("Audio/" + audio.Item1);
                }
            }
            else Console.WriteLine("Invalid string loaded check where you call LoadTextures as you are loading a non existant ContentList");
        }

        public static bool Exists(string path)
        {
            return File.Exists($@"Content\{path}.xnb");
        }

        public void UnloadTextures()
        {
            game.Content.Unload();
        }
        public static Rectangle GetTileTextureRectangle(ushort index)
        {
            return tileTexturesLoaded[index].Item2;
        }
        public static Rectangle GetOtherTextureRectangle(ushort index)
        {
            return otherTexturesLoaded[index].Item2;
        }

        /// <summary>
        /// Gets a Texture2D of a specific index from loaded textures, BE SURE TO CALL LOADTEXTURES BEFORE CALLING THIS OR ELSE WILL RETURN BAD THINGS (idek what it would be lmao but nasty ass numbers)
        /// </summary>
        public static (Texture2D albedoMap, Rectangle rectangleOfTexture, Texture2D normalMap) GetLoadedTileTexture(ushort index)
        {
            return tileTexturesLoaded[index];
        }
        public static (Texture2D albedoMap, Rectangle rectangleOfTexture, Texture2D normalMap) GetLoadedOtherTexture(ushort index)
        {
            return otherTexturesLoaded[index];
        }
        public static SoundEffect GetLoadedSoundEffect(ushort index)
        {
            return audioLoaded[index];
        }


    }
}
