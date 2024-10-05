using System.Timers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts
{
    /// <summary>
    /// Class you made for updating entities and tiles? atleast entities rn
    /// </summary>
    internal static class UpdateThings
    {
        public static void UpdateLevel(LevelEditor levelEditor, Player player)
        {
            TurnManager.Update(player);
            levelEditor.Update(player);
            Settings.Update();
            foreach (Menu menu in Game1.menus)
            {
                menu.UpdateElements();
            }
            if (Game1.activePlayerProjectiles.Count > 0)
            {
                for (int x = Game1.activePlayerProjectiles.Count - 1; x > -1; x--)
                {
                    Game1.activePlayerProjectiles[x].Update(true);
                }
            }
            if (Game1.activeEnemyProjectiles.Count > 0)
            {
                for (int x = Game1.activeEnemyProjectiles.Count - 1; x > -1; x--)
                {
                    Game1.activeEnemyProjectiles[x].Update(false);
                }
            }
            if (Game1.activeParticleEmitters.Count > 0)
            {
                for (int x = Game1.activeParticleEmitters.Count - 1; x > -1; x--)
                {
                    Game1.activeParticleEmitters[x].Update();
                }
            }
            if (Game1.activeParticles.Count > 0)
            {
                for (int x = Game1.activeParticles.Count - 1; x > -1; x--)
                {
                    Game1.activeParticles[x].Update();
                }
            }
            for (int x = 0; x < Math.Sqrt(LevelGenerator.rooms.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(LevelGenerator.rooms.Length); y++)
                {
                    LevelGenerator.rooms[x, y].Update(player);
                }
            }
        }
        public static void UpdateEntities(Player player)
        {
            if (Game1.currentGameObjects.Count > 0)
            {
                for (int x = Game1.currentGameObjects.Count - 1; x > -1; x--)
                {
                    if (Game1.currentGameObjects[x] is Entity)
                    {
                        Entity entity = (Entity)Game1.currentGameObjects[x];
                        entity.Update(player);
                        entity.UpdateAnimation();
                    }
                }
            }
        }
    }
}
