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
        public byte statusGiven;
        public byte breakEffect;

        public Tile(ushort textureIndex, bool isBarrier, byte statusGiven, byte breakEffect)
        {
            this.textureIndex = textureIndex;
            this.isBarrier = isBarrier;
            this.statusGiven = statusGiven;
            this.breakEffect = breakEffect;
        }
        public Tile()
        {
            textureIndex = 0;
            isBarrier = false;
            statusGiven = 0;
            breakEffect = 0;
        }
    }
}
