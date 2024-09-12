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
            levelEditor.Update(player, Game1.currentEntities);
            Settings.Update();
        }
        public static void UpdateEntities( Player player)
        {
            foreach (Entity entity in Game1.currentEntities)
            {
                entity.Update(player);
            }
        }
    }
}
