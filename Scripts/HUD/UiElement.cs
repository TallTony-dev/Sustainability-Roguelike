using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_Cross_Platform.Scripts;
using Monogame_Cross_Platform;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class UiElement
    {
        internal Vector2 position;
        public Rectangle hitBox; //says where entity is centered for drawing origin
        public ushort textureIndex { get; private set; }
        public bool isEnabled { get; set; } = true; // will be false eventually
        internal int xOffset;
        internal int yOffset;


        public UiElement(ushort textureIndex, int xOffset, int yOffset, Rectangle hitBox)
        {
            this.textureIndex = textureIndex;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.hitBox = new Rectangle(hitBox.X, hitBox.Y, hitBox.Width * 2, hitBox.Height * 2);
        }
        public virtual void Update()
        {

        }
        /// <summary>
        /// Run after graphics changes
        /// </summary>
        public void UpdateHitbox()
        {
            position.X = xOffset;
            position.Y = yOffset;
            hitBox = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), hitBox.Width, hitBox.Height); //This rounding may cause issues but yeah shouldnt be too bad
        }
        public void Enable()
        {
            isEnabled = true; 
        }
    }
}
