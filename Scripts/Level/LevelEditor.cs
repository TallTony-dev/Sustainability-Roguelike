using System.Linq;
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
        bool selectedBreakable = false;
        ushort selectedDecoIndex = 0;

        Menu editorMenu = new Menu(Menu.MenuType.levelEditor);

        double timeSinceT;
        double timeSinceTilePlaced;

        internal void Update(Player player)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.T) && Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceT > 1)
            {
                if (!isInEditor)
                {
                    player.position = new Vector2(TileMap.PosToAbsTileMapPos(player.position).Item1 * 32, TileMap.PosToAbsTileMapPos(player.position).Item2 * 32);
                    player.isInLevelEditorMode = true;
                    player.isInComboMode = true;
                    player.ignoresCollisions = true;
                    Room room = LevelGenerator.PosToRoom(player.position);
                    for (int i = 0; i < room.gameObjects.Count; i++)
                    {
                        if (room.gameObjects[i] is Entity)
                        {
                            Entity entity = (Entity)room.gameObjects[i];
                            entity.isEnabled = false;
                            entity.ignoresCollisions = true;
                        }
                    }
                    player.isEnabled = true;
                    isInEditor = true;
                    editorMenu.EnableMenu();
                }
                else if (isInEditor)
                {
                    player.isInLevelEditorMode = false;
                    player.isInComboMode = false;
                    Room room = LevelGenerator.PosToRoom(player.position);
                    for (int i = 0; i < room.gameObjects.Count; i++)
                    {
                        if (room.gameObjects[i] is Entity)
                        {
                            Entity entity = (Entity)room.gameObjects[i];
                            entity.isEnabled = false;
                            entity.ignoresCollisions = true;
                        }
                    }
                    player.isEnabled = true;
                    isInEditor = false;
                    editorMenu.DisableMenu();
                }
                timeSinceT = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
            if (isInEditor)
            {
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.E) && Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceTilePlaced > 0.3)
                {
                    LevelGenerator.ChangeTileAtPos(player.position, selectedTextureIndex, selectedIsBarrier, selectedBreakable, selectedDecoIndex);
                    timeSinceTilePlaced = Game1.gameTime.TotalGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Keys.R) && Game1.gameTime.TotalGameTime.TotalSeconds - timeSinceTilePlaced > 0.3)
                {
                    LevelGenerator.Change3x3TilesAroundPos(player.position, selectedTextureIndex, selectedIsBarrier, selectedBreakable, selectedDecoIndex);
                    timeSinceTilePlaced = Game1.gameTime.TotalGameTime.TotalSeconds;
                }
                if (editorMenu.IsButtonPressed(0))
                {
                    Room room = LevelGenerator.PosToRoom(player.position);
                    if (room.isOpen)
                    {
                        room.CloseSides();
                        LevelGenerator.SetTileMapToRoom(room.roomArrayX, room.roomArrayY);
                    }
                    else
                    {
                        room.OpenSides();
                        LevelGenerator.SetTileMapToRoom(room.roomArrayX, room.roomArrayY);
                    }
                }
                if (editorMenu.IsButtonPressed(1))
                {
                    selectedTextureIndex = 0;
                }
                if (editorMenu.IsButtonPressed(2))
                {
                    selectedTextureIndex = 16;
                    selectedDecoIndex = 1;
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
                    Room room = LevelGenerator.PosToRoom(player.position);
                    Enemy enemy = new Enemy(0, player.position);

                    room.gameObjects.Add(enemy);
                    Game1.currentGameObjects.Add(enemy);
                }
                if (editorMenu.IsButtonPressed(6))
                {
                    Room room = LevelGenerator.PosToRoom(player.position);
                    if (room.gameObjects.Count > 0 && room.gameObjects.Any(t => t.position == player.position))
                    {
                        GameObject gameObject = room.gameObjects.First(t => t.position == player.position);
                        room.gameObjects.Remove(gameObject);
                        Game1.currentGameObjects.Remove(gameObject);
                    }
                }
                if (editorMenu.IsButtonPressed(7))
                {
                    LevelGenerator.WriteRoomToFile(player);
                }

            }
        }
    }
}
