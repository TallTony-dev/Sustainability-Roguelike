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
                    if ((kstate.IsKeyDown(Keys.W) || jstate.Buttons[(int)Buttons.DPadDown] == ButtonState.Pressed) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 - 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) - 32;
                    }

                    else if ((kstate.IsKeyDown(Keys.S) || jstate.Buttons[(int)Buttons.DPadUp] == ButtonState.Pressed) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 + 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) + 32;
                    }

                    else if ((kstate.IsKeyDown(Keys.A) || jstate.Buttons[(int)Buttons.DPadLeft] == ButtonState.Pressed) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 - 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) - 32;
                    }

                    else if ((kstate.IsKeyDown(Keys.D) || jstate.Buttons[(int)Buttons.DPadRight] == ButtonState.Pressed) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 + 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) + 32;
                    }
                }
                else
                {
                    if (kstate.IsKeyDown(Keys.W) && !TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 - 1))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) - 32;
                    }

                    else if (kstate.IsKeyDown(Keys.S) && !TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 + 1))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) + 32;
                    }

                    else if (kstate.IsKeyDown(Keys.A) && !TileMap.IsCollisionAbs(playerCurrentTile.Item1 - 1, playerCurrentTile.Item2))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) - 32;
                    }

                    else if (kstate.IsKeyDown(Keys.D) && !TileMap.IsCollisionAbs(playerCurrentTile.Item1 + 1, playerCurrentTile.Item2))
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
