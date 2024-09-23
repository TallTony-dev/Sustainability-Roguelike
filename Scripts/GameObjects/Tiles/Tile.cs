using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Tiles
{
    public struct Tile
    {
        public ushort textureIndex;
        public bool isBarrier;
        public bool isBeingPathfoundTo;
        public bool breakable;

        public Tile(ushort textureIndex, bool isBarrier, bool breakable)
        {
            this.textureIndex = textureIndex;
            this.isBarrier = isBarrier;
            this.breakable = breakable;
            isBeingPathfoundTo = false;
        }
        public Tile()
        {
            textureIndex = 0;
            isBarrier = false;
            breakable = false;
            isBeingPathfoundTo = false;
        }
    }
}
