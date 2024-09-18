using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Tiles
{
    internal static class TileMap
    {
        public static Tile[,] tileMap = new Tile[512,512];
        public static Tile backgroundTile { get; private set; } = new Tile(0, false, 0);

        /// <summary>
        /// Takes a posiiton of an entity and converts it to a position on the tilemap, centers the position assuming 32x32 texture
        /// </summary>
        public static Vector2 PosToTileMapPos(Vector2 position)
        {
            return new Vector2(position.X / 32, position.Y / 32);
        }
        public static (int,int) PosToAbsTileMapPos(Vector2 position)
        {
            return ((int)Math.Round(position.X/32, 0, MidpointRounding.AwayFromZero), (int)Math.Round(position.Y/32, 0, MidpointRounding.AwayFromZero));
        }
        public static Vector2 TileMapPosToPos(int x, int y)
        {
            return new Vector2(x * 32, y * 32);
        }

        public static void SetBackground(Tile tile)
        {
            for (int y = 0; y < 512; y++)
            {
                for (int x = 0; x < 512; x++)
                {
                    if (tileMap[x,y].Equals(backgroundTile)) //may be issue with this, espcially if the old tile is also used in a room somewhere
                        tileMap[x, y] = tile;
                }
            }
            backgroundTile = tile;
        }
        public static void SettleTileMap()
        {
            for (var y = 0; y < 512; y++)
            {
                for (var x = 0; x < 512; x++)
                {
                    short enumToPick = 0;
                    Tile tile = tileMap[x, y];
                    ushort tileIndex = (ushort)(tile.textureIndex - (tile.textureIndex % 16));
                    int sqrtTileMapLength = (int)Math.Sqrt(tileMap.Length);

                    if (x + 1 < sqrtTileMapLength && tileMap[x + 1, y].textureIndex >= tileIndex && tileMap[x + 1, y].textureIndex < tileIndex + 16) //checking right
                    {
                        enumToPick += 1;
                    }
                    if (x - 1 >= 0 && tileMap[x - 1, y].textureIndex >= tileIndex && tileMap[x - 1, y].textureIndex < tileIndex + 16) //checking left
                    {
                        enumToPick += 2;
                    }
                    if (y + 1 < sqrtTileMapLength && tileMap[x, y + 1].textureIndex >= tileIndex && tileMap[x, y + 1].textureIndex < tileIndex + 16)//checking bottom
                    {
                        enumToPick += 4;
                    }
                    if (y - 1 >= 0 && tileMap[x, y - 1].textureIndex >= tileIndex && tileMap[x, y - 1].textureIndex < tileIndex + 16)//checking top
                    {
                        enumToPick += 8;
                    }

                    tileIndex += ((adjTiles1)enumToPick) switch
                    {
                        adjTiles1.none => 0,
                        adjTiles1.right => 1,
                        adjTiles1.rightleft => 2,
                        adjTiles1.left => 3,
                        adjTiles1.bottom => 4,
                        adjTiles1.bottomright => 5,
                        adjTiles1.bottomrightleft => 6,
                        adjTiles1.bottomleft => 7,
                        adjTiles1.topbottom => 8,
                        adjTiles1.topbottomright => 9,
                        adjTiles1.topbottomrightleft => 10,
                        adjTiles1.topbottomleft => 11,
                        adjTiles1.top => 12,
                        adjTiles1.topright => 13,
                        adjTiles1.toprightleft => 14,
                        adjTiles1.topleft => 15,
                        _ => 0
                    };

                    tileMap[x, y].textureIndex = tileIndex;
                }
            }
        }
        /// <summary>
        /// Checks if the entity hitbox collides with a tile hitbox, returns true if it collides
        /// </summary>
        public static bool IsCollisionAbs(int tileX, int tileY)
        {
            if (tileX < 0 || tileY < 0 || tileX > 511 || tileY > 511)
            {
                return true;
            }
            if (tileMap[tileX, tileY].isBarrier)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// checks for if the tile has a barrier
        /// </summary>
        public static (bool, Hitboxes.Hitbox) GetTileBounds(int tileX, int tileY)
        {
            if (tileMap[tileX, tileY].isBarrier)
                return (true, new Hitboxes.Hitbox(tileX * 32 - 16, tileY * 32 - 16, 32, 32));
            else
                return (false, new Hitboxes.Hitbox(0, 0, 0, 0));
        }

        public enum adjTiles1 { none, right, left, rightleft, bottom, bottomright, bottomleft, bottomrightleft, top, topright, topleft, toprightleft, topbottom, topbottomright, topbottomleft, topbottomrightleft }
    }
}
