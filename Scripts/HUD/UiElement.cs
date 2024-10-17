using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_Cross_Platform.Scripts;
using Monogame_Cross_Platform;
using static System.Net.Mime.MediaTypeNames;
using System.Security.AccessControl;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class UiElement
    {
        public Hitboxes.Hitbox hitBox; //says where entity is centered for drawing origin
        public ushort textureIndex { get; internal set; }
        public bool isEnabled { get; set; } = false; // will be false eventually
        internal float xOffset;
        internal float yOffset;
        private float absxOffset;
        private float absyOffset;
        internal float absHitBoxHeight;
        internal float absHitBoxWidth;
        internal float scale = 1;

        private MovementType movementType;
        private float xOffsetToTravelTo;
        private float yOffsetToTravelTo;
        private float travelDistance;
        private float scaleToTravel;
        private float animSpeed;
        private float animScaleSpeed;
        private float animLength;
        private double timeWhenBeganTravel = 0;
        public UiElement(ushort textureIndex, int xOffset, int yOffset, Rectangle hitBox)
        {
            this.textureIndex = textureIndex;
            absxOffset = xOffset;
            absyOffset = yOffset;
            this.xOffset = xOffset * Settings.uiScaleX;
            this.yOffset = yOffset * Settings.uiScaleY;
            absHitBoxHeight = hitBox.Height;
            absHitBoxWidth = hitBox.Width;
            this.hitBox = new Hitboxes.Hitbox(this.xOffset, this.yOffset, hitBox.Width * Settings.uiScaleX * scale, hitBox.Height * Settings.uiScaleY * scale);
        }
        public virtual bool IsPressed()
        {
            throw new AggregateException(); //this is checking if a non button uielement is pressed
            return false; //This isnt pressable
        }



        public static float InCubic(float t) => (t / 1.9f) * (2/(t + 0.5f));
        /// <summary>
        /// Run after graphics changes
        /// </summary>
        public virtual void Update()
        {
            if ( (xOffsetToTravelTo != absxOffset || yOffsetToTravelTo != yOffset) && !(Game1.gameTime.TotalGameTime.TotalSeconds > timeWhenBeganTravel + animLength) && animSpeed != 0)
            {
                float nextScale = 3;
                float nextX = 0;
                float nextY = 0;

                if (movementType == MovementType.linear)
                {
                    float updatedMoveSpeed = (float)(animSpeed * Game1.gameTime.ElapsedGameTime.TotalSeconds);

                    nextScale = scale + animScaleSpeed * (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * Math.Sign(scaleToTravel - scale); //modify scaling speed here
                    nextX = absxOffset + updatedMoveSpeed * Math.Sign(xOffsetToTravelTo - absxOffset);
                    nextY = absyOffset + updatedMoveSpeed * Math.Sign(yOffsetToTravelTo - absyOffset);

                    

                }
                else if (movementType == MovementType.bounce)
                {
                    float updatedMoveSpeed = (float)(animSpeed * Game1.gameTime.ElapsedGameTime.TotalSeconds);
                    float deltax = xOffsetToTravelTo - absxOffset;
                    float deltay = yOffsetToTravelTo - absyOffset;
                    float deltaTime = (float)Game1.gameTime.ElapsedGameTime.TotalSeconds;

                    float partTravelled = Vector2.Distance(new Vector2(xOffsetToTravelTo, yOffsetToTravelTo), new Vector2(absxOffset, absyOffset)) / travelDistance;
                    if (partTravelled < 0.001f)
                    {
                        absxOffset = xOffsetToTravelTo;
                        absyOffset = yOffsetToTravelTo;
                        scale = scaleToTravel;
                    }

                    nextX = absxOffset + updatedMoveSpeed * Math.Sign(deltax) * InCubic(partTravelled);
                    nextY = absyOffset + updatedMoveSpeed * Math.Sign(deltay) * InCubic(partTravelled);
                    nextScale = scale + animScaleSpeed * (float)Game1.gameTime.ElapsedGameTime.TotalSeconds * Math.Sign(scaleToTravel - scale) * (InCubic(partTravelled)); //modify scaling speed here

                }
                if (!float.IsNaN(nextX) && !float.IsNaN(nextY) && !float.IsNaN(nextScale))
                {
                    if (Math.Sign(xOffsetToTravelTo - absxOffset) != Math.Sign(xOffsetToTravelTo - nextX)) //checks for x overshoot
                        absxOffset = xOffsetToTravelTo;
                    else
                        absxOffset = nextX;

                    if (Math.Sign(yOffsetToTravelTo - absyOffset) != Math.Sign(yOffsetToTravelTo - nextY)) //checks for y overshoot
                        absyOffset = yOffsetToTravelTo;
                    else
                        absyOffset = nextY;

                    if (Math.Sign(scaleToTravel - scale) != Math.Sign(scaleToTravel - nextScale)) //checks for scaling overshoot
                        scale = scaleToTravel;
                    else
                        scale = nextScale;
                }
                else
                {
                    absxOffset = xOffsetToTravelTo;
                    absyOffset = yOffsetToTravelTo;
                    scale = scaleToTravel;
                }
            }


            xOffset = absxOffset * Settings.uiScaleX;
            yOffset = absyOffset * Settings.uiScaleY;
            hitBox = new Hitboxes.Hitbox(xOffset, yOffset, absHitBoxWidth * Settings.uiScaleX, absHitBoxHeight * Settings.uiScaleY); 
        }
        public void MoveTo(int xOffset, int yOffset, float scale, float animLength, MovementType movementType)
        {
            xOffsetToTravelTo = xOffset; //int conversions here might cause wackyness
            yOffsetToTravelTo = yOffset;
            scaleToTravel = scale;
            this.animLength = animLength;
            travelDistance = (xOffsetToTravelTo - absxOffset + yOffsetToTravelTo - absyOffset);//Vector2.Distance(new Vector2(xOffset, yOffset), new Vector2(absxOffset, absyOffset));
            animSpeed = travelDistance / animLength;
            animScaleSpeed = Math.Abs(scaleToTravel - this.scale) / animLength;
            this.movementType = movementType;
            
            timeWhenBeganTravel = Game1.gameTime.TotalGameTime.TotalSeconds;
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
