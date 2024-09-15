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

            (int absTileX, int absTileY) = TileMap.PosToAbsTileMapPos(entity.position); //old psotiion
            bool isXModified = false;
            bool isYModified = false;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (TileMap.IsCollision(entity, x+absTileX, y+absTileY))
                    {
                        Rectangle tileBounds = TileMap.GetTileBounds(absTileX + x, absTileY + y);

                        //TODO: Fix corners by checking for collisions with tiles in corners as well
                        //entity.hitbox is updated to new position already

                        //If old entity left bound is
                        if (x != 0 && !isXModified && entity.position.Y + entityHitBox.height / 2 > tileBounds.Top && entity.position.Y - entityHitBox.height / 2 < tileBounds.Bottom)
                        {
                            //if new position is past hitbox, and if its old position wasn't past hitbox
                            if (x < 0 && entityHitBox.left < tileBounds.Right && entity.position.X - entityHitBox.width / 2 >= tileBounds.Right) // Checking left tile
                            {
                                entityNewPos.X = tileBounds.Right + entityHitBox.width / 2;
                                isXModified = true;
                            }
                            if (x > 0 && entityHitBox.right > tileBounds.Left && entity.position.X + entityHitBox.width / 2 <= tileBounds.Left ) // Checking right tile
                            {
                                entityNewPos.X = tileBounds.Left - entityHitBox.width / 2;
                                isXModified = true;
                            }
                        }

                        if (y != 0 && !isYModified && entity.position.X + entityHitBox.width / 2 > tileBounds.Left && entity.position.X - entityHitBox.width / 2 < tileBounds.Right)
                        {
                            if (y < 0 && entityHitBox.top < tileBounds.Bottom && entity.position.Y - entityHitBox.height / 2 >= tileBounds.Bottom) // Checking top tile
                            {
                                entityNewPos.Y = tileBounds.Bottom + entityHitBox.height / 2;
                                isYModified = true;
                            }
                            if (y > 0 && entityHitBox.bottom > tileBounds.Top && entity.position.Y + entityHitBox.height / 2 <= tileBounds.Top) // Checking bottom tile
                            {
                                entityNewPos.Y = tileBounds.Top - entityHitBox.height / 2;
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
