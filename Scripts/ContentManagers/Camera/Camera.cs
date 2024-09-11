using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.ContentManagers.Camera
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        float xToMove = 0;
        float yToMove = 0;
        bool isFirstPass = true;
        public void Scale(float scale)
        {
            Transform = Matrix.CreateScale(scale);
        }
        public void Follow(GameObjects.Entities.Entity target)
        {
            if (isFirstPass)
            {
                xToMove = -target.position.X;
                yToMove = -target.position.Y;
                isFirstPass = false;
            }
            xToMove += (-target.position.X - xToMove)/5f;
            yToMove += (-target.position.Y - yToMove)/5f;
            
            var position = Matrix.CreateTranslation(xToMove, yToMove, 0);
            var offset = Matrix.CreateTranslation(
                (Settings.resolutionWidth / 2 / Settings.zoomLevel),
                (Settings.resolutionHeight / 2 / Settings.zoomLevel),
                0);
            var zoom = Matrix.CreateScale(Settings.zoomLevel);

            Transform = position * offset * (zoom);
        }
        public void Follow(GameObjects.Entities.Entity target, GameObjects.Entities.Entity target2)
        {
            throw new NotImplementedException();
            //This should place the camera in between two entities for bossfights, etc
            xToMove = -target.position.X;
            yToMove = -target.position.Y;
            var position = Matrix.CreateTranslation(xToMove / 1.5f, yToMove / 1.5f, 0);
            var offset = Matrix.CreateTranslation(
                (Settings.resolutionWidth / 2 / Settings.zoomLevel),
                (Settings.resolutionHeight / 2 / Settings.zoomLevel),
                0);
            var zoom = Matrix.CreateScale(Settings.zoomLevel);
            xToMove -= xToMove / 1.5f;
            yToMove -= yToMove / 1.5f;

            Transform = position * offset * (zoom);
        }
    }
}

