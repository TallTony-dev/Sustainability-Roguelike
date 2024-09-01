using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.Entities.Player;
using System;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Enemy;
using System.Collections.Generic;

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

        Player player = new Player(100, 100, new Vector2(10, 10), true, 0); //Put this in a better spot inside of an initialize level function within update or smth
        Enemy testEnemy = new Enemy(100, 5, 100, new Vector2(10, 10), true, 3); //same with this one

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Settings.resolutionWidth;
            _graphics.PreferredBackBufferHeight = Settings.resolutionHeight;
            _graphics.IsFullScreen = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentLoader = new ContentLoader(this);
            settings = new Settings();
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

            List<Entity> currentEntities = new List<Entity>() { player }; //temp
            UpdateThings.UpdateEntities(currentEntities);
            camera.Follow(player);
            settings.UpdateZoom();


            debugText = GameObjects.Tiles.TileMap.PosToTileMapPos(player.position).ToString();

            base.Update(gameTime);
        }
           
        protected override void Draw(GameTime gameTime)
        {
            // Drawing everything to render target here
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.AliceBlue);
            drawEntities.BeginBuffer(camera);

            if (testEnemy.isFlipped == false)
                drawEntities.AddToDrawBuffer(testEnemy, SpriteEffects.None);
            else
                drawEntities.AddToDrawBuffer(testEnemy, SpriteEffects.FlipHorizontally);

            if (player.isFlipped == false)
                drawEntities.AddToDrawBuffer(player, SpriteEffects.None);
            else
                drawEntities.AddToDrawBuffer(player, SpriteEffects.FlipHorizontally);
            int i = 0;
            foreach ((Texture2D texture, Rectangle rectangle) in ContentLoader.texturesLoaded)
            {
                drawEntities.AddToDrawBuffer(new Vector2(0 + (i * 32), 0), texture, rectangle);
                i++;
            }

            drawEntities.DrawBuffer();
            GraphicsDevice.SetRenderTarget(null);

            // Drawing the render target to the screen here
            drawEntities.BeginBuffer();
            drawEntities.spriteBatch.Draw(renderTarget, new Rectangle(0,0,Settings.resolutionWidth,Settings.resolutionHeight), Color.White);
            drawEntities.spriteBatch.DrawString(font, debugText, new Vector2(100, 100), Color.DarkGreen);
            drawEntities.DrawBuffer();

            base.Draw(gameTime);
        }
    }
}