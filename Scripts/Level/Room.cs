using System.IO;
using System.Linq;


namespace Monogame_Cross_Platform.Scripts.Level
{
    internal class Room
    {
        public Tile[,] tileArray;
        public int sqrtTileArrayLength { get; private set; }
        private ushort levelType = 0;

        public bool isX0 = false;
        public bool isY0 = false;
        public bool isXAtArrayLimit = false;
        public bool isYAtArrayLimit = false;

        public bool isARoom = false;

        public bool isOpen = false;

        public int roomArrayX = 0;
        public int roomArrayY = 0;

        public void SetTile(int x, int y, ushort startingTileTextureIndex, bool isBarrier, byte breakEffect, byte tileObject)
        {
            tileArray[x,y].textureIndex = startingTileTextureIndex;
            tileArray[x,y].tileObject = tileObject;
            tileArray[x,y].isBarrier = isBarrier;
            tileArray[x,y].breakEffect = breakEffect;
        }
        public Room(ushort levelType, ushort roomIndex, Random rand, int roomArrayX, int roomArrayY)
        {
            
            //empty room
            if (roomIndex == 0)
            {
                roomIndex = 0;
            }
            //entrance
            else if (roomIndex == 1)
            {
                roomIndex = 1;
                isARoom = true;
            }
            //enemy
            else if (roomIndex == 2)
            {
                roomIndex = (ushort)rand.Next(2, 13); //min is inclusive, max is exclusive, who knows why
                isARoom = true;
            }
            //other
            else if (roomIndex == 3)
            {
                roomIndex = (ushort)rand.Next(13, 18);
                isARoom = true;
            }
            //treasure
            else if (roomIndex == 4)
            {
                roomIndex = (ushort)rand.Next(18, 20);
                isARoom = true;
            }
            //boss
            else if (roomIndex == 5)
            {
                roomIndex = 20;
                isARoom = true;
            }
            //exit
            else if (roomIndex == 6)
            {
                roomIndex = 21;
                isARoom = true;
            }

            this.levelType = levelType;
            tileArray = new Tile[19, 19]; //must be square and odd
            sqrtTileArrayLength = (int)Math.Sqrt(tileArray.Length);
            string roomData = "error";
            if (levelType > 0)
                roomData = File.ReadLines("Content/RoomData.txt").Skip(roomIndex * 2 * levelType).Take(1).First(); //this skips the number indicated in roomindex * 2 so that comments can be added between lines in roomdata.txt
            else
            {
                if (roomIndex == 0)
                    roomData = File.ReadLines("Content/RoomData.txt").Skip(0).Take(1).First(); //ignores leveltype for testing purposes
                else
                    roomData = File.ReadLines("Content/RoomData.txt").Skip(2).Take(1).First(); //ignores leveltype for testing purposes
            }


            string[] tokens = roomData.Split(",");
            for (int y = 0; y < sqrtTileArrayLength; y++)
            {
                for (int x = 0; x < sqrtTileArrayLength; x++)
                {
                    tileArray[x, y].textureIndex = Convert.ToUInt16(tokens[(x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].isBarrier = Convert.ToBoolean(tokens[1 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].tileObject = Convert.ToByte(tokens[2 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].breakEffect = Convert.ToByte(tokens[3 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                }
            }

            this.roomArrayX = roomArrayX;
            this.roomArrayY = roomArrayY;
        }
        public (int x, int y) RoomTileToTileMap(int roomTileX, int roomTileY)
        {
            return (roomArrayX * 28 + roomTileX, roomArrayY * 28 + roomTileY);
        }
        public void OpenSides()
        {
            if (!isXAtArrayLimit && LevelGenerator.rooms[roomArrayX + 1, roomArrayY].isARoom)
            {
                if(levelType == 1)
                {
                    for (var y = 0; y < 3; y++)
                        SetTile(sqrtTileArrayLength - 1, (sqrtTileArrayLength - 1) / 2 + y - 1, 0, false, 0, 0);
                }
            }
            if (!isX0 && LevelGenerator.rooms[roomArrayX - 1, roomArrayY].isARoom)
            {
                if (levelType == 1)
                {
                    for (var y = 0; y < 3; y++)
                        SetTile(0, (sqrtTileArrayLength - 1) / 2 + y - 1, 0, false, 0, 0);
                }
            }
            if (!isY0 && LevelGenerator.rooms[roomArrayX, roomArrayY - 1].isARoom)
            {
                if (levelType == 1)
                {
                    for (var x = 0; x < 3; x++)
                        SetTile((sqrtTileArrayLength - 1) / 2 + x - 1, 0, 0, false, 0, 0);
                }
            }
            if (!isYAtArrayLimit && LevelGenerator.rooms[roomArrayX, roomArrayY + 1].isARoom)
            {
                if (levelType == 1)
                {
                    for (var x = 0; x < 3; x++)
                        SetTile((sqrtTileArrayLength - 1) / 2 + x - 1, sqrtTileArrayLength - 1, 0, false, 0, 0);
                }
            }
            isOpen = true;
        }
        public void CloseSides()
        {
            if (levelType == 1)
            {
                for (var y = 0; y < 3; y++)
                    tileArray[sqrtTileArrayLength - 1, (sqrtTileArrayLength - 1) / 2 + y - 1] = new Tile(16, true, 0, 0);
                for (var y = 0; y < 3; y++)
                    tileArray[0, (sqrtTileArrayLength - 1) / 2 + y - 1] = new Tile(16, true, 0, 0);
                for (var x = 0; x < 3; x++)
                    tileArray[(sqrtTileArrayLength - 1) / 2 + x - 1, 0] = new Tile(16, true, 0, 0);
                for (var x = 0; x < 3; x++)
                    tileArray[(sqrtTileArrayLength - 1) / 2 + x - 1, sqrtTileArrayLength - 1] = new Tile(16, true, 0, 0);
            }
            SettleTiles();
            LevelGenerator.SetTileMapToRoom(roomArrayX, roomArrayY);
            isOpen = false;
        }

        public void SettleTiles()
        {
            for (var y = 0; y < sqrtTileArrayLength; y++)
            {
                for (var x = 0; x < sqrtTileArrayLength; x++)
                { 
                    short enumToPick = 0;
                    Tile tile = tileArray[x, y];
                    ushort tileIndex = (ushort)(tile.textureIndex - (tile.textureIndex % 16));

                    if (x + 1 < sqrtTileArrayLength && tileArray[x + 1, y].textureIndex >= tileIndex && tileArray[x + 1, y].textureIndex < tileIndex + 16) //checking right
                    {
                        enumToPick += 1;
                    }
                    if (x - 1 >= 0 && tileArray[x - 1, y].textureIndex >= tileIndex && tileArray[x - 1, y].textureIndex < tileIndex + 16) //checking left
                    {
                        enumToPick += 2;
                    }
                    if (y + 1 < sqrtTileArrayLength && tileArray[x, y + 1].textureIndex >= tileIndex && tileArray[x, y + 1].textureIndex < tileIndex + 16)//checking bottom
                    {
                        enumToPick += 4;
                    }
                    if (y - 1 >= 0 && tileArray[x, y - 1].textureIndex >= tileIndex && tileArray[x, y - 1].textureIndex < tileIndex + 16)//checking top
                    {
                        enumToPick += 8;
                    }

                    tileIndex += ((adjTiles)enumToPick) switch
                    {
                        adjTiles.none => 0,
                        adjTiles.right => 1,
                        adjTiles.rightleft => 2,
                        adjTiles.left => 3,
                        adjTiles.bottom => 4,
                        adjTiles.bottomright => 5,
                        adjTiles.bottomrightleft => 6,
                        adjTiles.bottomleft => 7,
                        adjTiles.topbottom => 8,
                        adjTiles.topbottomright => 9,
                        adjTiles.topbottomrightleft => 10,
                        adjTiles.topbottomleft => 11,
                        adjTiles.top => 12,
                        adjTiles.topright => 13,
                        adjTiles.toprightleft => 14,
                        adjTiles.topleft => 15,
                        _ => 0
                    };

                    tileArray[x, y].textureIndex = tileIndex;
                }
            }
        }
        enum adjTiles { none, right, left, rightleft, bottom, bottomright, bottomleft, bottomrightleft, top, topright, topleft, toprightleft, topbottom, topbottomright, topbottomleft, topbottomrightleft }
    }
}
