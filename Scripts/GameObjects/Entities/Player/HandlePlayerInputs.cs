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
        double timeWhenDashed = 0;
        bool isDashing = false;
        Vector2 dashDirection;
        int oldScrollValue = 0;

        

        public float GetShootingAngle(Vector2 playerPos)
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
                var playerScreenPos = Vector2.Transform(playerPos, Game1.camera.Transform);
                y = ((mstate.Position.Y - (playerScreenPos.Y - Settings.resolutionHeight / 2)) - Settings.resolutionHeight / 2);
                x = ((mstate.Position.X - (playerScreenPos.X - Settings.resolutionWidth / 2)) - Settings.resolutionWidth / 2);
                
            }

            if (Math.Abs(x) > 0 || Math.Abs(y) > 0)
                angle = (float)(Math.Atan2(y, x));
            return angle;
        }
        public bool IsShooting()
        {
            if (StaticMouse.isContinuouslyPressed)
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
        public Vector2 GetPlayerMovement (Vector2 playerPosition, float playerSpeed)
        {
            float updatedPlayerSpeed = (float)(playerSpeed * Game1.gameTime.ElapsedGameTime.TotalSeconds);
            Vector2 oldPlayerPos = playerPosition;
            var kstate = Keyboard.GetState();
            
            if (isDashing)
            {
                float timeSinceDashed = (float)(Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenDashed);
                playerPosition.X += dashDirection.X * updatedPlayerSpeed * ((1 / (timeSinceDashed + 0.15f) - 1.6f) / 1.5f); // divide by 0 error here???
                playerPosition.Y += dashDirection.Y * updatedPlayerSpeed * ((1 / (timeSinceDashed + 0.15f) - 1.6f) / 1.5f);
                if (timeSinceDashed > 0.5)
                {
                    isDashing = false;
                }
            }

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

                if (!isDashing && kstate.IsKeyDown(Keys.Space) && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenDashed > 0.6)
                {
                    Vector2 movementDirection = new Vector2(playerPosition.X - oldPlayerPos.X, playerPosition.Y - oldPlayerPos.Y);

                    if (movementDirection != Vector2.Zero)
                    {
                        dashDirection = Vector2.Normalize(movementDirection);
                        isDashing = true;
                        timeWhenDashed = Game1.gameTime.TotalGameTime.TotalSeconds;
                    }
                }
            }
            else
            {
                JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);
                GamePadState gstate = GamePad.GetState((int)PlayerIndex.One);
                
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
                if (!isDashing && gstate.Triggers.Right > 0.1f && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenDashed > 0.6)
                {
                    dashDirection = Vector2.Normalize(new Vector2(playerPosition.X - oldPlayerPos.X, playerPosition.Y - oldPlayerPos.Y));
                    isDashing = true;
                    timeWhenDashed = Game1.gameTime.TotalGameTime.TotalSeconds;
                }
            }
            if (playerPosition.X == float.NaN)
                throw new FieldAccessException();
            return playerPosition;
        }


        double timeWhenChangedWeapons = 0;
        public short GetWeaponIndex(short currentWeapon, int inventoryCount)
        {
            double deltaTime = Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenChangedWeapons;

            if (Joystick.LastConnectedIndex != 0 && deltaTime > 0.1)
            {
                KeyboardState kstate = Keyboard.GetState();
                MouseState newMouseState = Mouse.GetState();
                int deltaScroll = newMouseState.ScrollWheelValue - oldScrollValue;
                if (kstate.IsKeyDown(Keys.V) || deltaScroll > 0)
                {
                    currentWeapon += 1;
                }
                else if (kstate.IsKeyDown(Keys.C) || deltaScroll < 0)
                {
                    currentWeapon -= 1;
                }
                        
                oldScrollValue = newMouseState.ScrollWheelValue;
                timeWhenChangedWeapons = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            else if (deltaTime > 0.1)
            {
                GamePadState gstate = GamePad.GetState((int)PlayerIndex.One);
                if (gstate.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    currentWeapon -= 1;
                }
                if (gstate.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    currentWeapon += 1;
                }
                timeWhenChangedWeapons = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            if (currentWeapon < 0)
                currentWeapon++;
            if (currentWeapon > inventoryCount - 1)
                currentWeapon--;

            return currentWeapon;
        }


        public Vector2 GetPlayerTurnBasedMovement(Vector2 playerPosition, GameTime gameTime, float playerSpeed, int movesLeft, bool isMoving, bool ignoreCollisions)
        {
            (int,int) playerCurrentTile = TileMap.PosToAbsTileMapPos(playerPosition);
            float updatedPlayerSpeed = (float)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            
            if (!isMoving && movesLeft > 0)
            {
                coordToTravelTo = playerPosition;
                KeyboardState kstate = Keyboard.GetState();
                GamePadState jstate = GamePad.GetState((int)PlayerIndex.One);
                if (jstate.IsConnected)
                {
                    if ((jstate.DPad.Down == ButtonState.Pressed || kstate.IsKeyDown(Keys.W)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 - 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) - 32;
                    }

                    else if ((jstate.DPad.Up == ButtonState.Pressed || kstate.IsKeyDown(Keys.S)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1, playerCurrentTile.Item2 + 1) || (ignoreCollisions)))
                    {
                        coordToTravelTo.Y = (playerCurrentTile.Item2 * 32) + 32;
                    }

                    else if ((jstate.DPad.Left == ButtonState.Pressed || kstate.IsKeyDown(Keys.A)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 - 1, playerCurrentTile.Item2) || (ignoreCollisions)))
                    {
                        coordToTravelTo.X = (playerCurrentTile.Item1 * 32) - 32;
                    }

                    else if ((jstate.DPad.Right == ButtonState.Pressed || kstate.IsKeyDown(Keys.D)) && (!TileMap.IsCollisionAbs(playerCurrentTile.Item1 + 1, playerCurrentTile.Item2) || (ignoreCollisions)))
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
