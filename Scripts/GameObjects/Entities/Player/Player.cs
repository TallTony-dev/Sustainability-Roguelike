using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts.Entities.Player
{
    /// <summary>
    /// Contains everything pertaining to the player itself, but not drawing or anything, derived from Entity.
    /// </summary>
    internal class Player : Entity
    {
        public int health;
        HandlePlayerInputs inputHandler = new HandlePlayerInputs();

         public Player(int health, float entitySpeed, Vector2 startingPos, bool canDash, ushort textureIndex) : base(entitySpeed, startingPos, canDash, textureIndex,EntityMovement.AIType.none)
        {
            this.health = health;
        }

        public override void Move()
        {
            Vector2 playerNewPos = inputHandler.GetPlayerMovement(position, Game1.gameTime, entitySpeed);

            playerNewPos = entityMovement.ValidateMovement(this, playerNewPos);

            if (playerNewPos.X > position.X)
                isFlipped = true;
            else
                isFlipped = false;
            position = playerNewPos;
        }
    }
}
