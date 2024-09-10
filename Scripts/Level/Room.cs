using System.IO;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;

namespace Monogame_Cross_Platform.Scripts.Level
{
    internal class Room
    {
        public Tile[,] room;
        int sqrtRoomLength;
        public void SetTile(int x, int y, ushort startingTileTextureIndex, bool isBarrier, byte breakEffect, byte statusGiven)
        {
            room[x,y].textureIndex = startingTileTextureIndex;
            room[x,y].statusGiven = statusGiven;
            room[x,y].isBarrier = isBarrier;
            room[x,y].breakEffect = breakEffect;
        }
        public Room(ushort roomIndex)
        {
            room = new Tile[32, 32];
            sqrtRoomLength = (int)Math.Sqrt(room.Length);
            string roomData = File.ReadLines("Content/RoomData.txt").Skip(roomIndex).Take(1).First();
            string[] tokens = roomData.Split(",");
            for (int y = 0; y < sqrtRoomLength; y++)
            {
                for (int x = 0; x < sqrtRoomLength; x++)
                {
                    room[x, y].textureIndex = Convert.ToUInt16(tokens[(y * 4) + (x * sqrtRoomLength)]);
                    room[x, y].isBarrier = Convert.ToBoolean(tokens[1 +(y * 4) + (x * sqrtRoomLength)]);
                    room[x, y].statusGiven = Convert.ToByte(tokens[2 + (y * 4) + (x * sqrtRoomLength)]);
                    room[x, y].breakEffect = Convert.ToByte(tokens[3 + (y * 4) + (x * sqrtRoomLength)]);
                }
            }
        }
        public void SettleTiles()
        {
            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    short enumToPick = 0;
                    Tile tile = room[x, y];
                    ushort tileIndex = tile.textureIndex;

                    if (x + 1 < sqrtRoomLength && room[x + 1, y].textureIndex >= tile.textureIndex && room[x + 1, y].textureIndex < tile.textureIndex + 16) //checking right
                    {
                        enumToPick += 1;
                    }
                    if (x - 1 >= 0 && room[x - 1, y].textureIndex >= tile.textureIndex && room[x - 1, y].textureIndex < tile.textureIndex + 16) //checking left
                    {
                        enumToPick += 2;
                    }
                    if (y + 1 < sqrtRoomLength && room[x, y + 1].textureIndex >= tile.textureIndex && room[x, y + 1].textureIndex < tile.textureIndex + 16)//checking bottom
                    {
                        enumToPick += 4;
                    }
                    if (y - 1 >= 0 && room[x, y - 1].textureIndex >= tile.textureIndex && room[x, y - 1].textureIndex < tile.textureIndex + 16)//checking top
                    {
                        enumToPick += 8;
                    }

                    tileIndex += ((adjTiles)enumToPick) switch
                    {
                        adjTiles.none => 0,
                        adjTiles.right => 1,
                        adjTiles.rightleft => 2,
                        adjTiles.left => 3,
                        adjTiles.bottom => 4,
                        adjTiles.bottomright => 5,
                        adjTiles.bottomrightleft => 6,
                        adjTiles.bottomleft => 7,
                        adjTiles.topbottom => 8,
                        adjTiles.topbottomright => 9,
                        adjTiles.topbottomrightleft => 10,
                        adjTiles.topbottomleft => 11,
                        adjTiles.top => 12,
                        adjTiles.topright => 13,
                        adjTiles.toprightleft => 14,
                        adjTiles.topleft => 15,
                        _ => 0
                    };

                    room[x, y].textureIndex = tileIndex;
                }
            }
        }
        enum adjTiles { none, right, left, rightleft, bottom, bottomright, bottomleft, bottomrightleft, top, topright, topleft, toprightleft, topbottom, topbottomright, topbottomleft, topbottomrightleft }
    }
}
