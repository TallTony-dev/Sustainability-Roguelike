global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using Monogame_Cross_Platform.Scripts.ContentManagers;
global using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
global using System;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
global using Monogame_Cross_Platform.Scripts.HUD;
global using System.Collections.Generic;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
using Monogame_Cross_Platform.Scripts.Level;

namespace Monogame_Cross_Platform.Scripts
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;

        ContentLoader contentLoader;
        DrawThings drawEntities;
        RenderTarget2D renderTarget;
        ContentManagers.Camera.Camera camera = new ContentManagers.Camera.Camera();

        LevelEditor levelEditor;

        public static GameTime gameTime;

        SpriteFont font; //Temp font
        public static string debugText = "test";

         Player player = new Player(100, 100, new Vector2(100, 100),new Hitboxes.Hitbox(0,0,30,30), 0); //Put this in a better spot inside of an initialize level function within update or smth
         //Enemy testEnemy = new Enemy(100, 5, 100, new Vector2(10, 10), 3); //same with this one
         internal static List<Entity> currentEntities; //temp?
         internal static List<Menu> activeMenus = new List<Menu>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Settings.ApplySettings(); // TEMPTEMPTEMP
            Settings.InitializeSettings();

            _graphics.PreferredBackBufferWidth = Settings.resolutionWidth;
            _graphics.PreferredBackBufferHeight = Settings.resolutionHeight;
            _graphics.IsFullScreen = Settings.isFullScreen;



            currentEntities = new List<Entity>() { player }; //TEMP THIS SHOULD BE HANDLED ELSEWHERE

            levelEditor = new LevelEditor();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentLoader = new ContentLoader(this);

            LevelGenerator.GenerateLevel(1); //TEMP
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            // TODO: use this.Content to load your game content here
            drawEntities = new DrawThings();
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

            UpdateThings.UpdateLevel(levelEditor, player);
            UpdateThings.UpdateEntities(player);

            camera.Follow(player);
            Settings.UpdateZoom();

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
                    if (x>=0 && y>=0 && x<=512 && y<=512)
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
            drawEntities.spriteBatch.DrawString(font, debugText, new Vector2(400, 400), Color.Fuchsia); //draws debug text
            drawEntities.DrawBuffer();

            drawEntities.BeginUiBuffer();
            drawEntities.AddToUiBuffer(activeMenus); //draws menus
            drawEntities.DrawUiBuffer();

            base.Draw(gameTime);
        }
    }
}