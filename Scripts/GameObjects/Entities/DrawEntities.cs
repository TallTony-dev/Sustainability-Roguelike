using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities
{
    /// <summary>
    /// Contains everything related to drawing all objects onto the scene.
    /// </summary>
    internal class DrawEntities
    {
        public SpriteBatch spriteBatch { get; }
        GraphicsDeviceManager graphics;
        public DrawEntities()
        {
            graphics = Game1._graphics;
            spriteBatch = new SpriteBatch(Game1._graphics.GraphicsDevice);
        }
        public DrawEntities(SpriteBatch spriteBatch) 
        {
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Adds something to the draw buffer which is drawn after DrawBuffer is called.
        /// </summary>
        public void AddToDrawBuffer(Vector2 position, Texture2D texture, Rectangle rectangle)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
        /// <summary>
        /// Adds something to the draw buffer which is drawn after DrawBuffer is called. This overload includes a sourceRectangle for spritesheets.
        /// </summary>
        public void AddToDrawBuffer(Entity entity, SpriteEffects spriteEffect)
        {
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTexture(entity.textureIndex);
            spriteBatch.Draw(texture, entity.position, rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0f);
        }

        public void DrawBuffer() => spriteBatch.End(); //Draws what is in the buffer
        public void BeginBuffer(ContentManagers.Camera.Camera camera) => spriteBatch.Begin(SpriteSortMode.BackToFront,null,SamplerState.PointClamp,transformMatrix: camera.Transform); //Begins the buffer with a maxtrix transform
        public void BeginBuffer() => spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp); //Begins the buffer without camera matrix
    }
}
