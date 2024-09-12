using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Tiles
{
    internal static class TileMap
    {
        public static Tile[,] tileMap = new Tile[512,512];

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
        /// Checks if the entity hitbox collides with a tile hitbox, returns true if it collides
        /// </summary>
        public static bool IsCollision(Entity entity, int tileX, int tileY)
        {
            if (tileX < 0 || tileY < 0 || tileX > 511 || tileY > 511)
            {
                return true;
            }
            if (tileMap[tileX, tileY].isBarrier && entity.hitBox.Intersects(GetTileBounds(tileX, tileY)))
            {
                return true;
            }
            return false;
        }
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
        /// Does not check for if the tile has a barrier, only returns rectangle boundary
        /// </summary>
        public static Rectangle GetTileBounds(int tileX, int tileY)
        {
            return new Rectangle(tileX * 32 - 16, tileY * 32 - 16, 32, 32);
        }

    }
}
