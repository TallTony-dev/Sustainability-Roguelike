using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player
{
    public class HandlePlayerInputs
    {
        int deadZone = 4096;
        Vector2 coordToTravelTo = new Vector2(0, 0);
        public float GetShootingAngle()
        {
            float angle = 0;
            var mstate = Mouse.GetState();
            float y = 0;
            float x = 0;

            if (Joystick.LastConnectedIndex == 0)
            {
                JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);
                
                if (jstate.Axes[3] < -deadZone || jstate.Axes[3] > deadZone)
                {
                    y = (float)jstate.Axes[3];
                }

                if (jstate.Axes[2] < -deadZone || jstate.Axes[2] > deadZone)
                {
                    x = (float)jstate.Axes[2];
                }
            }
            else
            {
                y = (mstate.Position.Y - Settings.resolutionHeight / 2);
                x = (mstate.Position.X - Settings.resolutionWidth / 2);
            }

            if (Math.Abs(x) > 0 || Math.Abs(y) > 0)
                angle = (float)(Math.Atan2(y, x));
            return angle;
        }
        public bool IsShooting()
        {
            var mstate = Mouse.GetState();
            if (mstate.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            if (Joystick.LastConnectedIndex == 0)
            {
                JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);

                if (jstate.Axes[2] > deadZone || jstate.Axes[2] < -deadZone || jstate.Axes[3] > deadZone || jstate.Axes[3] < -deadZone)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Gets a Vector2 of the location the player should be in, it also gets the inputs required for this function.
        /// </summary>
        public Vector2 GetPlayerMovement (Vector2 playerPosition, GameTime gameTime, float playerSpeed)
        {
            float updatedPlayerSpeed = (float)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            var kstate = Keyboard.GetState();
            if (Joystick.LastConnectedIndex != 0)
            {
                if (kstate.IsKeyDown(Keys.W))
                {
                    playerPosition.Y -= updatedPlayerSpeed;
                }

                if (kstate.IsKeyDown(Keys.S))
                {
                    playerPosition.Y += updatedPlayerSpeed;
                }

                if (kstate.IsKeyDown(Keys.A))
                {
                    playerPosition.X -= updatedPlayerSpeed;
                }

                if (kstate.IsKeyDown(Keys.D))
                {
                    playerPosition.X += updatedPlayerSpeed;
                }
            }
            else
            {
                JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);

                if (jstate.Axes[1] < -deadZone)
                {
                    playerPosition.Y -= (updatedPlayerSpeed / 8) * ((Math.Abs(jstate.Axes[1]) / 4096));
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    playerPosition.Y += (updatedPlayerSpeed / 8) * ((Math.Abs(jstate.Axes[1]) / 4096));
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    playerPosition.X -= (updatedPlayerSpeed / 8) * ((Math.Abs(jstate.Axes[0]) / 4096));
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    playerPosition.X += (updatedPlayerSpeed / 8) * ((Math.Abs(jstate.Axes[0]) / 4096));
                }
            }
            return playerPosition;
        }

        public Vector2 GetPlayerTurnBasedMovement(Vector2 playerPosition, GameTime gameTime, float playerSpeed, int movesLeft, bool isMoving, bool ignoreCollisions)
        {
            (int,int) playerCurrentTile = TileMap.PosToAbsTileMapPos(playerPosition);
            float updatedPlayerSpeed = (float)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            
            if (!isMoving && movesLeft > 0)
            {
                coordToTravelTo = playerPosition;
                KeyboardState kstate = Keyboard.GetState();
                JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);
                if (jstate.IsConnected)
                {
                    if ((jstate.Buttons[(int)Buttons.DPadDown] == ButtonState.Pressed || kstate.IsKeyDown(Keys.W)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 - 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) - 32;
                    }

                    else if ((jstate.Buttons[(int)Buttons.DPadUp] == ButtonState.Pressed || kstate.IsKeyDown(Keys.S)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 + 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) + 32;
                    }

                    else if ((jstate.Buttons[(int)Buttons.DPadLeft] == ButtonState.Pressed || kstate.IsKeyDown(Keys.A)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 - 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) - 32;
                    }

                    else if ((jstate.Buttons[(int)Buttons.DPadRight] == ButtonState.Pressed || kstate.IsKeyDown(Keys.D)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 + 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) + 32;
                    }
                }
                else
                {
                    if (kstate.IsKeyDown(Keys.W) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 - 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) - 32;
                    }

                    else if (kstate.IsKeyDown(Keys.S) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 + 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) + 32;
                    }

                    else if (kstate.IsKeyDown(Keys.A) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 - 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) - 32;
                    }

                    else if (kstate.IsKeyDown(Keys.D) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 + 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) + 32;
                    }
                }

            }
            if (playerPosition != coordToTravelTo)
            {
                float nextX = playerPosition.X + updatedPlayerSpeed * Math.Sign(coordToTravelTo.X - playerPosition.X);
                float nextY = playerPosition.Y + updatedPlayerSpeed * Math.Sign(coordToTravelTo.Y - playerPosition.Y);

                if (Math.Sign(coordToTravelTo.X - playerPosition.X) != Math.Sign(coordToTravelTo.X - nextX)) //checks for x overshoot
                    playerPosition.X = coordToTravelTo.X;
                else
                    playerPosition.X = nextX;

                if (Math.Sign(coordToTravelTo.Y - playerPosition.Y) != Math.Sign(coordToTravelTo.Y - nextY)) //checks for y overshoot
                    playerPosition.Y = coordToTravelTo.Y;
                else
                    playerPosition.Y = nextY;

            }

            return playerPosition;
        }   

    }
}
