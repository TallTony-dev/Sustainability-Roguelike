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
        public Hitboxes.Hitbox hitBox; //says where entity is centered for drawing origin
        public ushort textureIndex { get; private set; }
        public bool isEnabled { get; set; } = false; // will be false eventually
        internal float xOffset;
        internal float yOffset;


        public UiElement(ushort textureIndex, int xOffset, int yOffset, Rectangle hitBox)
        {
            this.textureIndex = textureIndex;
            this.xOffset = xOffset * Settings.uiScaleX;
            this.yOffset = yOffset * Settings.uiScaleY;
            this.hitBox = new Hitboxes.Hitbox(this.xOffset, this.yOffset, hitBox.Width * Settings.uiScaleX, hitBox.Height * Settings.uiScaleY);
        }
        public virtual bool IsPressed()
        {
            throw new AggregateException(); //this is checking if a non button uielement is pressed
            return false; //This isnt pressable
        }
        /// <summary>
        /// Run after graphics changes
        /// </summary>
        public void Update()
        {
            hitBox = new Hitboxes.Hitbox(xOffset * Settings.uiScaleX, yOffset * Settings.uiScaleY, hitBox.width * Settings.uiScaleX, hitBox.height * Settings.uiScaleY); 
        }
        public void Enable()
        {
            isEnabled = true; 
        }

        public enum ClampingType { top, bottom, right, left, topright, topleft, bottomright, bottomleft }
    }
}
