using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts.Level
{
    public class LevelEditor
    {
        public bool isInEditor = false;

        ushort selectedTextureIndex = 0;
        bool selectedIsBarrier = false;
        byte selectedStatusGiven = 0;
        byte selectedBreakEffect = 0;

        Menu editorMenu = new Menu(Menu.MenuType.levelEditor);

        static double timeSinceT;

        internal void Update(Player player, List<Entity> entityList)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.T) && Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceT > 1)
            {
                if (!isInEditor)
                {
                    player.position = new Vector2(TileMap.PosToAbsTileMapPos(player.position).Item1 * 32, TileMap.PosToAbsTileMapPos(player.position).Item2 * 32);
                    player.isInLevelEditorMode = true;
                    foreach (Entity entity in entityList)
                    {
                        entity.isInAbsMovementMode = true;
                    }
                    isInEditor = true;
                    editorMenu.EnableMenu();
                }
                else if (isInEditor)
                {
                    player.isInLevelEditorMode = false;
                    foreach (Entity entity in entityList)
                    {
                        entity.isInAbsMovementMode = false;
                    }
                    isInEditor = false;
                    editorMenu.DisableMenu();
                }
                timeSinceT = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            if (isInEditor)
            {
                var kstate = Keyboard.GetState();
                (int, int) playerAbsPos = TileMap.PosToAbsTileMapPos(player.position);
                if (kstate.IsKeyDown(Keys.E))
                {
                    LevelGenerator.ChangeTileAtPos(player.position, selectedTextureIndex, selectedIsBarrier, selectedBreakEffect, selectedStatusGiven);
                }
                //TODO: Make this be using a menu with buttons
                if (editorMenu.IsButtonPressed(0))
                {
                    LevelGenerator.UpdateTileMap();
                }
                if (editorMenu.IsButtonPressed(1))
                {
                    selectedTextureIndex = 0;
                }
                if (editorMenu.IsButtonPressed(2))
                {
                    selectedTextureIndex = 16;
                }
                if (editorMenu.IsButtonPressed(3))
                {
                    selectedIsBarrier = false;
                }
                if (editorMenu.IsButtonPressed(4))
                {
                    selectedIsBarrier = true;
                }
                if (editorMenu.IsButtonPressed(5))
                {
                    LevelGenerator.WriteRoomToFile(player);
                }

            }
        }
    }
}
