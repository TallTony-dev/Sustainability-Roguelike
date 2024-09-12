using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.Hitboxes
{
    internal struct Hitbox
    {
        //Y and x position are the top corner of the hitbox
        public float xPos { get; set; }
        public float yPos { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float left => xPos;
        public float right => xPos + width;
        public float top => yPos;
        public float bottom => yPos + height;
        public Hitbox(float xPos, float yPos, float width, float height)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.width = width;
            this.height = height;
        }
        public void UpdatePosition(float x, float y)
        {
            xPos = x - width/2;
            yPos = y - width/2;
        }
        public bool Intersects(Rectangle rectangle)
        {
            if (rectangle.Left < right && rectangle.Right > left && rectangle.Top < bottom && rectangle.Bottom > top)
            {
                return true;
            }
            else return false;
        }
    }
}
