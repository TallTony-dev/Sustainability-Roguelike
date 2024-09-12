using System.IO;
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
                        rooms[x, y] = new Room(0);
                        for (var tileY = 0; tileY < 32; tileY++)
                        {
                            for (var tileX = 0; tileX < 32; tileX++)
                            {
                                TileMap.tileMap[x*32 + tileX, y*32 + tileY] = rooms[x, y].tileArray[tileX, tileY];
                            }
                        }
                        rooms[x, y].SettleTiles();
                    }
                }
            }
        }
        public static void UpdateTileMap()
        {
            for (var x = 0; x < 7; x++)
            {
                for (var y = 0; y < 7; y++)
                {
                    for (var tileY = 0; tileY < 32; tileY++)
                    {
                        for (var tileX = 0; tileX < 32; tileX++)
                        {
                            TileMap.tileMap[x * 32 + tileX, y * 32 + tileY] = rooms[x, y].tileArray[tileX, tileY];
                        }
                    }
                    rooms[x, y].SettleTiles();
                }
            }
        }
        public static Room PosToRoom(Vector2 position)
        {
            (int x, int y) tileMapPos = TileMap.PosToAbsTileMapPos(position);
            return rooms[tileMapPos.x / 32, tileMapPos.y / 32];
        }
        public static (int, int) PosToTileOfRoom(Vector2 position)
        {
            return (TileMap.PosToAbsTileMapPos(position).Item1 % 32, TileMap.PosToAbsTileMapPos(position).Item2 % 32);
        }
        public static void ChangeTileAtPos(Vector2 position, ushort startingIndex, bool isBarrier, byte breakEffect, byte statusGiven)
        {
            (int tileX, int tileY) = PosToTileOfRoom(position);
            Room room = PosToRoom(position);
            room.SetTile(tileX, tileY, startingIndex, isBarrier, breakEffect, statusGiven); //error here
            room.SettleTiles();
        }
        /// <summary>
        /// Takes player position and writes the room the player is in to the roomdata file
        /// </summary>
        public static void WriteRoomToFile(Player player)
        {
            Room roomToWrite = PosToRoom(player.position);
            StreamWriter writer = File.AppendText("C:/Users/User/source/repos/Monogame Cross Platform/Content/RoomData.txt");
            writer.Write("\n");

            for (var tileY = 0; tileY < 32; tileY++)
            {
                for (var tileX = 0; tileX < 32; tileX++)
                {
                    Tile tile = roomToWrite.tileArray[tileX, tileY];
                    writer.Write($"{16 * (int)(tile.textureIndex / 16)},{tile.isBarrier},{tile.statusGiven},{tile.breakEffect},");
                }
            }
            writer.Close();
        }

    }
}
