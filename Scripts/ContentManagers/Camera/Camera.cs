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
        float rotationToMove = 0;
        bool isFirstPass = true;
        public List<(float deltaX, float deltaY, float deltaRotation, float timeRemaining)> cameraAnimationsToPlay = new List<(float deltaX, float deltaY, float deltaRotation, float timeRemaining)>();

        private double timeWhenCameraLocked = 0;
        public static bool IsLocked = true;
        public void Update()
        {
            if (cameraAnimationsToPlay.Count > 0)
            {
                for (var x = cameraAnimationsToPlay.Count - 1; x >= 0; x--)
                {
                    var animation = cameraAnimationsToPlay[x];
                    xToMove += animation.deltaX * (1 / (float)(animation.timeRemaining - Game1.gameTime.ElapsedGameTime.TotalSeconds + 0.5)) / 10f;
                    yToMove += animation.deltaY * (1 / (float)(animation.timeRemaining - Game1.gameTime.ElapsedGameTime.TotalSeconds + 0.5)) / 10f;
                    rotationToMove += animation.deltaRotation * (1 / (float)(animation.timeRemaining - Game1.gameTime.ElapsedGameTime.TotalSeconds + 0.5)) / 10f;
                    cameraAnimationsToPlay[x] = (animation.deltaX, animation.deltaY, animation.deltaRotation, animation.timeRemaining - (float)Game1.gameTime.ElapsedGameTime.TotalSeconds);
                    if (animation.timeRemaining < 0)
                        cameraAnimationsToPlay.RemoveRange(x, 1);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q) && !TurnManager.isPaused && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenCameraLocked > 0.5 && !IsLocked)
            {
                IsLocked = true;
                timeWhenCameraLocked = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && !TurnManager.isPaused && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenCameraLocked > 0.5 && IsLocked)
            {
                IsLocked = false;
                timeWhenCameraLocked = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
        }
        public void Follow(Entity target)
        {
            if (isFirstPass)
            {
                xToMove = -target.position.X;
                yToMove = -target.position.Y;
                isFirstPass = false;
            }
            xToMove += (-target.position.X - xToMove)/5f;
            yToMove += (-target.position.Y - yToMove)/5f;
            rotationToMove += -rotationToMove / 5f;

            var position = Matrix.CreateTranslation(xToMove, yToMove, 0);
            var offset = Matrix.CreateTranslation(
                (Settings.resolutionWidth / 2 / Settings.zoomLevel),
                (Settings.resolutionHeight / 2 / Settings.zoomLevel),
                0);
            var zoom = Matrix.CreateScale(Settings.zoomLevel);

            var rotation = Matrix.CreateRotationZ(rotationToMove);

            Transform = position * offset * zoom * rotation;
        }
        public void Follow(Vector2 target, Vector2 target2)
        {
            //This should place the camera in between two entities for bossfights, etc
            if (isFirstPass)
            {
                xToMove = (-target.X / 2 - target2.X / 2);
                yToMove = (-target.Y / 2 - target2.Y / 2);
                isFirstPass = false;
            }
            xToMove += ((-target.X / 2 - target2.X / 2) - xToMove)/5f;
            yToMove += ((-target.Y / 2 - target2.Y / 2) - yToMove)/5f;
            rotationToMove += -rotationToMove / 5f;

            var position = Matrix.CreateTranslation(xToMove, yToMove, 0);
            var offset = Matrix.CreateTranslation(
                (Settings.resolutionWidth / 2 / Settings.zoomLevel),
                (Settings.resolutionHeight / 2 / Settings.zoomLevel),
                0);
            var zoom = Matrix.CreateScale(Settings.zoomLevel);

            var rotation = Matrix.CreateRotationZ(rotationToMove);
            Transform = position * offset * zoom * rotation;

        }
    }
}

