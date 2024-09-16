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
        private float absxOffset;
        private float absyOffset;
        internal float scale = 1;

        private MovementType movementType;
        private float xOffsetToTravel;
        private float yOffsetToTravel;
        private float scaleToTravel;
        private double timeWhenStartedAnim = 0;
        private int animLengthMS;
        double positionChange;
        private bool isMoving = false;
        public UiElement(ushort textureIndex, int xOffset, int yOffset, Rectangle hitBox)
        {
            this.textureIndex = textureIndex;
            absxOffset = xOffset;
            absyOffset = yOffset;
            this.xOffset = xOffset * Settings.uiScaleX;
            this.yOffset = yOffset * Settings.uiScaleY;
            this.hitBox = new Hitboxes.Hitbox(this.xOffset, this.yOffset, hitBox.Width * Settings.uiScaleX * scale, hitBox.Height * Settings.uiScaleY * scale);
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
            double timeSinceAnimStart = Game1.gameTime.TotalGameTime.TotalMilliseconds - timeWhenStartedAnim;
            if (isMoving)
            {
                if (animLengthMS < timeSinceAnimStart)
                {
                    isMoving = false;
                }
                else
                {
                    if (movementType == MovementType.linear)
                    {
                        positionChange = timeSinceAnimStart / animLengthMS;
                        scale = (float)(scaleToTravel * positionChange);
                        absxOffset += (float)(xOffsetToTravel * positionChange);
                        absyOffset += (float)(yOffsetToTravel * positionChange);
                        xOffsetToTravel -= (float)(xOffsetToTravel * positionChange);
                        yOffsetToTravel -= (float)(yOffsetToTravel * positionChange);
                    }
                    if (movementType == MovementType.bounce)
                    {

                    }
                }
            }
            xOffset = absxOffset * Settings.uiScaleX;
            yOffset = absyOffset * Settings.uiScaleY;
            hitBox = new Hitboxes.Hitbox(xOffset, yOffset, hitBox.width * Settings.uiScaleX, hitBox.height * Settings.uiScaleY); 
        }
        public void MoveTo(int xOffset, int yOffset, float scale, int milliseconds, MovementType movementType)
        {
            if (!isMoving)
            {
                timeWhenStartedAnim = Game1.gameTime.TotalGameTime.TotalMilliseconds;
                xOffsetToTravel = (int)(xOffset - absxOffset); //int conversions here might cause wackyness
                yOffsetToTravel = (int)(yOffset - absyOffset);
                scaleToTravel = scale - this.scale;
                isMoving = true;
                animLengthMS = milliseconds;
                this.movementType = movementType;
            }
        }
        public void Enable()
        {
            isEnabled = true; 
        }
        public void Disable()
        {
            isEnabled = false;
        }

        public enum MovementType { linear, bounce }
    }
}
