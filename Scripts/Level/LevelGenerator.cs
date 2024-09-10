using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;

namespace Monogame_Cross_Platform.Scripts.Level
{
    internal static class LevelGenerator
    {
        public static Room[,] rooms = new Room[7,7];
        /// <summary>
        /// Edits the static class tilemap to fit the current level selected by param levelNumber
        /// </summary>
        public static void GenerateLevel(byte levelType)
        {
            if (levelType == 1)
            {
                for(var x = 0; x < 7; x++)
                {
                    for (var y = 0; y < 7; y++)
                    {
                        Room room = rooms[x, y];
                        room = new Room(0);
                        room.SettleTiles();
                        for (var tileY = 0; tileY < 32; tileY++)
                        {
                            for (var tileX = 0; tileX < 32; tileX++)
                            {
                                TileMap.tileMap[x*32 + tileX, y*32 + tileY] = room.room[tileX, tileY];
                            }
                        }
                    }
                }
            }
        }


    }
}
