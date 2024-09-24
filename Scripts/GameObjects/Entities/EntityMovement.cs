using System.Linq;
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
        List<(int, int)> pointsToTravelThrough = new List<(int, int)> ();
        private double timeWhenPathfound = 0;
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
                    if (!(oldAbsTileX + x <= minVal || oldAbsTileY + y <= minVal || oldAbsTileX + x >= maxVal || oldAbsTileY + y >= maxVal))
                    {
                        (bool isBarrier, Hitboxes.Hitbox tileBounds) = TileMap.GetTileBounds(oldAbsTileX + x, oldAbsTileY + y); //checks min and max in here
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
            }
            return entityNewPos;
        }
    


        public EntityMovement(AIType aiType)
        {
            this.aiType = aiType;
        }

        public static float DistanceBetween (Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public Vector2 GetAbsPathfindingMovement(int attackRange, Vector2 currentPos, float speed, AIType aiType, Player.Player playerToPathFindTo)
        {
            throw new NotImplementedException();



            return new Vector2(0, 0);
        }


        /// <summary>
        /// Pathfinds to a player
        /// </summary>
        public Vector2 GetNormalPathfindingMovement(int attackRange, Vector2 currentPos, float speed, AIType aiType, Player.Player playerToPathFindTo)
        {

            (float playerTileX, float playerTileY) = TileMap.PosToTileMapPos(playerToPathFindTo.position);
            (float entityTileX, float entityTileY) = TileMap.PosToTileMapPos(currentPos);
            float distanceBetween = DistanceBetween(new Vector2(playerTileX, playerTileY), new Vector2(entityTileX, entityTileY));


            if (distanceBetween >= attackRange + 1 || distanceBetween <= attackRange - 0.2 || TileMap.DoesRaycastCollide(playerTileX, playerTileY, entityTileX, entityTileY, 30, 30))
            {
                if (pointsToTravelThrough.Count == 0 && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenPathfound > 0.25)
                {
                    pointsToTravelThrough = GetTileMapPointsToTravel(attackRange, currentPos, aiType, playerToPathFindTo);
                    timeWhenPathfound = Game1.gameTime.TotalGameTime.TotalSeconds;
                }
            }

            if (pointsToTravelThrough.Count != 0)
            {
                Vector2 coordToTravelTo = TileMap.TileMapPosToPos(pointsToTravelThrough.Last().Item1, pointsToTravelThrough.Last().Item2);

                TileMap.SetPathfindingModeOfTile(pointsToTravelThrough.Last().Item1, pointsToTravelThrough.Last().Item2, true);

                float updatedEntitySpeed = (float)(speed * Game1.gameTime.ElapsedGameTime.TotalSeconds);

                float nextX = currentPos.X + updatedEntitySpeed * Math.Sign(coordToTravelTo.X - currentPos.X);
                float nextY = currentPos.Y + updatedEntitySpeed * Math.Sign(coordToTravelTo.Y - currentPos.Y);

                if (Math.Sign(coordToTravelTo.X - currentPos.X) != Math.Sign(coordToTravelTo.X - nextX)) //checks for x overshoot
                    currentPos.X = coordToTravelTo.X;
                else
                    currentPos.X = nextX;

                if (Math.Sign(coordToTravelTo.Y - currentPos.Y) != Math.Sign(coordToTravelTo.Y - nextY)) //checks for y overshoot
                    currentPos.Y = coordToTravelTo.Y;
                else
                    currentPos.Y = nextY;

                if (currentPos == coordToTravelTo)
                {
                    TileMap.SetPathfindingModeOfTileAtCoord(coordToTravelTo.X, coordToTravelTo.Y, false);
                    pointsToTravelThrough.Remove(pointsToTravelThrough.Last());

                    if (pointsToTravelThrough.Count != 0 && TileMap.IsCollisionAbs(pointsToTravelThrough.Last().Item1, pointsToTravelThrough.Last().Item2))
                        pointsToTravelThrough.Clear();

                }
            } 
            return currentPos;  
        }

        private List<(int, int)> GetTileMapPointsToTravel(int attackRange, Vector2 currentPos, AIType aiType, Player.Player player)
        {
            Vector2 tileMapPos = Tiles.TileMap.PosToTileMapPos(currentPos);
            List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)> openTilesChecked = new List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)>();
            List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)> closedTilesChecked = new List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)>();

            int timesFailedToFind = 0;

            (float entityTileX, float entityTileY) = TileMap.PosToTileMapPos(currentPos);
            (float playerTileX, float playerTileY) = TileMap.PosToTileMapPos(player.position);
            (int entityAbsTileX, int entityAbsTileY) = TileMap.PosToAbsTileMapPos(currentPos);
            (int playerAbsTileX, int playerAbsTileY) = TileMap.PosToAbsTileMapPos(player.position);

            if (aiType == AIType.enemy)
            {

                openTilesChecked.Add((entityAbsTileX, entityAbsTileY, entityAbsTileX, entityAbsTileY, 0));

                bool exitLoop = false;
                int TileMapSize = (int)Math.Sqrt(TileMap.tileMap.Length);

                while (!exitLoop && timesFailedToFind < 2)
                {
                    int index = openTilesChecked.IndexOf(openTilesChecked.First(t => t.weightT == openTilesChecked.Min(x => x.weightT))); //gets index of item with the lowest t value
                    (int prevX, int prevY, int checkedX, int checkedY, int weightT) = openTilesChecked[index];


                    //if its trying to go too far away (MEH impossible movement test) then exit loop
                    if (Math.Abs(checkedX - entityAbsTileX) + Math.Abs(checkedY - entityAbsTileY) > 1 + attackRange * 2)
                    {
                        timesFailedToFind++;
                        exitLoop = true;
                    }


                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            if (!exitLoop)
                            {
                                //if its the previous tile then skip
                                if (x == 0 && y == 0)
                                    continue;
                                //if its out of tilemap bounds then skip
                                if (prevX - 1 < 0 || prevY - 1 < 0 || prevX + 1 >= TileMapSize || prevY + 1 >= TileMapSize)
                                    continue;

                                //if its a diagonal
                                if (x != 0 && y != 0)
                                {
                                    //get neighboring horizontal and vertical tiles between diagonal to check corner cutting
                                    bool isHorizontallyWalkable = !TileMap.tileMap[checkedX + x, checkedY].isBarrier;
                                    bool isVerticallyWalkable = !TileMap.tileMap[checkedX, checkedY + y].isBarrier;

                                    //skip diagonal movement if either horizontal or vertical neighbor is blocked (corner-cut prevention)
                                    if (!isHorizontallyWalkable || !isVerticallyWalkable)
                                        continue;
                                }


                                //if it wasnt previously checked, and isnt a collision
                                if (!closedTilesChecked.Any(t => t.checkedX == checkedX + x && t.checkedY == checkedY + y) && !openTilesChecked.Any(t => t.checkedX == checkedX + x && t.checkedY == checkedY + y) && 
                                    !TileMap.IsCollisionAbs(checkedX + x, checkedY + y))
                                {
                                    //then add it to the list
                                    weightT = CalculateWeight(checkedX + x, checkedY + y, playerAbsTileX, playerAbsTileY, attackRange);
                                    openTilesChecked.Add((checkedX, checkedY, checkedX + x, checkedY + y, weightT));

                                    //if its in range and a raycast from there wouldn't collide with a tile NOT IMPLEMENTED
                                    float tileDistanceBetweenTileAndPlayer = DistanceBetween(new Vector2(playerTileX, playerTileY), new Vector2(checkedX + x, checkedY + y));
                                    if (tileDistanceBetweenTileAndPlayer <= attackRange + 1 && tileDistanceBetweenTileAndPlayer >= attackRange - 0.2 && !TileMap.DoesRaycastCollide(playerTileX, playerTileY, checkedX + x, checkedY + y, 30, 30))
                                    {
                                        exitLoop = true;
                                    }
                                }
                            }
                        }
                    }

                    if (openTilesChecked.Count != 1)
                    {
                        closedTilesChecked.Add(openTilesChecked[index]);
                        openTilesChecked.Remove(openTilesChecked[index]);
                    }
                    else
                        timesFailedToFind++;
                }
            }

            if (aiType == AIType.npc)
            {

            }

            List<(int tileX, int tileY)> nodes = new List<(int, int)>();

            if (closedTilesChecked.Count > 0)
            {
                int backTrackTileX;
                int backTrackTileY;
                if (timesFailedToFind == 0)
                {
                    //last open tile will be in range and the end point
                    nodes.Add((openTilesChecked.Last().checkedX, openTilesChecked.Last().checkedY));
                    (backTrackTileX, backTrackTileY) = (openTilesChecked.Last().prevX, openTilesChecked.Last().prevY);
                }
                else
                {
                    //if its known to be going way too far away, it will default to the lowest weight position
                    int index = openTilesChecked.IndexOf(openTilesChecked.First(t => t.weightT == openTilesChecked.Min(x => x.weightT))); //gets index of item with the lowest t (weight) value
                    nodes.Add((openTilesChecked[index].checkedX, openTilesChecked[index].checkedY));
                    (backTrackTileX, backTrackTileY) = (openTilesChecked[index].prevX, openTilesChecked[index].prevY);
                }

                do
                { //backtracking here
                    int currentTileIndex = closedTilesChecked.IndexOf(closedTilesChecked.First(t => t.checkedX == backTrackTileX && t.checkedY == backTrackTileY));
                    (int prevX, int prevY, int checkedX, int checkedY, int weightT) = closedTilesChecked[currentTileIndex];
                    nodes.Add((checkedX, checkedY));
                    (backTrackTileX, backTrackTileY) = (prevX, prevY);
                } while ((entityAbsTileX, entityAbsTileY) != nodes[nodes.Count - 1]);

                nodes.RemoveAt(nodes.Count - 1);
                TileMap.tileMap[nodes.Last().tileX, nodes.Last().tileY].isBeingPathfoundTo = true;
            }

            return nodes;
        }

        public int CalculateWeight(int tileX, int tileY, int playerTileX, int playerTileY, int attackRange)
        {
            int deltaX = Math.Abs(tileX - playerTileX);
            int deltaY = Math.Abs(tileY - playerTileY);
            return (deltaX * 10 + deltaY * 10 - (6 * Math.Min(deltaX, deltaY)) + Math.Abs(deltaX - attackRange) * 10 + Math.Abs(deltaY - attackRange) * 10); 
            //how far it is from the player minus diagonal shortcuts possible, + the distance from attack range, TODO: maybe should skip how far it is from the player 
        }
        public enum AIType { none, enemy, npc }
    } 
}
