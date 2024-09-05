using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.GameObjects
{
    internal class HitboxManager
    {
        public Rectangle hitBox { get; internal set; }
        public HitboxManager(Rectangle hitBox)
        {
            this.hitBox = hitBox;
        }
        public void UpdateHitbox(Vector2 position)
        {
            hitBox = new Rectangle((int)Math.Round(position.X) - 16, (int)Math.Round(position.Y) - 16, hitBox.Width, hitBox.Height);
        }
    }
}
