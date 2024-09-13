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
        public static Room[,] rooms = new Room[9,9]; //must be square (odd preferred)
        /// <summary>
        /// Edits the static class tilemap to fit the current level selected by param levelNumber
        /// </summary>
        public static void GenerateLevel(byte levelType, int levelLength)
        {
            short sqrtRoomsLength = (short)Math.Sqrt(rooms.Length);
            (int index, ushort distance)[,] roomIndices = new (int, ushort)[sqrtRoomsLength,sqrtRoomsLength];
            (int x, int y) currentRoom = (4, 4);
            Random rand = new Random();

            roomIndices[currentRoom.x, currentRoom.y] = (1, 0);
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
                    roomIndices[currentRoom.x, currentRoom.y].distance = (ushort)(Math.Abs(4 - currentRoom.x) + Math.Abs(4 - currentRoom.y));
                    levelLength--;
                }
            }

            ushort bossRooms = 0;
            ushort treasureRooms = 0;
            ushort enemyRooms = 0;
            (ushort, ushort) exitRoom = (0,0);
            ushort maxDistance = 0;
            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    if (roomIndices[x, y].distance > maxDistance)
                        maxDistance = roomIndices[x, y].distance;
                }
            }

            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    (int index, ushort distance) = roomIndices[x, y];
                    if (distance > 0)
                    {
                        //settle the room indices here based off distance to the indices in document (1 is an entrance, 2 is an enemy room, 3 is a treasure room, 4 is a boss room, 5 is an exit)
                        ushort adjacentTiles = 0;
                        if (roomIndices[x + 1, y].index > 0)
                            adjacentTiles++;
                        if (roomIndices[x - 1, y].index > 0)
                            adjacentTiles++;
                        if (roomIndices[x, y + 1].index > 0)
                            adjacentTiles++;
                        if (roomIndices[x, y - 1].index > 0)
                            adjacentTiles++;

                        //If its max distance away and exit is uninited, makes an exit
                        if (distance == maxDistance && exitRoom == (0, 0))
                        {
                            roomIndices[x, y].index = 5;
                        }

                        //If not an exit, If theres an adjacent exit and no bossrooms, makes a boss room
                        else if (bossRooms == 0)
                        {
                            if (roomIndices[x + 1, y].index == 5)
                            {
                                roomIndices[x, y].index = 4;
                                bossRooms++;
                            }
                            if (roomIndices[x - 1, y].index == 5)
                            {
                                roomIndices[x, y].index = 4;
                                bossRooms++;
                            }
                            if (roomIndices[x, y + 1].index == 5)
                            {
                                roomIndices[x, y].index = 4;
                                bossRooms++;
                            }
                            if (roomIndices[x, y - 1].index == 5)
                            {
                                roomIndices[x, y].index = 4;
                                bossRooms++;
                            }
                        }

                        //If not an exit or boss room, If its a dead end and theres less than 2 treasure rooms, its a treasure room
                        else if (adjacentTiles == 3 && treasureRooms < 2)
                        {
                            roomIndices[x, y].index = 3;
                            treasureRooms++;
                        }

                        //If nothing else, make it an enemy room
                        else 
                            roomIndices[x, y].index = 2;
                        //entrance is default so it just wont be inited due to being distance 0
                    }
                }
            }

            if (levelType == 0) //Debug/make new room mode
            {
                for (var x = 0; x < sqrtRoomsLength; x++)
                {
                    for (var y = 0; y < sqrtRoomsLength; y++)
                    {
                        rooms[x, y] = new Room(0); //this is for testing purposes only, change to default room later or entirely delete level 0 as it is for debugging
                        for (var tileY = 0; tileY < 32; tileY++)
                        {
                            for (var tileX = 0; tileX < 32; tileX++)
                            {
                                TileMap.tileMap[x * 32 + tileX, y * 32 + tileY] = rooms[x, y].tileArray[tileX, tileY];
                            }
                        }
                        rooms[x, y].SettleTiles();
                    }
                }
            }

            if (levelType == 1)
            {
                for(var x = 0; x < sqrtRoomsLength; x++)
                {
                    for (var y = 0; y < sqrtRoomsLength; y++)
                    {
                        if (roomIndices[x, y].index == 0)
                            rooms[x, y] = new Room(0);
                        if (roomIndices[x, y].index == 1)
                            rooms[x, y] = new Room(1);
                        if (roomIndices[x, y].index == 2)
                            rooms[x, y] = new Room((ushort)rand.Next(2, 18));
                        if (roomIndices[x, y].index == 3)
                            rooms[x, y] = new Room((ushort)rand.Next(18, 20)); //min is inclusive, max is exclusive, who knows why
                        if (roomIndices[x, y].index == 4)
                            rooms[x, y] = new Room(20);
                        if (roomIndices[x, y].index == 5)
                            rooms[x, y] = new Room(21);


                        //Adds Rooms to tilemap TODO: make this have bridges between active rooms
                        for (var tileY = 0; tileY < 32; tileY++)
                        {
                            for (var tileX = 0; tileX < 32; tileX++)
                            {
                                TileMap.tileMap[x*32 + tileX, y*32 + tileY] = rooms[x, y].tileArray[tileX, tileY];
                            }
                        }
                        rooms[x, y].SettleTiles();
                    }
                }
            }


            //Set tilemap to the rooms tiles then updates tilemap
            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    for (var tileY = 0; tileY < 32; tileY++)
                    {
                        for (var tileX = 0; tileX < 32; tileX++)
                        {
                            TileMap.tileMap[x * 32 + tileX, y * 32 + tileY] = rooms[x, y].tileArray[tileX, tileY];
                        }
                    }
                }
            }
            UpdateTileMap();
        }

        public static void UpdateTileMap()
        {
            short sqrtRoomsLength = (short)Math.Sqrt(rooms.Length);
            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    for (var tileY = 0; tileY < 32; tileY++)
                    {
                        for (var tileX = 0; tileX < 32; tileX++)
                        {
                            TileMap.tileMap[x * 32 + tileX, y * 32 + tileY] = rooms[x, y].tileArray[tileX, tileY];
                        }
                    }
                    rooms[x, y].SettleTiles();
                }
            }
        }
        public static Room PosToRoom(Vector2 position)
        {
            (int x, int y) tileMapPos = TileMap.PosToAbsTileMapPos(position);
            return rooms[tileMapPos.x / 32, tileMapPos.y / 32];
        }
        public static (int, int) PosToTileOfRoom(Vector2 position)
        {
            return (TileMap.PosToAbsTileMapPos(position).Item1 % 32, TileMap.PosToAbsTileMapPos(position).Item2 % 32);
        }
        public static void ChangeTileAtPos(Vector2 position, ushort startingIndex, bool isBarrier, byte breakEffect, byte statusGiven)
        {
            (int tileX, int tileY) = PosToTileOfRoom(position);
            Room room = PosToRoom(position);
            if (tileX < room.sqrtTileArrayLength && tileY < room.sqrtTileArrayLength && tileX > -1 && tileY > -1)
                room.SetTile(tileX, tileY, startingIndex, isBarrier, breakEffect, statusGiven);
            UpdateTileMap();
        }
        public static void Change3x3TilesAroundPos(Vector2 position, ushort startingIndex, bool isBarrier, byte breakEffect, byte statusGiven)
        {
            (int tileX, int tileY) = PosToTileOfRoom(position);
            Room room = PosToRoom(position);
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (tileX + x < room.sqrtTileArrayLength && tileY + y < room.sqrtTileArrayLength && tileX + x > -1 && tileY + y > -1)
                        room.SetTile(tileX + x, tileY + y, startingIndex, isBarrier, breakEffect, statusGiven);
                }
            }
            UpdateTileMap();
        }
        /// <summary>
        /// Takes player position and writes the room the player is in to the roomdata file
        /// </summary>
        public static void WriteRoomToFile(Player player)
        {
            Room roomToWrite = PosToRoom(player.position);
            StreamWriter writer = File.AppendText("C:/Users/User/source/repos/Monogame Cross Platform/Content/RoomData.txt");
            writer.Write("\n");

            for (var tileY = 0; tileY < roomToWrite.sqrtTileArrayLength; tileY++)
            {
                for (var tileX = 0; tileX < roomToWrite.sqrtTileArrayLength; tileX++)
                {
                    Tile tile = roomToWrite.tileArray[tileX, tileY];
                    writer.Write($"{16 * (int)(tile.textureIndex / 16)},{tile.isBarrier},{tile.statusGiven},{tile.breakEffect},");
                }
            }
            writer.Close();
        }

    }
}
