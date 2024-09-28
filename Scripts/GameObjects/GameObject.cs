using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects
{
    internal class GameObject
    {
        public ushort textureIndex { get; set; }
        public ushort ObjectIndex { get; set; }
        internal AnimationHandler animationHandler;
        public Vector2 position { get; internal set; }
        public float rotation = 0f;
        public bool isEnabled = false;
        public GameObject(ushort animationIndex, Vector2 startingTile)
        {
            position = TileMap.TileMapPosToPos((int)startingTile.X, (int)startingTile.Y);
            textureIndex = 0;
            animationHandler = new AnimationHandler(animationIndex);
            animationHandler.SetTextureAnimation(0);
        }
        public void UpdateAnimation()
        {
            if (isEnabled)
                (textureIndex, position, rotation) = animationHandler.Update(position, rotation);
        }
        public void SetAnimationData(ushort animationDatatxtIndex)
        {
            animationHandler.SetTextureAnimationData(animationDatatxtIndex);
        }
    } 
}
