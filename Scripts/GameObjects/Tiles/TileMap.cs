using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Tiles
{
    internal static class TileMap
    {
        public static Tile[,] tileMap = new Tile[512,512];
        public static int sqrtTileMapLength = (int)Math.Sqrt(tileMap.Length);

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

        public static void ResetTileMap(Tile tile)
        {
            Random random = new Random();
            for (int y = 0; y < 512; y++)
            {
                for (int x = 0; x < 512; x++)
                {
                    tileMap[x, y] = tile;
                    byte rand = (byte)random.Next(0, 4);
                    tileMap[x, y].rotation = rand;
                }
            }
        }
        public static void SettleTileMap()
        {
            int sqrtTileMapLength = (int)Math.Sqrt(tileMap.Length);
            for (var y = 0; y < 512; y++)
            {
                for (var x = 0; x < 512; x++)
                {
                    SettleTile(x, y);
                }
            }
        }
        public static void SettleTile(int x, int y)
        {
            short enumToPick = 0;
            Tile tile = tileMap[x, y];
            ushort tileIndex = (ushort)(tile.textureIndex - (tile.textureIndex % 30));
            bool isBottom = false;
            bool isTop = false;
            bool isLeft = false;
            bool isRight = false;
            bool isBottomRight = false;
            bool isTopRight = false;
            bool isBottomLeft = false;
            bool isTopLeft = false;


            if (x + 1 < sqrtTileMapLength && tileMap[x + 1, y].textureIndex >= tileIndex && tileMap[x + 1, y].textureIndex < tileIndex + 30) //checking right
            {
                enumToPick += 1;
                isRight = true;
                if (y + 1 < sqrtTileMapLength && tileMap[x + 1, y + 1].textureIndex >= tileIndex && tileMap[x + 1, y + 1].textureIndex < tileIndex + 30)//checking bottom
                {
                    isBottomRight = true;
                }
                if (y - 1 >= 0 && tileMap[x + 1, y - 1].textureIndex >= tileIndex && tileMap[x + 1, y - 1].textureIndex < tileIndex + 30)//checking top
                {
                    isTopRight = true;
                }
            }
            if (x - 1 >= 0 && tileMap[x - 1, y].textureIndex >= tileIndex && tileMap[x - 1, y].textureIndex < tileIndex + 30) //checking left
            {
                enumToPick += 2;
                isLeft = true;
                if (y + 1 < sqrtTileMapLength && tileMap[x - 1, y + 1].textureIndex >= tileIndex && tileMap[x - 1, y + 1].textureIndex < tileIndex + 30)//checking bottom
                {
                    isBottomLeft = true;
                }
                if (y - 1 >= 0 && tileMap[x - 1, y - 1].textureIndex >= tileIndex && tileMap[x - 1, y - 1].textureIndex < tileIndex + 30)//checking top
                {
                    isTopLeft = true;
                }
            }
            if (y + 1 < sqrtTileMapLength && tileMap[x, y + 1].textureIndex >= tileIndex && tileMap[x, y + 1].textureIndex < tileIndex + 30)//checking bottom
            {
                enumToPick += 4;
                isBottom = true;
            }
            if (y - 1 >= 0 && tileMap[x, y - 1].textureIndex >= tileIndex && tileMap[x, y - 1].textureIndex < tileIndex + 30)//checking top
            {
                enumToPick += 8;
                isTop = true;
            }


            if (!isBottomRight && enumToPick == 15)
            {
                enumToPick = 16;
            }
            else if (!isBottomLeft && enumToPick == 15)
            {
                enumToPick = 17;
            }
            else if (!isTopRight && enumToPick == 15)
            {
                enumToPick = 18;
            }
            else if (!isTopLeft && enumToPick == 15)
            {
                enumToPick = 19;
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


                adjTiles1.bottomrightcorner => 16,
                adjTiles1.bottomleftcorner => 17,
                adjTiles1.toprightcorner => 18,
                adjTiles1.topleftcorner => 19,
                _ => 0
            };

            tileMap[x, y].textureIndex = tileIndex;
        }
        public static void SettleTileMap(int startingTileX, int startingTileY, int endingTileX, int endingTileY)
        {
            for (var y = startingTileY; y < endingTileY; y++)
            {
                for (var x = startingTileX; x < endingTileX; x++)
                {
                    if (x > -1 && y > -1 && x < sqrtTileMapLength && y < sqrtTileMapLength)
                    {
                        SettleTile(x, y);
                    }
                }
            }
        }
        /// <summary>
        /// Checks if the entity hitbox collides with a tile hitbox or an entity at its abs position, returns true if it collides
        /// </summary>
        public static bool IsCollisionAbs(int tileX, int tileY)
        {
            if (tileX < 0 || tileY < 0 || tileX > 511 || tileY > 511)
            {
                return true;
            }
            if (tileMap[tileX, tileY].isBarrier || tileMap[tileX, tileY].isBeingPathfoundTo)
            {
                return true;
            }
            foreach (GameObject gameObject in Game1.currentGameObjects)
            {
                if (gameObject is Entity)
                {
                    Entity entity = (Entity)gameObject;
                    (int entityX, int entityY) = PosToAbsTileMapPos(entity.position);
                    if (entity.isEnabled && (entityX, entityY) == (tileX, tileY))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void SetPathfindingModeOfTileAtCoord(float x, float y, bool mode)
        {
            (int tileX, int tileY) = PosToAbsTileMapPos(new Vector2(x, y));
            tileMap[tileX, tileY].isBeingPathfoundTo = mode;
        }
        public static void SetPathfindingModeOfTile(int x, int y, bool mode)
        {
            tileMap[x, y].isBeingPathfoundTo = mode;
        }


        public static bool DoesRaycastCollide(float targetPosTileX, float targetPosTileY, float sourcePosTileX, float sourcePosTileY, float width, float height)
        {
            int bottomX = (int)Math.Round(Math.Min(targetPosTileX, sourcePosTileX));
            int topX = (int)Math.Round(Math.Max(targetPosTileX, sourcePosTileX));
            int bottomY = (int)Math.Round(Math.Min(targetPosTileY, sourcePosTileY));
            int topY = (int)Math.Round(Math.Max(targetPosTileY, sourcePosTileY));
            float deltaY = Math.Abs(targetPosTileY - sourcePosTileY);
            float deltaX = Math.Abs(targetPosTileX - sourcePosTileX);


            float slope = (targetPosTileY * 32 - sourcePosTileY * 32) / (targetPosTileX * 32 - sourcePosTileX * 32);
            float b = (targetPosTileY * 32 - slope * targetPosTileX * 32);

            for (int x = bottomX - 1; x < topX + 1; x++)
            {
                for (int y = bottomY - 1; y < topY + 1; y++)
                {
                    if (tileMap[x, y].isBarrier)
                    {
                        (_, Hitboxes.Hitbox hitbox) = GetTileBounds(x, y);

                        //if its more horizontal than vertical, then check every x and calculate y, otherwise invert that
                        (float tileCoordX, float tileCoordY) = TileMapPosToPos(x, y);

                        if (deltaX > deltaY)
                        {
                            if (hitbox.Intersects(new Hitboxes.Hitbox(tileCoordX, slope * tileCoordX + b, width, height)))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (hitbox.Intersects(new Hitboxes.Hitbox((tileCoordY - b) / slope, tileCoordY, width, height)))
                            {
                                return true;
                            }
                        }
                    }
                }
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
        /// <summary>
        /// Returns no matter if the tile is barrier or not
        /// </summary>
        public static Hitboxes.Hitbox ForceGetTileBounds(int tileX, int tileY)
        {
            return new Hitboxes.Hitbox(tileX * 32 - 16, tileY * 32 - 16, 32, 32);
        }

        public enum adjTiles1 { none, right, left, rightleft, bottom, bottomright, bottomleft, bottomrightleft, top, topright, topleft, toprightleft, topbottom, topbottomright, topbottomleft, topbottomrightleft, bottomrightcorner, bottomleftcorner, toprightcorner, topleftcorner }
    }
}
