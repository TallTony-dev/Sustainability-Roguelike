using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Tiles
{
    internal static class TileMap
    {
        public static Tile[,] tileMap = new Tile[4096,4096];

        /// <summary>
        /// Takes a standard coordinate and checks if it is within a tile marked with passable or whatever you end up calling it, true if it is a valid spot
        /// </summary>
        //public static bool IsInValidTile(Rectangle entityBounds, Vector2 entityPosition)
        //{
        //   if (PosToTileMapPos(entityPosition))

        //}
        //public static bool IsInValidTile(int tileX, int tileY)
        //{

        //}

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
        /// <summary>
        /// Does not check for if the tile has a barrier, only returns rectangle to check intersection
        /// </summary>
        public static Rectangle GetTileBounds(int tileX, int tileY)
        {
            return new Rectangle(tileX * 32, tileY * 32, 32, 32);
        }

    }
}
