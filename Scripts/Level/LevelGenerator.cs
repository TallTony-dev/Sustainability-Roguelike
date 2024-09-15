using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;

namespace Monogame_Cross_Platform.Scripts.Level
{
    internal static class LevelGenerator
    {
        public static Room[,] rooms = new Room[11,11]; //must be square (odd preferred)
        static short sqrtRoomsLength = 11;
        /// <summary>
        /// Edits the static class tilemap to fit the current level selected by param levelNumber
        /// </summary>
        public static void GenerateLevel(byte levelType, int levelLength)
        {
            short sqrtRoomsLength = (short)Math.Sqrt(rooms.Length);
            (int index, ushort distance)[,] roomIndices = new (int, ushort)[sqrtRoomsLength,sqrtRoomsLength];
            (int x, int y) currentRoom = (5, 5);
            Random rand = new Random();

            //makes the entrance
            roomIndices[currentRoom.x, currentRoom.y] = (1, 0);

            //Generates active rooms
            while (levelLength > 0)
            {
                int roomChange = rand.Next(1, 5);
                if (roomChange == 1 && currentRoom.x + 1 < sqrtRoomsLength)
                    currentRoom.x += 1;
                if (roomChange == 2 && currentRoom.x > 0)
                    currentRoom.x -= 1;
                if (roomChange == 3 && currentRoom.y + 1 < sqrtRoomsLength)
                    currentRoom.y += 1;
                if (roomChange == 4 && currentRoom.y > 0)
                    currentRoom.y -= 1;

                if (roomIndices[currentRoom.x, currentRoom.y].index == 0)
                {
                    roomIndices[currentRoom.x, currentRoom.y].index = 1;
                    roomIndices[currentRoom.x, currentRoom.y].distance = (ushort)(Math.Abs(5 - currentRoom.x) + Math.Abs(5 - currentRoom.y));
                    levelLength--;
                }
            }

            (int roomX, int roomY) bossRoom = (0,0);
            ushort treasureRooms = 0;
            ushort otherRooms = 0;
            ushort enemyRooms = 0;
            (int roomX, int roomY) maxDistanceRoom = (0,0);

            //finds max distance
            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    if (roomIndices[x, y].distance > (roomIndices[maxDistanceRoom.roomX, maxDistanceRoom.roomY].distance))
                        maxDistanceRoom = (x, y);
                }
            }
            roomIndices[maxDistanceRoom.roomX, maxDistanceRoom.roomY].index = 6;

            //settles indices according to a set of rules
            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    if (roomIndices[x, y].index == 1 && roomIndices[x, y].distance != 0)
                    {
                        //settle the room indices starting here based off distance to the indices in document (1 is an entrance, 2 is an enemy room, 3 is an other room, 4 is a treasure room, 5 is a boss room, 6 is an exit)
                        bool isX0 = false;
                        bool isY0 = false;
                        bool isXAtArrayLimit = false;
                        bool isYAtArrayLimit = false;

                        if (x == 0)
                            isX0 = true;
                        if (y == 0)
                            isY0 = true;
                        if (x + 1 >= sqrtRoomsLength)
                            isXAtArrayLimit = true;
                        if (y + 1 >= sqrtRoomsLength)
                            isYAtArrayLimit = true;

                        ushort adjacentTiles = 0;
                        if (!isXAtArrayLimit && roomIndices[x + 1, y].index > 0)
                        {
                            adjacentTiles++;
                        }
                        if (!isX0 && roomIndices[x - 1, y].index > 0)
                        {
                            adjacentTiles++;
                        }
                        if (!isYAtArrayLimit && roomIndices[x, y + 1].index > 0)
                        {
                            adjacentTiles++;
                        }
                        if (!isY0 && roomIndices[x, y - 1].index > 0)
                        {
                            adjacentTiles++;
                        }

                        //If theres an adjacent exit and no bossrooms, makes a boss room
                        if (bossRoom == (0,0))
                        {
                            if (!isXAtArrayLimit && roomIndices[x + 1, y].index == 6)
                            {
                                roomIndices[x, y].index = 5;
                                bossRoom = (x, y);
                            }
                            else if (!isX0 && roomIndices[x - 1, y].index == 6)
                            {
                                roomIndices[x, y].index = 5;
                                bossRoom = (x, y);
                            }
                            else if (!isYAtArrayLimit && roomIndices[x, y + 1].index == 6)
                            {
                                roomIndices[x, y].index = 5;
                                bossRoom = (x, y);
                            }
                            else if (!isY0 && roomIndices[x, y - 1].index == 6)
                            {
                                roomIndices[x, y].index = 5;
                                bossRoom = (x, y);
                            }
                        }
                        if (roomIndices[x, y].index != 5)
                        {
                            //if is next to a boss room and is not 5, will bug if bossroom is 0,0
                            if (bossRoom != (0,0) && (bossRoom.roomX + 1 == x || bossRoom.roomX - 1 == x || bossRoom.roomY + 1 == y || bossRoom.roomY - 1 == y) && roomIndices[x, y].index != 5)
                            {
                                roomIndices[x, y].index = 0;
                            }
                            //If not an exit or boss room, If its a dead end and theres less than 2 treasure rooms, its a treasure room
                            if (adjacentTiles == 1 && treasureRooms < 2)
                            {
                                roomIndices[x, y].index = 4;
                                treasureRooms++;
                            }
                            //if enemy rooms are more than double or equal to double the number of other rooms, make it an other room
                            else if (enemyRooms >= otherRooms * 2)
                            {
                                roomIndices[x, y].index = 3;
                                otherRooms++;
                            }

                            //If nothing else, make it an enemy room
                            else
                            {
                                roomIndices[x, y].index = 2;
                                enemyRooms++;
                            }
                            //entrance is default so it just wont be inited due to being distance 0
                        }
                    }
                }
            }

            for(var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    rooms[x, y] = new Room(levelType, (ushort)roomIndices[x, y].index, rand, x, y);
                }
            }



            //Sets room properties that are about their position in the array, then sets the tilemap tiles to their tiles
            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    Room room = rooms[x, y];

                    if (x == 0)
                        room.isX0 = true;
                    if (y == 0)
                        room.isY0 = true;
                    if (x + 1 >= sqrtRoomsLength)
                        room.isXAtArrayLimit = true;
                    if (y + 1 >= sqrtRoomsLength)
                        room.isYAtArrayLimit = true;

                    bool isRightARoom = false;
                    bool isLeftARoom = false;
                    bool isBottomARoom = false;
                    bool isTopARoom = false;

                    if (!room.isXAtArrayLimit && rooms[x + 1, y].isARoom)
                    {
                        isRightARoom = true;
                    }
                    if (!room.isX0 && rooms[x - 1, y].isARoom)
                    {
                        isLeftARoom = true;
                    }
                    if (!room.isYAtArrayLimit && rooms[x, y + 1].isARoom)
                    {
                        isBottomARoom = true;
                    }
                    if (!room.isY0 && rooms[x, y - 1].isARoom)
                    {
                        isTopARoom = true;
                    }
                    //TODO: Add level specific bridges here
                    if (room.isARoom)
                        room.OpenSides();
                    if ((isRightARoom || isLeftARoom) && room.isARoom)
                    {
                        short bridgeSide = 0;
                        if (isRightARoom)
                            bridgeSide = 1;
                        if (isLeftARoom)
                            bridgeSide = -1;
                        //Generate horizontal bridges
                        if (levelType == 1)
                        {
                            for (var tilex = 0; tilex < 10; tilex++)
                            {
                                TileMap.tileMap[28 * x + tilex * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9, 28 * y + ((room.sqrtTileArrayLength - 1) / 2) - 2] = new Tile(18, true, 0, 0);
                                TileMap.tileMap[28 * x + tilex * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9, 28 * y + ((room.sqrtTileArrayLength - 1) / 2) - 1] = new Tile(6, false, 0, 0);
                                TileMap.tileMap[28 * x + tilex * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9, 28 * y + (room.sqrtTileArrayLength - 1) / 2] = new Tile(10, false, 0, 0);
                                TileMap.tileMap[28 * x + tilex * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9, 28 * y + ((room.sqrtTileArrayLength - 1) / 2) + 1] = new Tile(14, false, 0, 0);
                                TileMap.tileMap[28 * x + tilex * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9, 28 * y + ((room.sqrtTileArrayLength - 1) / 2) + 2] = new Tile(18, true, 0, 0);
                            }
                        }
                    }
                    if ((isTopARoom || isBottomARoom) && room.isARoom)
                    {
                        short bridgeSide = 0;
                        if (isBottomARoom)
                            bridgeSide = 1;
                        if (isTopARoom)
                            bridgeSide = -1;
                        //Generate vertical bridges
                        if (levelType == 1)
                        {
                            for (var tiley = 0; tiley < 10; tiley++)
                            {
                                TileMap.tileMap[28 * x + ((room.sqrtTileArrayLength - 1) / 2) - 2, 28 * y + tiley * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9] = new Tile(18, true, 0, 0);
                                TileMap.tileMap[28 * x + ((room.sqrtTileArrayLength - 1) / 2) - 1, 28 * y + tiley * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9] = new Tile(6, false, 0, 0);
                                TileMap.tileMap[28 * x + (room.sqrtTileArrayLength - 1) / 2, 28 * y + tiley * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9] = new Tile(10, false, 0, 0);
                                TileMap.tileMap[28 * x + ((room.sqrtTileArrayLength - 1) / 2) + 1, 28 * y + tiley * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9] = new Tile(14, false, 0, 0);
                                TileMap.tileMap[28 * x + ((room.sqrtTileArrayLength - 1) / 2) + 2, 28 * y + tiley * bridgeSide + bridgeSide * ((room.sqrtTileArrayLength - 1) / 2) + 9] = new Tile(18, true, 0, 0);
                            }
                        }
                    }
                }
            }
            UpdateTileMap();
        }

        public static void UpdateTileMap()
        {

            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    SetTileMapToRoom(x, y);
                }
            }
            TileMap.SettleTileMap();
        }
        public static void SetTileMapToRoom(int roomX, int roomY)
        {
            Room room = rooms[roomX, roomY];
            for (var tileY = 0; tileY < room.sqrtTileArrayLength; tileY++)
            {
                for (var tileX = 0; tileX < room.sqrtTileArrayLength; tileX++)
                {
                    TileMap.tileMap[roomX * 28 + tileX, roomY * 28 + tileY] = room.tileArray[tileX, tileY];
                }
            }
        }
        public static Room PosToRoom(Vector2 position)
        {
            (int x, int y) tileMapPos = TileMap.PosToAbsTileMapPos(position);
            return rooms[tileMapPos.x / 28, tileMapPos.y / 28];
        }
        public static (int x, int y) PosToRoomIndex(Vector2 position)
        {
            (int x, int y) tileMapPos = TileMap.PosToAbsTileMapPos(position);
            return (tileMapPos.x / 28, tileMapPos.y / 28);
        }
        public static (int, int) PosToTileOfRoom(Vector2 position)
        {
            return (TileMap.PosToAbsTileMapPos(position).Item1 % 28, TileMap.PosToAbsTileMapPos(position).Item2 % 28);
        }
        public static void ChangeTileAtPos(Vector2 position, ushort startingIndex, bool isBarrier, byte breakEffect, byte tileObject)
        {
            (int tileX, int tileY) = PosToTileOfRoom(position);
            Room room = PosToRoom(position);
            if (tileX < room.sqrtTileArrayLength && tileY < room.sqrtTileArrayLength && tileX > -1 && tileY > -1)
                room.SetTile(tileX, tileY, startingIndex, isBarrier, breakEffect, tileObject);

            SetTileMapToRoom(PosToRoomIndex(position).x, PosToRoomIndex(position).y);
        }
        public static void Change3x3TilesAroundPos(Vector2 position, ushort startingIndex, bool isBarrier, byte breakEffect, byte tileObject)
        {
            (int tileX, int tileY) = PosToTileOfRoom(position);
            Room room = PosToRoom(position);
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (tileX + x < room.sqrtTileArrayLength && tileY + y < room.sqrtTileArrayLength && tileX + x > -1 && tileY + y > -1)
                        room.SetTile(tileX + x, tileY + y, startingIndex, isBarrier, breakEffect, tileObject);
                }
            }
            SetTileMapToRoom(PosToRoomIndex(position).x, PosToRoomIndex(position).y);
        }

        /// <summary>
        /// Takes player position and writes the room the player is in to the roomdata file
        /// </summary>
        public static void WriteRoomToFile(Player player)
        {
            Room roomToWrite = PosToRoom(player.position);

            SetTileMapToRoom(PosToRoomIndex(player.position).x, PosToRoomIndex(player.position).y);

            StreamWriter writer = File.AppendText("C:/Users/User/source/repos/Monogame Cross Platform/Content/RoomData.txt");
            writer.Write("\n");

            for (var tileY = 0; tileY < roomToWrite.sqrtTileArrayLength; tileY++)
            {
                for (var tileX = 0; tileX < roomToWrite.sqrtTileArrayLength; tileX++)
                {
                    Tile tile = roomToWrite.tileArray[tileX, tileY];
                    writer.Write($"{16 * (int)(tile.textureIndex / 16)},{tile.isBarrier},{tile.tileObject},{tile.breakEffect},");
                }
            }
            writer.Close();
        }

    }
}
