using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player
{
    /// <summary>
    /// Contains everything pertaining to the player itself, but not drawing or anything, derived from Entity.
    /// </summary>
    internal class Player : Entity
    {
        HandlePlayerInputs inputHandler = new HandlePlayerInputs();
        public bool isInLevelEditorMode = false;
        private List<Weapon> weapons;

         public Player(int health, float entitySpeed, Vector2 startingTile, Hitboxes.Hitbox hitBox, ushort textureIndex) : base(entitySpeed, startingTile, textureIndex, hitBox, EntityMovement.AIType.none)
        {
            this.health = health;
            isEnabled = true;
        }

        public override void Update(Player player)
        {
            if (isEnabled)
            {
                TakeAction(player);
                activeWeapon.Update(inputHandler);

                if (inputHandler.IsShooting())
                    activeWeapon.Fire(position, true);

                if (movingSpeed > 0)
                {
                    isMoving = true;
                    animationHandler.SetAnimation(1);
                    animationHandler.SetAnimationSpeed(1, (int)(20000 / movingSpeed));
                }
                else
                {
                    isMoving = false;
                    animationHandler.SetAnimation(0);
                }

            }
        }

        public override void TakeAction(Player _)
        {
            if (!isInAbsMovementMode && isEnabled)
            {
                Vector2 playerNewPos = inputHandler.GetPlayerMovement(position, Game1.gameTime, entitySpeed);
                if (!ignoresCollisions)
                {
                    hitBox.UpdatePosition(playerNewPos.X, playerNewPos.Y);
                    playerNewPos = entityMovement.ValidateMovement(this, playerNewPos);
                }
                hitBox.UpdatePosition(playerNewPos.X, playerNewPos.Y);

                if (playerNewPos.X > position.X)
                    isFlipped = true;
                else
                    isFlipped = false;
                movingSpeed = (Math.Abs(playerNewPos.X - position.X) + Math.Abs(playerNewPos.Y - position.Y)) * Game1.gameTime.ElapsedGameTime.Milliseconds;
                if (movingSpeed > 0)
                    isMoving = true;
                else
                    isMoving = false;
                position = playerNewPos;
            }
            else if(isEnabled && movesLeft > 0)
            {
                Vector2 playerNewPos = inputHandler.GetPlayerTurnBasedMovement(position, Game1.gameTime, entitySpeed, movesLeft, isMoving, ignoresCollisions);

                if (playerNewPos.X > position.X)
                    isFlipped = true;
                else
                    isFlipped = false;
                movingSpeed = (Math.Abs(playerNewPos.X - position.X) + Math.Abs(playerNewPos.Y - position.Y)) * Game1.gameTime.ElapsedGameTime.Milliseconds;
                if (movingSpeed > 0)
                    isMoving = true;
                else
                    isMoving = false;
                position = playerNewPos;
            }
        }
    }
}
