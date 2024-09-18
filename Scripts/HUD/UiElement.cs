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
        private int animSpeed;
        private float travelDistance;
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
            if ( (xOffsetToTravel != absxOffset || yOffsetToTravel != yOffset) && animSpeed != 0)
            {
                if (movementType == MovementType.linear)
                {
                    float updatedMoveSpeed = (float)(animSpeed * Game1.gameTime.ElapsedGameTime.TotalSeconds);

                    float nextScale = scale + updatedMoveSpeed * Math.Sign(scaleToTravel - scale) / (animSpeed * (travelDistance + 1) / 100); //modify scaling speed here
                    float nextX = absxOffset + updatedMoveSpeed * Math.Sign(xOffsetToTravel - absxOffset);
                    float nextY = absyOffset + updatedMoveSpeed * Math.Sign(yOffsetToTravel - absyOffset);

                    if (Math.Sign(xOffsetToTravel - absxOffset) != Math.Sign(xOffsetToTravel - nextX)) //checks for x overshoot
                        absxOffset = xOffsetToTravel;
                    else
                        absxOffset = nextX;

                    if (Math.Sign(yOffsetToTravel - absyOffset) != Math.Sign(yOffsetToTravel - nextY)) //checks for y overshoot
                        absyOffset = yOffsetToTravel;
                    else
                        absyOffset = nextY;

                    if (Math.Sign(scaleToTravel - scale) != Math.Sign(scaleToTravel - nextScale)) //checks for scaling overshoot
                        scale = scaleToTravel;
                    else
                        scale = nextScale;

                }

                if (movementType == MovementType.bounce)
                {

                }
            }





            xOffset = absxOffset * Settings.uiScaleX;
            yOffset = absyOffset * Settings.uiScaleY;
            hitBox = new Hitboxes.Hitbox(xOffset, yOffset, hitBox.width * Settings.uiScaleX, hitBox.height * Settings.uiScaleY); 
        }
        public void MoveTo(int xOffset, int yOffset, float scale, int speed, MovementType movementType)
        {
            xOffsetToTravel = xOffset; //int conversions here might cause wackyness
            yOffsetToTravel = yOffset;
            scaleToTravel = scale;
            animSpeed = speed;
            this.movementType = movementType;
            travelDistance = Math.Abs(yOffsetToTravel - absyOffset) + Math.Abs(xOffsetToTravel - absxOffset);
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
