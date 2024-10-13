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
        public ushort decorationIndex;
        public byte rotation;

        public Tile(ushort textureIndex, bool isBarrier, bool breakable, byte decorationIndex)
        {
            this.textureIndex = textureIndex;
            this.isBarrier = isBarrier;
            this.breakable = breakable;
            isBeingPathfoundTo = false;
            this.decorationIndex = decorationIndex;
            rotation = 0;
        }
        public Tile(ushort textureIndex, bool isBarrier, bool breakable, byte decorationIndex, byte rotation)
        {
            this.textureIndex = textureIndex;
            this.isBarrier = isBarrier;
            this.breakable = breakable;
            isBeingPathfoundTo = false;
            this.decorationIndex = decorationIndex;
            this.rotation = rotation;
        }
        public Tile()
        {
            textureIndex = 0;
            isBarrier = false;
            breakable = false;
            isBeingPathfoundTo = false;
            decorationIndex = 0;
            rotation = 0;
        }
    }
}
