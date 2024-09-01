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
        /// Takes a posiiton of an entity and converts it to a position on the tilemap, centers the position assuming 32x32 texture
        /// </summary>
        public static Vector2 PosToTileMapPos(Vector2 position)
        {
            return new Vector2(((position.X) / 32), ((position.Y) / 32));
        }
    }
}
