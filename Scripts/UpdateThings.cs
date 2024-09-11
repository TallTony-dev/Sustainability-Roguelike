using System.Timers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts
{
    /// <summary>
    /// Class you made for updating entities and tiles? atleast entities rn
    /// </summary>
    internal static class UpdateThings
    {
        static double timeSinceT;
        public static void UpdateLevel(LevelEditor levelEditor, Player player, List<Entity> entityList, List<HUD.Menu> menuList)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.T) && (Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceT) > 1)
            {
                if (!levelEditor.isInEditor)
                {
                    player.position = new Vector2(TileMap.PosToAbsTileMapPos(player.position).Item1 * 32, TileMap.PosToAbsTileMapPos(player.position).Item2 * 32);
                    player.isInLevelEditorMode = true;
                    foreach (Entity entity in entityList)
                    {
                        entity.isInAbsMovementMode = true;
                    }
                    levelEditor.isInEditor = true;
                }
                else if (levelEditor.isInEditor)
                {
                    player.isInLevelEditorMode = false;
                    foreach (Entity entity in entityList)
                    {
                        entity.isInAbsMovementMode = false;
                    }
                    levelEditor.isInEditor = false;
                }
                timeSinceT = Game1.gameTime.TotalGameTime.TotalSeconds; 
            }

            if (levelEditor.isInEditor)
            {
                levelEditor.Update(player.position);
            }

            //foreach (HUD.Menu menu in menuList)
            //{
            //    menu.Update(); //WIP TODO
            //} This shouldnt behere I dont think, maybe make the menu list static and only call when nessacary?
        }
        public static void UpdateEntities(List<Entity> entityList, Player player)
        {
            foreach (Entity entity in entityList)
            {
                entity.Update(player);
            }
        }
    }
}
