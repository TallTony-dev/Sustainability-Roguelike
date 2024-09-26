using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts
{
    internal static class TurnManager
    {
        internal static bool isInComboMode = false;
        internal static bool isPlayerTurn = true;
        internal static bool isInGame = true;

        public static void Update(Player player)
        {
            Room room = LevelGenerator.PosToRoom(player.position);
            if (!isInComboMode)
            {
                
            }
            else
            {

            }

        }
        /// <summary>
        /// Should pause all 'ingame' functions (movement, projectiles, etc) for pause menus and home menu
        /// </summary>
        public static void PauseGame(Player player)
        {
            foreach (GameObject gameObject in Game1.currentGameObjects)
            {
                gameObject.isEnabled = false;
            }
            foreach (Projectile projectile in Game1.activeEnemyProjectiles)
            {
                projectile.isEnabled = false;
            }
            foreach (Projectile projectile in Game1.activePlayerProjectiles)
            {
                projectile.isEnabled = false;
            }
            player.isEnabled = false;
        }
        /// <summary>
        /// Should resume all functions in the game
        /// </summary>
        public static void ResumeGame(Player player)
        {
            foreach (Projectile projectile in Game1.activeEnemyProjectiles)
            {
                projectile.isEnabled = true;
            }
            foreach (Projectile projectile in Game1.activePlayerProjectiles)
            {
                projectile.isEnabled = true;
            }
            foreach (GameObject gameObject in LevelGenerator.PosToRoom(player.position).gameObjects)
            {
                gameObject.isEnabled = true;
            }
            player.isEnabled = true;
        }
    }
}
