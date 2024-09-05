using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
using System;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Enemy;
using System.Collections.Generic;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;

namespace Monogame_Cross_Platform.Scripts
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;

        ContentLoader contentLoader;
        DrawEntities drawEntities;
        RenderTarget2D renderTarget;
        ContentManagers.Camera.Camera camera = new ContentManagers.Camera.Camera();
        Settings settings;

        public static GameTime gameTime;

        SpriteFont font; //Temp font
        public static string debugText = "test";

         Player player = new Player(100, 20, new Vector2(100, 100),new Rectangle(0,0,32,32), 0); //Put this in a better spot inside of an initialize level function within update or smth
         //Enemy testEnemy = new Enemy(100, 5, 100, new Vector2(10, 10), 3); //same with this one
         List<Entity> currentEntities; //temp?

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Settings.resolutionWidth;
            _graphics.PreferredBackBufferHeight = Settings.resolutionHeight;
            _graphics.IsFullScreen = true;

            currentEntities = new List<Entity>() { player }; //TEMP THIS SHOULD BE HANDLED ELSEWHERE
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentLoader = new ContentLoader(this);
            settings = new Settings();

            //TileMap.tileMap[4, 5] = new Tile(2, true, 0, 0); // MAKES A TESTING TILE
            TileMap.tileMap[5, 5] = new Tile(2, true, 0, 0); // MAKES A TESTING TILE
            base.Initialize();
            
        }

        protected override void LoadContent() 
        {
            // TODO: use this.Content to load your game content here
            drawEntities = new DrawEntities();
            contentLoader.LoadTextures("AlwaysLoaded");

            renderTarget = new RenderTarget2D(GraphicsDevice, Settings.resolutionWidth, Settings.resolutionHeight);
            font = Content.Load<SpriteFont>("Arial"); //Temp font

        }

        protected override void Update(GameTime _gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            gameTime = _gameTime;
            // TODO: Add your update logic here

            UpdateThings.UpdateEntities(currentEntities, player);
            camera.Follow(player);
            settings.UpdateZoom();

            (int absTileX, int absTileY) = TileMap.PosToAbsTileMapPos(player.position); //temp
            debugText = TileMap.PosToAbsTileMapPos(player.position).ToString() + player.position.ToString() + TileMap.GetTileBounds(absTileX + 1, absTileY).Left.ToString(); //debug text that displays values to test

            base.Update(gameTime);
        }
           
        protected override void Draw(GameTime gameTime)
        {
            // Drawing everything to render target here
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.AliceBlue);
            drawEntities.BeginBuffer(camera);

            //Draws Tiles onto map rendering only the area visible to player
            for (int x = (int)(player.position.X - _graphics.PreferredBackBufferWidth) / 32; x < (player.position.X + _graphics.PreferredBackBufferWidth) / 32; x++)
            {
                for (int y = (int)(player.position.Y - _graphics.PreferredBackBufferHeight) / 32; y < (player.position.Y + _graphics.PreferredBackBufferHeight) / 32; y++)
                {
                    if (x>0 && y>0 && x<4095 && y<4095)
                    drawEntities.AddToDrawBuffer(TileMap.tileMap[x, y], x, y);
                }
            }
            //Draws entities active in the currentEntities list
            drawEntities.AddToDrawBuffer(currentEntities);

            drawEntities.DrawBuffer();
            GraphicsDevice.SetRenderTarget(null);

            // Drawing the render target to the screen here
            drawEntities.BeginBuffer();
            drawEntities.spriteBatch.Draw(renderTarget, new Rectangle(0,0,Settings.resolutionWidth,Settings.resolutionHeight), Color.White);
            drawEntities.spriteBatch.DrawString(font, debugText, new Vector2(100, 100), Color.DarkGreen); //draws debug text
            drawEntities.DrawBuffer();

            base.Draw(gameTime);
        }
    }
}