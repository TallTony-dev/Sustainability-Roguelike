global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
using System;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.HUD;
using System.Collections.Generic;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;

namespace Monogame_Cross_Platform.Scripts
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;

        ContentLoader contentLoader;
        DrawThings drawEntities;
        RenderTarget2D renderTarget;
        ContentManagers.Camera.Camera camera = new ContentManagers.Camera.Camera();
        ContentManagers.Camera.Camera uiCamera = new ContentManagers.Camera.Camera();
        Settings settings;

        LevelEditor levelEditor;

        public static GameTime gameTime;

        SpriteFont font; //Temp font
        public static string debugText = "test";

         Player player = new Player(100, 100, new Vector2(100, 100),new Hitboxes.Hitbox(0,0,30,30), 0); //Put this in a better spot inside of an initialize level function within update or smth
         //Enemy testEnemy = new Enemy(100, 5, 100, new Vector2(10, 10), 3); //same with this one
         List<Entity> currentEntities; //temp?
         List<Menu> activeMenus;

        int buttonIncrement = 1; //TEMP

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Settings.resolutionWidth;
            _graphics.PreferredBackBufferHeight = Settings.resolutionHeight;
            _graphics.IsFullScreen = true;

            currentEntities = new List<Entity>() { player }; //TEMP THIS SHOULD BE HANDLED ELSEWHERE
            activeMenus = new List<Menu>() { new Menu(new List<UiElement>() { new Button(1, new Rectangle(0,0,128,128), 0, 0)})}; //this too

            levelEditor = new LevelEditor();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentLoader = new ContentLoader(this);
            settings = new Settings();

            Level.LevelGenerator.GenerateLevel(1); //TEMP

            base.Initialize();
            settings.ApplySettings();
            settings.InitializeSettings();
            uiCamera.Scale(2);
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

            UpdateThings.UpdateLevel(levelEditor, player, currentEntities, activeMenus);
            UpdateThings.UpdateEntities(currentEntities, player);
            camera.Follow(player);
            settings.UpdateZoom();

            if (activeMenus[0].Update())
            {
                buttonIncrement++; //TODO: FIX BUTTONS
            }
            
            debugText = buttonIncrement.ToString() + "test"; //debug text that displays values to test

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

            drawEntities.BeginUiBuffer(uiCamera);
            drawEntities.AddToUiBuffer(activeMenus); //draws menus
            drawEntities.DrawUiBuffer();

            base.Draw(gameTime);
        }
    }
}