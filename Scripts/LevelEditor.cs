using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;

namespace Monogame_Cross_Platform.Scripts
{
    public class LevelEditor
    {
        public bool isInEditor = false;
        public bool isChangingTile;
        Tile tileToPlace = new Tile();
        ushort selectedTextureIndex = 0;
        bool selectedIsBarrier = false;
        byte selectedStatusGiven = 0;
        byte selectedBreakEffect = 0;

        public void Update(Vector2 playerPosition)
        {
        var kstate = Keyboard.GetState();
            (int, int) playerAbsPos = TileMap.PosToAbsTileMapPos(playerPosition);
            if (kstate.IsKeyDown(Keys.E))
            {
                TileMap.tileMap[playerAbsPos.Item1, playerAbsPos.Item2] = tileToPlace;
            }
            if (kstate.IsKeyDown(Keys.Q) && !isChangingTile)
            {
                isChangingTile = true;
            }
            if (isChangingTile)
            {
                //TODO: Make this be using a menu with buttons
                if (kstate.IsKeyDown(Keys.Z))
                {
                    //selectedTextureIndex =
                }
                if (kstate.IsKeyDown(Keys.X))
                {
                    //selectedIsBarrier =
                }
                if (kstate.IsKeyDown(Keys.C))
                {
                    //selectedStatusGiven = 
                }
                if (kstate.IsKeyDown(Keys.V))
                {
                    //selectedBreakEffect = 
                }
            }
        }
    }
}
