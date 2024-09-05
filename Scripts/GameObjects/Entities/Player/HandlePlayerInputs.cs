using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cross_Platform.Scripts.Entities.Player
{
    public class HandlePlayerInputs
    {
        /// <summary>
        /// Gets a Vector2 of the location the player should be in, it also gets the inputs required for this function.
        /// </summary>
        public Vector2 GetPlayerMovement (Vector2 playerPosition, GameTime gameTime, float playerSpeed)
        {
            float updatedPlayerSpeed = (float)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            int deadZone = 4096;
            var kstate = Keyboard.GetState();

            if (Joystick.LastConnectedIndex != 0)
            {
                if (kstate.IsKeyDown(Keys.Up))
                {
                    playerPosition.Y -= updatedPlayerSpeed * 4;
                }

                if (kstate.IsKeyDown(Keys.Down))
                {
                    playerPosition.Y += updatedPlayerSpeed * 4;
                }

                if (kstate.IsKeyDown(Keys.Left))
                {
                    playerPosition.X -= updatedPlayerSpeed * 4;
                }

                if (kstate.IsKeyDown(Keys.Right))
                {
                    playerPosition.X += updatedPlayerSpeed * 4;
                }
            }


            if (Joystick.LastConnectedIndex == 0)
            {
                JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);

                if (jstate.Axes[1] < -deadZone)
                {
                    playerPosition.Y -= updatedPlayerSpeed * (Math.Abs(jstate.Axes[1]) / 4096);
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    playerPosition.Y += updatedPlayerSpeed * (Math.Abs(jstate.Axes[1]) / 4096);
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    playerPosition.X -= updatedPlayerSpeed * (Math.Abs(jstate.Axes[0]) / 4096);
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    playerPosition.X += updatedPlayerSpeed * (Math.Abs(jstate.Axes[0]) / 4096);
                }
            }
            return playerPosition;
        }
        
    }
}
