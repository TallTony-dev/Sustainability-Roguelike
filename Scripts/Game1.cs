global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using Microsoft.Xna.Framework.Audio;
global using Monogame_Cross_Platform.Scripts.ContentManagers;
global using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;
global using Monogame_Cross_Platform.Scripts.GameObjects;
global using Monogame_Cross_Platform.Scripts.Particles;
global using System;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
global using Monogame_Cross_Platform.Scripts.HUD;
global using System.Collections.Generic;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
global using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Enemy;
global using Monogame_Cross_Platform.Scripts.GameObjects.Weapons;
using Monogame_Cross_Platform.Scripts.Level;
using System.Linq;

namespace Monogame_Cross_Platform.Scripts
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static Random rand = new Random();
        
        public static ContentLoader contentLoader;
        DrawThings drawEntities;
        public static RenderTarget2D renderTarget;
        internal static ContentManagers.Camera.Camera camera = new ContentManagers.Camera.Camera();
        public static AudioPlayer audioPlayer;
        public static LevelEditor levelEditor;

        public static GameTime gameTime;

        SpriteFont font; //Temp font
        public static string debugText = "test";

        internal static Player player;
        internal static List<GameObject> currentGameObjects;
        internal static List<Menu> menus = new List<Menu>();
        internal static List<Projectile> activePlayerProjectiles = new List<Projectile>();
        internal static List<Projectile> activeEnemyProjectiles = new List<Projectile>();
        internal static List<Particle> activeParticles = new List<Particle>();
        internal static List<ParticleEmitter> activeParticleEmitters = new List<ParticleEmitter>();


        private FrameCounter _frameCounter = new FrameCounter();
        private Effect effect;

        static bool toExit = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            currentGameObjects = new List<GameObject>(); //should be handled elsewhere maybe

            levelEditor = new LevelEditor();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentLoader = new ContentLoader(this);

            audioPlayer = new AudioPlayer(ContentLoader.audioLoaded);
            player = new Player(5000, 250, new Vector2(177, 177), new Hitboxes.Hitbox(0, 0, 31, 31), 0); //Put this in a better spot inside of an initialize level function within update or smth

            effect = Content.Load<Effect>("C:/Users/User/source/repos/Monogame Cross Platform/bin/Debug/net6.0/Content/Shader");

            IsFixedTimeStep = false; //TEMP

            //Settings.ApplySettingsToFile(); //TEMP
            Settings.InitializeSettings();
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            // TODO: use this.Content to load your game content here

            drawEntities = new DrawThings(effect);
            contentLoader.LoadTextures("AlwaysLoaded");
            contentLoader.LoadTextures("Audio");
            contentLoader.LoadTextures("OtherTextures");

            renderTarget = new RenderTarget2D(GraphicsDevice, Settings.resolutionWidth, Settings.resolutionHeight);
            font = Content.Load<SpriteFont>("Arial"); //Temp font


            effect.Parameters["AmbientColor"].SetValue(2f);
        }

        protected override void Update(GameTime _gameTime)
        {
            if (toExit)
                Exit();
            gameTime = _gameTime;
            // TODO: Add your update logic here
            audioPlayer.Update();
            camera.Update();
            if (IsActive)
            {
                StaticMouse.Update();

                UpdateThings.UpdateAlwaysUpdateThings(camera);
                GameState.Update(player);
            }

            effect.Parameters["lightPosition"].SetValue(Vector3.Transform(new Vector3(player.position, -1f), camera.Transform));
            effect.Parameters["lightColor"].SetValue(new Vector3(1.2f,1.2f,1f));

            //effect.Parameters["playerLightDirection"].SetValue(Vector3.Normalize(new Vector3((float)Math.Cos(player.inputHandler.GetShootingAngle(player.position)), (float)Math.Sin(player.inputHandler.GetShootingAngle(player.position)), 0)));

            debugText = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond); //TEMP


            Settings.UpdateZoom();

            base.Update(gameTime);
        }



        private static bool resetRenderTarget = false;
        protected override void Draw(GameTime gameTime)
        {

            _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds); //TEMP

            // Drawing everything to render target here
            if (resetRenderTarget)
            {
                Game1.renderTarget = new RenderTarget2D(GraphicsDevice, Game1._graphics.PreferredBackBufferWidth, Game1._graphics.PreferredBackBufferHeight);
                resetRenderTarget = false;
            }

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            drawEntities.BeginBuffer(camera);

            //Draws Tiles onto map rendering only the area visible to player
            if (GameState.isInGame)
            {
                for (int x = (int)(player.position.X - _graphics.PreferredBackBufferWidth / 2) / 32; x < (player.position.X + _graphics.PreferredBackBufferWidth / 2) / 32; x++)
                {
                    for (int y = (int)(player.position.Y - _graphics.PreferredBackBufferHeight / 2) / 32; y < (player.position.Y + _graphics.PreferredBackBufferHeight / 2) / 32; y++)
                    {
                        if (x >= 0 && y >= 0 && x <= 512 && y <= 512)
                            drawEntities.AddToDrawBuffer(TileMap.tileMap[x, y], x, y);
                    }
                }
                drawEntities.AddToDrawBuffer(currentGameObjects);
                drawEntities.AddToDrawBuffer(activeParticles);
            }

            effect.Parameters["AmbientColor"].SetValue(0.6f); //this passes to the shader when no vertex atleast


            if (camera.Transform != new Matrix())
                effect.Parameters["MatrixTransform"].SetValue(camera.Transform);

            camera.UpdateProjectionMatrix();
            effect.Parameters["ProjectionMatrix"].SetValue(camera.Projection);


            effect.CurrentTechnique.Passes[0].Apply();

            drawEntities.DrawBuffer();


            GraphicsDevice.SetRenderTarget(null);

            // Drawing the render target to the screen here
            drawEntities.BeginBuffer();



            drawEntities.spriteBatch.Draw(renderTarget, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            drawEntities.spriteBatch.DrawString(font, debugText, new Vector2(400, 400), Color.DarkBlue); //draws debug text
            drawEntities.DrawBuffer();

            drawEntities.BeginUiBuffer();
            drawEntities.AddToUiBuffer(menus); //draws menus
            drawEntities.DrawUiBuffer();


            base.Draw(gameTime);
        }
        public static void ExitGame()
        {
            toExit = true;
        }

        public static void ResetRenderTarget()
        {
            resetRenderTarget = true;
        }
    }


    public class FrameCounter
    {
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MaximumSamples = 100;

        private Queue<float> _sampleBuffer = new();

        public void Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MaximumSamples)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
        }
    }
}