using Monogame_Cross_Platform.Scripts.Hitboxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Objects
{
    /// <summary>
    /// Not yet implemented, just acts as a gameobject
    /// </summary>
    internal class CollidableObject : GameObject
    {
        public bool isBarrier = true;
        //public Hitbox hitBox;
        public CollidableObject(ushort textureIndex, Vector2 startingTile, Vector2 hitboxSize) : base(textureIndex, startingTile)
        {
            Vector2 pos = TileMap.TileMapPosToPos((int)startingTile.X, (int)startingTile.Y);
            //this.hitBox = new Hitbox(pos.X - hitboxSize.X/2, pos.Y - hitboxSize.Y/2, hitboxSize.X, hitboxSize.Y);
        }
    }
}
