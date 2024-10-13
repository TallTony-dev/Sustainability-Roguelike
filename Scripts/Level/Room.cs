
using System.IO;
using System.Linq;


namespace Monogame_Cross_Platform.Scripts.Level
{
    internal class Room
    {
        public Tile[,] tileArray;
        public List<GameObject> gameObjects = new List<GameObject>();
        public int sqrtTileArrayLength { get; private set; }
        private ushort levelType = 1;

        public bool isX0 = false;
        public bool isY0 = false;
        public bool isXAtArrayLimit = false;
        public bool isYAtArrayLimit = false;

        public ushort roomType = 0;

        public bool isOpen = false;
        public bool wasActive = false;

        public int roomArrayX = 0;
        public int roomArrayY = 0;

        public void SetTile(int x, int y, ushort startingTileTextureIndex, bool isBarrier, bool breakable, ushort decorationIndex)
        {
            tileArray[x,y].textureIndex = startingTileTextureIndex;
            tileArray[x,y].isBarrier = isBarrier;
            tileArray[x,y].breakable = breakable;
            tileArray[x, y].decorationIndex = decorationIndex;
        }
        public void SetTile(int x, int y, Tile tile)
        {
            tileArray[x, y] = tile;
        }
        public Room(ushort levelType, ushort roomIndex, int roomArrayX, int roomArrayY)
        {
            this.roomArrayX = roomArrayX;
            this.roomArrayY = roomArrayY;
            roomType = roomIndex;
            //empty room
            if (roomIndex == 0)
            {
                roomIndex = 0;
            }
            //entrance
            else if (roomIndex == 1)
            {
                roomIndex = 1;
            }
            //enemy
            else if (roomIndex == 2)
            {
                roomIndex = (ushort)Game1.rand.Next(2, 13); //min is inclusive, max is exclusive, who knows why
            }
            //other
            else if (roomIndex == 3)
            {
                roomIndex = (ushort)Game1.rand.Next(13, 18);
            }
            //treasure
            else if (roomIndex == 4)
            {
                roomIndex = (ushort)Game1.rand.Next(18, 20);
            }
            //boss
            else if (roomIndex == 5)
            {
                roomIndex = 20;
            }
            //exit
            else if (roomIndex == 6)
            {
                roomIndex = 21;
            }

            this.levelType = levelType;
            tileArray = new Tile[19, 19]; //must be square and odd
            sqrtTileArrayLength = (int)Math.Sqrt(tileArray.Length);
            string roomData = "error";
            if (levelType > 0)
                roomData = File.ReadLines("Content/RoomData.txt").Skip(roomIndex * 2 + (levelType - 1) * 46 + 2).Take(1).First(); //this skips the number indicated in roomindex * 2 so that comments can be added between lines in roomdata.txt
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
                    tileArray[x, y].textureIndex = Convert.ToUInt16(tokens[1 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].isBarrier = Convert.ToBoolean(tokens[2 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].breakable = Convert.ToBoolean(tokens[3 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                    tileArray[x, y].decorationIndex = Convert.ToUInt16(tokens[4 + (x * 4) + (y * sqrtTileArrayLength * 4)]);
                }
            }

            //TODO: gameobjects are here
            tokens[0] = tokens[0].Trim();
            string[] gameObjects = tokens[0].Split("*");

            foreach (string gameObject in gameObjects)
            {
                if (gameObject != "")
                {
                    string[] gameObjectData = gameObject.Split("-");
                    string gameObjectType = gameObjectData[0];

                    (int tileMapTileX, int tileMapTileY) = RoomTileToTileMap(Convert.ToInt32(gameObjectData[1]), Convert.ToInt32(gameObjectData[2]));


                    if (gameObjectType == "Enemy")
                    {
                        Vector2 position = new Vector2(TileMap.TileMapPosToPos(tileMapTileX, tileMapTileY).X, TileMap.TileMapPosToPos(tileMapTileX, tileMapTileY).Y);
                        this.gameObjects.Add(new Enemy(Convert.ToUInt16(gameObjectData[3]), position));
                    }
                    if (gameObjectType == "Exit")
                    {
                        Vector2 position = new Vector2(tileMapTileX, tileMapTileY);
                        this.gameObjects.Add(new GameObjects.Objects.Exit(Convert.ToUInt16(gameObjectData[3]), position, new Vector2(64,64)));
                    }

                }
            }


            foreach (GameObject gameObject in this.gameObjects)
            {
                Game1.currentGameObjects.Add(gameObject);
            }

        }
        public void Update(Player player)
        {
            int enemies = 0; 
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject is Enemy)
                    enemies++;
            }
            if (isOpen)
            {
                if (LevelGenerator.PosToRoom(player.position) == this && LevelGenerator.PosToTileOfRoom(player.position).Item1 < 17 && LevelGenerator.PosToTileOfRoom(player.position).Item2 < 17
                    && LevelGenerator.PosToTileOfRoom(player.position).Item1 > 1 && LevelGenerator.PosToTileOfRoom(player.position).Item2 > 1
                    && !player.isInLevelEditorMode && enemies > 0)
                {
                    Activate();
                    EnableObjects();
                }
            }
            else
            {
                if (LevelGenerator.PosToRoom(player.position) != this || LevelGenerator.PosToTileOfRoom(player.position).Item1 > 18 || LevelGenerator.PosToTileOfRoom(player.position).Item2 > 18 || player.isInLevelEditorMode || enemies == 0)
                {
                    Deactivate();
                    DisableEntities();
                }
            }
        }

        public void EnableObjects()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.isEnabled = true;
            }
        }

        public void DisableEntities()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject is Entity)
                {
                    gameObject.isEnabled = false;
                }
            }
        }

        public void Activate()
        {
            CloseSides();
            LevelGenerator.SetTileMapToRoom(roomArrayX, roomArrayY);
            wasActive = true;
        }

        public void Deactivate()
        {
            OpenSides();
            LevelGenerator.SetTileMapToRoom(roomArrayX, roomArrayY);
        }

        public (int x, int y) TileMapToRoomTile(int tileX, int tileY)
        {
            return (tileX % (roomArrayX * 28), tileY % (roomArrayY * 28));
        }
        public (int x, int y) RoomTileToTileMap(int roomTileX, int roomTileY)
        {
            return (roomArrayX * 28 + roomTileX, roomArrayY * 28 + roomTileY);
        }

        public void OpenSides()
        {
            Tile openTile = new Tile();
            if (levelType == 1)
                openTile = new Tile(0, false, false, 0);
            else if (levelType == 2)
                openTile = new Tile(0, false, false, 0);

            if (!isXAtArrayLimit && LevelGenerator.rooms[roomArrayX + 1, roomArrayY].roomType != 0)
            {
                for (var y = 0; y < 3; y++)
                    SetTile(sqrtTileArrayLength - 1, (sqrtTileArrayLength - 1) / 2 + y - 1, openTile);
            }
            if (!isX0 && LevelGenerator.rooms[roomArrayX - 1, roomArrayY].roomType != 0)
            {
                for (var y = 0; y < 3; y++)
                    SetTile(0, (sqrtTileArrayLength - 1) / 2 + y - 1, openTile);
            }
            if (!isY0 && LevelGenerator.rooms[roomArrayX, roomArrayY - 1].roomType != 0)
            {
                for (var x = 0; x < 3; x++)
                    SetTile((sqrtTileArrayLength - 1) / 2 + x - 1, 0, openTile);
            }
            if (!isYAtArrayLimit && LevelGenerator.rooms[roomArrayX, roomArrayY + 1].roomType != 0)
            {
                for (var x = 0; x < 3; x++)
                    SetTile((sqrtTileArrayLength - 1) / 2 + x - 1, sqrtTileArrayLength - 1, openTile);
            }
            isOpen = true;
        }
        public void CloseSides()
        {
            Tile closedTile = new Tile();
            if (levelType == 1)
                closedTile = new Tile(30, true, false, 0);
            else if (levelType == 2)
                closedTile = new Tile(30, true, false, 0);

                for (var y = 0; y < 3; y++)
                    SetTile(sqrtTileArrayLength - 1, (sqrtTileArrayLength - 1) / 2 + y - 1, closedTile);
                for (var y = 0; y < 3; y++)
                    SetTile(0, (sqrtTileArrayLength - 1) / 2 + y - 1, closedTile);
                for (var x = 0; x < 3; x++)
                    SetTile((sqrtTileArrayLength - 1) / 2 + x - 1, 0, closedTile);
                for (var x = 0; x < 3; x++)
                    SetTile((sqrtTileArrayLength - 1) / 2 + x - 1, sqrtTileArrayLength - 1, closedTile);
            isOpen = false;
        }

        
        enum adjTiles { none, right, left, rightleft, bottom, bottomright, bottomleft, bottomrightleft, top, topright, topleft, toprightleft, topbottom, topbottomright, topbottomleft, topbottomrightleft }
    }
}
