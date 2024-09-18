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
            float maxVal = 511f;
            float minVal = 0f;
            Hitboxes.Hitbox entityHitBox = entity.hitBox;
            if (tileMapX >= maxVal)
                entityNewPos.X = maxVal * entityHitBox.width;
            if (tileMapX <= minVal)
                entityNewPos.X = minVal * entityHitBox.width;
            if (tileMapY >= maxVal)
                entityNewPos.Y = maxVal * entityHitBox.height;
            if (tileMapY <= minVal)
                entityNewPos.Y = minVal * entityHitBox.height;

            (int oldAbsTileX, int oldAbsTileY) = TileMap.PosToAbsTileMapPos(entity.position); //old position
            (int newAbsTileX, int newAbsTileY) = TileMap.PosToAbsTileMapPos(entityNewPos); //new position
            int xTileDifference = Math.Abs(newAbsTileX - oldAbsTileX);
            int yTileDifference = Math.Abs(newAbsTileY - oldAbsTileY); //can be optimized

            bool isXModified = false;
            bool isYModified = false;
            for (int x = -1 - xTileDifference; x < 2 + xTileDifference; x++)
            {
                for (int y = -1 - yTileDifference; y < 2 + yTileDifference; y++)
                {
                    (bool isBarrier, Hitboxes.Hitbox tileBounds) = TileMap.GetTileBounds(oldAbsTileX + x, oldAbsTileY + y);
                    if (isBarrier)
                    {
                        if (x != 0 && !isXModified && entity.position.Y + entityHitBox.height / 2 > tileBounds.top && entity.position.Y - entityHitBox.height / 2 < tileBounds.bottom)
                        {
                            //if new position is past hitbox, and if its old position wasn't past hitbox
                            if (x < 0 && entityHitBox.left < tileBounds.right && entity.position.X - entityHitBox.width / 2 >= tileBounds.right) // Checking left tile
                            {
                                entityNewPos.X = tileBounds.right + entityHitBox.width / 2;
                                isXModified = true;
                            }
                            if (x > 0 && entityHitBox.right > tileBounds.left && entity.position.X + entityHitBox.width / 2 <= tileBounds.left) // Checking right tile
                            {
                                entityNewPos.X = tileBounds.left - entityHitBox.width / 2;
                                isXModified = true;
                            }
                        }

                        if (y != 0 && !isYModified && entity.position.X + entityHitBox.width / 2 > tileBounds.left && entity.position.X - entityHitBox.width / 2 < tileBounds.right)
                        {
                            if (y < 0 && entityHitBox.top < tileBounds.bottom && entity.position.Y - entityHitBox.height / 2 >= tileBounds.bottom) // Checking top tile
                            {
                                entityNewPos.Y = tileBounds.bottom + entityHitBox.height / 2;
                                isYModified = true;
                            }
                            if (y > 0 && entityHitBox.bottom > tileBounds.top && entity.position.Y + entityHitBox.height / 2 <= tileBounds.top) // Checking bottom tile
                            {
                                entityNewPos.Y = tileBounds.top - entityHitBox.height / 2;
                                isYModified = true;
                            }
                        }
                    }

                }
            }
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
