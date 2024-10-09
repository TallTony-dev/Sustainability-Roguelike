using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class MiniMap : UiElement
    {
        public MiniMap(ushort backgroundTextureIndex, int xOffset, int yOffset, Rectangle hitBox) : base(backgroundTextureIndex, xOffset, yOffset, hitBox)
        {
            scale = 0.7f;
            mapIconScale = 0.4f;
        }
        public ushort entranceTextureIndex = 32;
        public ushort enemyRoomTextureIndex = 33;
        public ushort otherRoomTextureIndex = 34;
        public ushort treasureRoomTextureIndex = 35;
        public ushort bossRoomTextureIndex = 36;
        public ushort exitTextureIndex = 37;
        public ushort bridgeTextureIndex = 16;

        private float mapIconScale;
        private int borderDistance = 8;

        int maxInitedX;
        int minInitedX;
        int maxInitedY;
        int minInitedY;

        public ushort[,] knownRoomTypes = new ushort[(int)Math.Sqrt(Level.LevelGenerator.rooms.Length), (int)Math.Sqrt(Level.LevelGenerator.rooms.Length)];

        public ushort squareSize = 2;

        public void DrawMiniMap(SpriteBatch uiSpriteBatch)
        {
            (Texture2D backTexture, Rectangle backRectangle) = ContentLoader.GetLoadedOtherTexture(textureIndex);
            (Texture2D entranceTexture, Rectangle entranceRectangle) = ContentLoader.GetLoadedTileTexture(entranceTextureIndex);
            (Texture2D enemyRoomTexture, Rectangle enemyRoomRectangle) = ContentLoader.GetLoadedTileTexture(enemyRoomTextureIndex);
            (Texture2D otherRoomTexture, Rectangle otherRoomRectangle) = ContentLoader.GetLoadedTileTexture(otherRoomTextureIndex);
            (Texture2D treasureRoomTexture, Rectangle treasureRoomRectangle) = ContentLoader.GetLoadedTileTexture(treasureRoomTextureIndex);
            (Texture2D bossRoomTexture, Rectangle bossRoomRectangle) = ContentLoader.GetLoadedTileTexture(bossRoomTextureIndex);
            (Texture2D exitTexture, Rectangle exitRectangle) = ContentLoader.GetLoadedTileTexture(exitTextureIndex);
            (Texture2D bridgeTexture, Rectangle bridgeRectangle) = ContentLoader.GetLoadedTileTexture(bridgeTextureIndex);

            backRectangle = new Rectangle(0,0, backTexture.Width, backTexture.Height);
            //draw background
            uiSpriteBatch.Draw(backTexture, new Vector2(xOffset, yOffset), null, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * scale, Settings.uiScaleY * scale), SpriteEffects.None, 0.1f);

            float deltaX = maxInitedX - minInitedX;
            float deltaY = maxInitedY - minInitedY;
            int sqrtRoomsLength = (int)Math.Sqrt(knownRoomTypes.Length);
            float usableWidthOverBackground = backRectangle.Width * scale * Settings.uiScaleX - borderDistance * 2;
            float usableHeightOverBackground = backRectangle.Height * scale * Settings.uiScaleY - borderDistance * 2;

            for (var x = 0; x <= deltaX; x++)
            {
                for (var y = 0; y <= deltaY; y++)
                {
                    if (!(x + minInitedX > sqrtRoomsLength - 1 || y + minInitedY > sqrtRoomsLength - 1))
                    {
                        ushort roomType = knownRoomTypes[x + minInitedX, y + minInitedY];

                        if (roomType != 0)
                        {
                            Vector2 placement = new Vector2((xOffset + usableWidthOverBackground/2 + borderDistance + ((usableWidthOverBackground) / (squareSize)) * (x - deltaX / 2)), 
                                                            (yOffset + usableHeightOverBackground/2 + borderDistance + ((usableHeightOverBackground) / (squareSize)) * (y - deltaY / 2)));



                            if (roomType == 1)
                                uiSpriteBatch.Draw(entranceTexture, placement, entranceRectangle, Color.White, 0f, new Vector2(entranceRectangle.Width / 2, entranceRectangle.Height / 2), new Vector2(Settings.uiScaleX * mapIconScale, Settings.uiScaleY * mapIconScale), SpriteEffects.None, 0f);
                            if (roomType == 2)
                                uiSpriteBatch.Draw(enemyRoomTexture, placement, enemyRoomRectangle, Color.White, 0f, new Vector2(enemyRoomRectangle.Width / 2, enemyRoomRectangle.Height / 2), new Vector2(Settings.uiScaleX * mapIconScale, Settings.uiScaleY * mapIconScale), SpriteEffects.None, 0f);
                            if (roomType == 3)
                                uiSpriteBatch.Draw(otherRoomTexture, placement, otherRoomRectangle, Color.White, 0f, new Vector2(otherRoomRectangle.Width / 2, otherRoomRectangle.Height / 2), new Vector2(Settings.uiScaleX * mapIconScale, Settings.uiScaleY * mapIconScale), SpriteEffects.None, 0f);
                            if (roomType == 4)
                                uiSpriteBatch.Draw(treasureRoomTexture, placement, treasureRoomRectangle, Color.White, 0f, new Vector2(treasureRoomRectangle.Width / 2, treasureRoomRectangle.Height / 2), new Vector2(Settings.uiScaleX * mapIconScale, Settings.uiScaleY * mapIconScale), SpriteEffects.None, 0f);
                            if (roomType == 5)
                                uiSpriteBatch.Draw(bossRoomTexture, placement, bossRoomRectangle, Color.White, 0f, new Vector2(bossRoomRectangle.Width / 2, bossRoomRectangle.Height / 2), new Vector2(Settings.uiScaleX * mapIconScale, Settings.uiScaleY * mapIconScale), SpriteEffects.None, 0f);
                            if (roomType == 6)
                                uiSpriteBatch.Draw(exitTexture, placement, exitRectangle, Color.White, 0f, new Vector2(exitRectangle.Width / 2, exitRectangle.Height / 2), new Vector2(Settings.uiScaleX * mapIconScale, Settings.uiScaleY * mapIconScale), SpriteEffects.None, 0f);
                            //draw bridges here if theres adjacent tiles?? maybe not needed
                        }
                    }
                }
            }
        }
        public void UpdateMiniMap(Player player)
        {
            Room currentRoom = LevelGenerator.PosToRoom(player.position);
            if (currentRoom.wasActive)
            {
                for (var x = -1; x < 2; x++)
                {
                    for (var y = -1; y < 2; y++)
                    {
                        if (!(y != 0 && x != 0) && !(currentRoom.roomArrayX + x < 0 || currentRoom.roomArrayX + x > Math.Sqrt(LevelGenerator.rooms.Length) - 1 || currentRoom.roomArrayY + y < 0 || currentRoom.roomArrayY + y > Math.Sqrt(LevelGenerator.rooms.Length) - 1))
                        {
                            knownRoomTypes[currentRoom.roomArrayX + x, currentRoom.roomArrayY + y] = LevelGenerator.rooms[currentRoom.roomArrayX + x, currentRoom.roomArrayY + y].roomType;
                        }
                    }
                }
            }
            CalculateSquareSize();
            Update();
        }
        public void CalculateSquareSize()
        {
            int sqrtRoomsLength = (int)Math.Sqrt(LevelGenerator.rooms.Length);
            maxInitedX = sqrtRoomsLength / 2;
            minInitedX = sqrtRoomsLength / 2;
            maxInitedY = sqrtRoomsLength / 2;
            minInitedY = sqrtRoomsLength / 2;

            for (var x = 0; x < sqrtRoomsLength; x++)
            {
                for (var y = 0; y < sqrtRoomsLength; y++)
                {
                    if (knownRoomTypes[x,y] != 0)
                    {
                        if (x > maxInitedX)
                            maxInitedX = x;
                        if (x < minInitedX)
                            minInitedX = x;
                        if (y > maxInitedY)
                            maxInitedY = y;
                        if (y < minInitedY)
                            minInitedY = y;
                    }
                }
            }
            squareSize = (ushort)(Math.Max(maxInitedX - minInitedX, maxInitedY - minInitedY) + 1);
            if (squareSize % 2 == 0) //if its even then add 1
            {
                squareSize += 1;
            }
        }
    }
}
