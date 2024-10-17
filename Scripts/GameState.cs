using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
        internal static bool isInGame = false;
        public static bool isPaused = true;
        public static Menu inGameMenu = new Menu(Menu.MenuType.inGameUi);
        public static Menu mainMenu = new Menu(Menu.MenuType.opening);
        public static Menu pauseMenu = new Menu(Menu.MenuType.pauseMenu);
        public static Menu winMenu = new Menu(Menu.MenuType.win);

        private static double timeWhenPaused = 0;

        public static void WinGame()
        {
            isInGame = false;
            isPaused = true;
            winMenu.EnableMenu();
        }

        public static void Update(Player player)
        {
            if (isInGame)
            {
                if (!inGameMenu.isActive)
                {
                    inGameMenu.EnableMenu();
                }
                if (mainMenu.isActive)
                    mainMenu.DisableMenu();

                if (!isPaused)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !isPaused && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenPaused > 0.5)
                    {
                        PauseGame(player);
                        timeWhenPaused = Game1.gameTime.TotalGameTime.TotalSeconds;
                    }

                }

                if (isPaused)
                {
                    if (!pauseMenu.isActive)
                        pauseMenu.EnableMenu();
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && isPaused && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenPaused > 0.5)
                    {
                        ResumeGame(player);
                        timeWhenPaused = Game1.gameTime.TotalGameTime.TotalSeconds;
                        pauseMenu.DisableMenu();
                    }
                    if (pauseMenu.IsButtonPressed(0))
                    {
                        isInGame = false;
                        pauseMenu.DisableMenu();
                    }
                    if (pauseMenu.IsButtonPressed(1))
                    {
                        ResumeGame(player);
                        pauseMenu.DisableMenu();
                    }
                }

                UpdateThings.UpdateLevel(Game1.levelEditor, player);
                UpdateThings.UpdateEntities(player);
            }
            else if (!isInGame)
            {
                if (inGameMenu.isActive)
                {
                    inGameMenu.DisableMenu();
                }
                if (!winMenu.isActive)
                {
                    if (!mainMenu.isActive)
                        mainMenu.EnableMenu();

                    if (mainMenu.IsButtonPressed(0))
                    {
                        isInGame = true;
                        ResumeGame(player);
                    }
                    if (mainMenu.IsButtonPressed(1))
                    {
                        Game1.ExitGame();
                    }
                }
                else
                {
                    if (winMenu.IsButtonPressed(0))
                    {
                        winMenu.DisableMenu();
                    }
                }
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
        public static void NextLevel()
        {
            //run animation for player here
            //have background be something temporary
            Game1.player.position = new Vector2(5664, 5664);

            for (int i = Game1.currentGameObjects.Count - 1; i >= 0; i--)
            {
                if (Game1.currentGameObjects[i] is Entity && !(Game1.currentGameObjects[i] is Player))
                    Game1.currentGameObjects[i].Destroy();
                else if (Game1.currentGameObjects[i] is Weapon)
                {
                    Weapon weapon = (Weapon)Game1.currentGameObjects[i];
                    if (weapon.owner == null)
                        weapon.Destroy();
                }
            }

            LevelGenerator.GenerateNextLevel();
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
