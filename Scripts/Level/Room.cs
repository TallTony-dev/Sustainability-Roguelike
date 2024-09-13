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
        public Tile[,] tileArray;
        public int sqrtTileArrayLength { get; private set; }
        public void SetTile(int x, int y, ushort startingTileTextureIndex, bool isBarrier, byte breakEffect, byte statusGiven)
        {
            tileArray[x,y].textureIndex = startingTileTextureIndex;
            tileArray[x,y].statusGiven = statusGiven;
            tileArray[x,y].isBarrier = isBarrier;
            tileArray[x,y].breakEffect = breakEffect;
        }
        public Room(ushort roomIndex)
        {
            tileArray = new Tile[32, 32];
            sqrtTileArrayLength = (int)Math.Sqrt(tileArray.Length);
            string roomData = File.ReadLines("Content/RoomData.txt").Skip(roomIndex * 2).Take(1).First(); //this skips the number indicated in roomindex * 2 so that comments can be added between lines in roomdata.txt
            string[] tokens = roomData.Split(",");
            for (int y = 0; y < sqrtTileArrayLength; y++)
            {
                for (int x = 0; x < sqrtTileArrayLength; x++)
                {
                    tileArray[x, y].textureIndex = Convert.ToUInt16(tokens[(x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].isBarrier = Convert.ToBoolean(tokens[1 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].statusGiven = Convert.ToByte(tokens[2 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].breakEffect = Convert.ToByte(tokens[3 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
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
                    Tile tile = tileArray[x, y];
                    ushort tileIndex = (ushort)(tile.textureIndex - (tile.textureIndex % 16));

                    if (x + 1 < sqrtTileArrayLength && tileArray[x + 1, y].textureIndex >= tileIndex && tileArray[x + 1, y].textureIndex < tileIndex + 16) //checking right
                    {
                        enumToPick += 1;
                    }
                    if (x - 1 >= 0 && tileArray[x - 1, y].textureIndex >= tileIndex && tileArray[x - 1, y].textureIndex < tileIndex + 16) //checking left
                    {
                        enumToPick += 2;
                    }
                    if (y + 1 < sqrtTileArrayLength && tileArray[x, y + 1].textureIndex >= tileIndex && tileArray[x, y + 1].textureIndex < tileIndex + 16)//checking bottom
                    {
                        enumToPick += 4;
                    }
                    if (y - 1 >= 0 && tileArray[x, y - 1].textureIndex >= tileIndex && tileArray[x, y - 1].textureIndex < tileIndex + 16)//checking top
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

                    tileArray[x, y].textureIndex = tileIndex;
                }
            }
        }
        enum adjTiles { none, right, left, rightleft, bottom, bottomright, bottomleft, bottomrightleft, top, topright, topleft, toprightleft, topbottom, topbottomright, topbottomleft, topbottomrightleft }
    }
}
