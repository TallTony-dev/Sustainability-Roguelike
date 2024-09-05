using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
using System.Collections.Generic;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities
{
    internal class EntityMovement
    {
        AIType aiType;
        float entitySpeed;
        internal Vector2 ValidateMovement(Entity entity, Vector2 entityNewPos)
        {
            (float tileMapX, float tileMapY) = TileMap.PosToTileMapPos(entityNewPos);
            float maxVal = 4094f;
            float minVal = 1f;
            if (tileMapX >= maxVal)
                entityNewPos.X = maxVal * 32;
            if (tileMapX <= minVal)
                entityNewPos.X = minVal * 32;
            if (tileMapY >= maxVal)
                entityNewPos.Y = maxVal * 32;
            if (tileMapY <= minVal)
                entityNewPos.Y = minVal * 32;

            (int absTileX, int absTileY) = TileMap.PosToAbsTileMapPos(entityNewPos);
            Rectangle entityHitBox = entity.hitboxManager.hitBox;
            bool isXModified = false;
            bool isYModified = false;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (TileMap.IsCollision(entity, x+absTileX, y+absTileY))
                    {
                        Rectangle tileBounds = TileMap.GetTileBounds(absTileX + x, absTileY + y);

                        if (x != 0 && !isXModified && entityHitBox.Bottom - 2 > tileBounds.Top && entityHitBox.Top + 2 < tileBounds.Bottom)
                        {
                            if (x < 0) // Checking left tile
                            {
                                entityNewPos.X = tileBounds.Right + entityHitBox.Width / 2;
                                isXModified = true;
                            }
                            if (x > 0) // Checking right tile
                            {
                                entityNewPos.X = tileBounds.Left - entityHitBox.Width / 2;
                                isXModified = true;
                            }
                        }

                        if (y != 0 && !isYModified && entityHitBox.Right - 2 > tileBounds.Left && entityHitBox.Left + 2 < tileBounds.Right)
                        {
                            //if right position is over the left tilebound or if the left position is under the right tilebound then run it
                            if (y < 0) // Checking top tile
                            {
                                entityNewPos.Y = tileBounds.Bottom + entityHitBox.Height / 2;
                                isYModified = true;
                            }
                            if (y > 0 ) // Checking bottom tile
                            {
                                entityNewPos.Y = tileBounds.Top - entityHitBox.Height / 2;
                                isYModified = true;
                            }
                        }
                    }
                }
            }
            //entityNewPos.Round();
            //Remove any decimals?
            return entityNewPos;
        }
    


        public EntityMovement(AIType aiType, Entity entity)
        {
            this.aiType = aiType;
            this.entitySpeed = entity.entitySpeed;
        }


        /// <summary>
        /// Pathfinds to a player
        /// </summary>
        public Vector2 GetPathfindingMovement(int attackRange, Vector2 currentPos, float speed, AIType aiType, Player.Player playerToPathFindTo)
        {
            var pointsToTravelThrough = GetTileMapPointsToTravel(attackRange, currentPos, aiType, playerToPathFindTo);
            return new Vector2(0, 0);
            //return new Vector2(pointsToTravelThrough[0]);   
        }

        private List<(int, int)> GetTileMapPointsToTravel(int attackRange, Vector2 currentPos, AIType aiType, Player.Player player)
        {
            Vector2 tileMapPos = Tiles.TileMap.PosToTileMapPos(currentPos);
            List<(short, short, short, short, short, bool)> tilesChecked = new List<(short, short, short, short, short, bool)>(); //previous y, previous x, checked y, checked x, weight, isbarrier
            List<(short, short, short, short, short)> edgeTilesChecked = new List<(short, short, short, short, short)>(); //previous y, previous x, checked y, checked x, weight
            //if (aiType == AIType.enemy)
            //{
            //    if (Vector2.Distance(currentPos, player.position) > attackRange && TileMap.PosToTileMapPos(currentPos)) //checks if not in range
            //    {
            //        tilesChecked.Add(currentPos.X)
            //    }
            //}

            if (aiType == AIType.npc)
            {

            }
            return new List<(int, int)> { (0, 0) };
        }
        public enum AIType { none, enemy, npc }
    } 
}
