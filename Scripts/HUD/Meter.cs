using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class Meter : UiElement
    {
        public Meter(ushort frontTextureIndex, ushort backTextureIndex, int xOffset, int yOffset, Rectangle meterSize, bool isVertical, int maxVal, int minVal) : base(backTextureIndex, xOffset, yOffset, meterSize, 0f)
        {
            this.frontTextureIndex = frontTextureIndex;
            this.isVertical = isVertical;
            this.maxVal = maxVal;
            this.minVal = minVal;
            xMeterOffset = meterSize.X;
            yMeterOffset = meterSize.Y;
            value = maxVal;

            Update(maxVal);
        }
        public int xMeterOffset;
        public int yMeterOffset;

        public ushort frontTextureIndex;
        public int minVal;
        public int maxVal;
        public int value;
        public bool isVertical;
        public float xScale = 1; //use these instead of normal scale from gameobject
        public float yScale = 1;
        public Rectangle drawingMask { get; private set; }
        public void Update(int newValue)
        {
            value = newValue;
            float proportion = (float)value / (maxVal - minVal);
            if (!isVertical)
                drawingMask = new Rectangle(xMeterOffset, yMeterOffset, (int)Math.Round(absHitBoxWidth * proportion), (int)absHitBoxHeight);
            else
                drawingMask = new Rectangle(xMeterOffset, yMeterOffset, (int)absHitBoxWidth, (int)Math.Round(absHitBoxHeight * proportion));
            Update();
        }



    }
}
