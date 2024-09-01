using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.ContentManagers.Camera
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        public void Follow(GameObjects.Entities.Entity target)
        {
            var position = Matrix.CreateTranslation(
              -target.position.X,
              -target.position.Y,
              0);
            var offset = Matrix.CreateTranslation(
                (Settings.resolutionWidth / 2 / Settings.zoomLevel),
                (Settings.resolutionHeight / 2 / Settings.zoomLevel), //set to the resolution of the rendertarget here and change the offset to put character where you want
                0);
            var zoom = Matrix.CreateScale(Settings.zoomLevel);

            Transform = position * offset * (zoom);
        }
    }
}

