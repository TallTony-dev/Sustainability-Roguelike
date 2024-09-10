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
        public void Update()
        {
var kstate = Keyboard.GetState();
            (int, int) playerAbsPos = TileMap.PosToAbsTileMapPos(player.position);
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
                if (kstate.IsKeyDown(Keys.Z))
                {
                    tileToPlace.textureIndex = 
                }
                if (kstate.IsKeyDown(Keys.X))
                {
                    tileToPlace.isBarrier = 
                }
                if (kstate.IsKeyDown(Keys.C))
                {
                    tileToPlace.statusGiven = 
                }
                if (kstate.IsKeyDown(Keys.V))
                {
                    tileToPlace.breakEffect = 
        }
    }
}
