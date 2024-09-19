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

        public float DistanceBetween (Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public Vector2 GetAbsPathfindingMovement(int attackRange, Vector2 currentPos, float speed, AIType aiType, Player.Player playerToPathFindTo)
        {
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


            if (distanceBetween >= attackRange + 0.5 || distanceBetween <= attackRange - 0.25)
            {
                if (pointsToTravelThrough.Count == 0 && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenPathfound > 0.25)
                {
                    pointsToTravelThrough = GetTileMapPointsToTravel(attackRange, currentPos, aiType, playerToPathFindTo);
                    timeWhenPathfound = Game1.gameTime.TotalGameTime.TotalSeconds;
                }

                if (pointsToTravelThrough.Count != 0)
                {
                    Vector2 coordToTravelTo = TileMap.TileMapPosToPos(pointsToTravelThrough.Last().Item1, pointsToTravelThrough.Last().Item2);
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
                        pointsToTravelThrough.Remove(pointsToTravelThrough.Last());
                    }
                } 
            }
            else if (!TileMap.DoesRaycastCollide(playerTileX, playerTileY, entityTileX, entityTileY))
            {
                pointsToTravelThrough.Clear();
            }
            return currentPos;  
        }

        private List<(int, int)> GetTileMapPointsToTravel(int attackRange, Vector2 currentPos, AIType aiType, Player.Player player)
        {
            Vector2 tileMapPos = Tiles.TileMap.PosToTileMapPos(currentPos);
            List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)> openTilesChecked = new List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)>();
            List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)> closedTilesChecked = new List<(int prevX, int prevY, int checkedX, int checkedY, int weightT)>();

            if (aiType == AIType.enemy)
            {
                (float entityTileX, float entityTileY) = TileMap.PosToTileMapPos(currentPos);
                (float playerTileX, float playerTileY) = TileMap.PosToTileMapPos(player.position);
                (int entityAbsTileX, int entityAbsTileY) = TileMap.PosToAbsTileMapPos(currentPos);
                (int playerAbsTileX, int playerAbsTileY) = TileMap.PosToAbsTileMapPos(player.position);

                openTilesChecked.Add((entityAbsTileX, entityAbsTileY, entityAbsTileX, entityAbsTileY, CalculateWeight(playerAbsTileX, playerAbsTileY, entityAbsTileX, entityAbsTileY)));

                bool exitLoop = false;
                int TileMapSize = (int)Math.Sqrt(TileMap.tileMap.Length);
                while (!exitLoop)
                {
                    int index = openTilesChecked.IndexOf(openTilesChecked.First(t => t.weightT == openTilesChecked.Min(x => x.weightT))); //gets index of item with the lowest t value, NOT SURE HOW HANDLES TWO WITH SAME, probably gets first one
                    (int prevX, int prevY, int checkedX, int checkedY, int weightT) = openTilesChecked[index];


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
                                    bool isHorizontallyWalkable = !TileMap.tileMap[prevX + x, prevY].isBarrier;
                                    bool isVerticallyWalkable = !TileMap.tileMap[prevX, prevY + y].isBarrier;

                                    //skip diagonal movement if either horizontal or vertical neighbor is blocked (corner-cut prevention)
                                    if (!isHorizontallyWalkable || !isVerticallyWalkable)
                                        continue;
                                }


                                //if it wasnt previously checked, and isnt a barrier
                                if (!closedTilesChecked.Any(t => t.checkedX == checkedX + x && t.checkedY == checkedY + y) && !TileMap.IsCollisionAbs(checkedX + x, checkedY + y))
                                {
                                    //then add it to the list
                                    weightT = CalculateWeight(playerAbsTileX, playerAbsTileY, checkedX + x, checkedY + y);
                                    openTilesChecked.Add((checkedX, checkedY, checkedX + x, checkedY + y, weightT));

                                    //if its in range and a raycast from there wouldn't collide with a tile NOT IMPLEMENTED
                                    float distanceBetweenTileAndPlayer = DistanceBetween(new Vector2(playerTileX, playerTileY), new Vector2(checkedX + x, checkedY + y));
                                    if (distanceBetweenTileAndPlayer <= attackRange + 0.5 && distanceBetweenTileAndPlayer >= attackRange - 0.25 && !TileMap.DoesRaycastCollide(playerTileX, playerTileY, checkedX + x, checkedY + y))
                                    {
                                        exitLoop = true;
                                    }
                                }
                            }
                        }
                    }
                    closedTilesChecked.Add(openTilesChecked[index]);
                    openTilesChecked.Remove(openTilesChecked[index]);

                }

                List<(int tileX, int tileY)> nodes = new List<(int, int)>();


                //last open tile will be in range and the end point
                nodes.Add((openTilesChecked.Last().checkedX, openTilesChecked.Last().checkedY));
                (int backTrackTileX, int backTrackTileY) = (openTilesChecked.Last().prevX, openTilesChecked.Last().prevY);
                do
                { //backtracking here
                    int currentTileIndex = closedTilesChecked.IndexOf(closedTilesChecked.First(t => t.checkedX == backTrackTileX && t.checkedY == backTrackTileY));
                    (int prevX, int prevY, int checkedX, int checkedY, int weightT) = closedTilesChecked[currentTileIndex];
                    nodes.Add((checkedX, checkedY));
                    (backTrackTileX, backTrackTileY) = (prevX, prevY);


                } while ((entityAbsTileX, entityAbsTileY) != nodes[nodes.Count - 1]);

                return nodes;
            }

            if (aiType == AIType.npc)
            {

            }
            return new List<(int, int)> { (0, 0) };
        }

        public int CalculateWeight(int playerTileX, int playerTileY, int tileX, int tileY)
        {
            int deltaX = Math.Abs(tileX - playerTileX);
            int deltaY = Math.Abs(tileY - playerTileY);
            return (deltaX * 10 + deltaY * 10 - (6 * Math.Min(deltaX, deltaY)));
        }
        public enum AIType { none, enemy, npc }
    } 
}
