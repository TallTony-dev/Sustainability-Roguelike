using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts
{
    /// <summary>
    /// State manager
    /// </summary>
    internal static class GameState
    {
        internal static bool isInComboMode = false;
        internal static bool isPlayerTurn = true;
        internal static bool isInGame = true;
        public static bool isPaused = false;
        public static Menu inGameMenu = new Menu(Menu.MenuType.inGameUi);

        public static void Update(Player player)
        {
            if (isInGame && !inGameMenu.isActive)
            {
                inGameMenu.EnableMenu();
            }
            else if (!isInGame && inGameMenu != null)
            {
                Game1.menus.Remove(inGameMenu);
                inGameMenu.DisableMenu();
            }
            MiniMap miniMap = (MiniMap)inGameMenu.elements[0];
            miniMap.UpdateMiniMap(player);

            Meter healthbar = (Meter)inGameMenu.elements[1];
            healthbar.maxVal = player.maxHealth;
            healthbar.Update(player.health);
            
            Inventory inventory = (Inventory)inGameMenu.elements[2];
            inventory.Update(player.weapons, player.activeWeaponIndex);

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
            foreach (Projectile projectile in Game1.activeEnemyProjectiles)
            {
                projectile.isEnabled = false;
            }
            foreach (Projectile projectile in Game1.activePlayerProjectiles)
            {
                projectile.isEnabled = false;
            }
            foreach (GameObject gameObject in LevelGenerator.PosToRoom(player.position).gameObjects)
            {
                gameObject.isEnabled = false;
            }
            player.isEnabled = false;
            isPaused = true;
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
            isPaused = false;
        }
    }
}
