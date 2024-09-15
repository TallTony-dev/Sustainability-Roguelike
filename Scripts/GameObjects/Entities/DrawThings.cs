using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities
{
    /// <summary>
    /// Contains everything related to drawing all objects onto the scene.
    /// </summary>
    internal class DrawThings
    {
        public SpriteBatch spriteBatch { get; }
        public SpriteBatch uiSpriteBatch { get; }
        GraphicsDeviceManager graphics;
        public DrawThings()
        {
            graphics = Game1._graphics;
            spriteBatch = new SpriteBatch(Game1._graphics.GraphicsDevice);
            uiSpriteBatch = new SpriteBatch(Game1._graphics.GraphicsDevice);
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
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(entity.textureIndex);
            spriteBatch.Draw(texture, entity.position, rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0f);
        }
        public void AddToDrawBuffer(Tiles.Tile tile, int tileMapX, int tileMapY)
        {
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(tile.textureIndex);
            spriteBatch.Draw(texture, new Vector2(tileMapX * 32, tileMapY * 32), rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
        public void AddToUiBuffer(List<HUD.Menu> menus)
        {
            foreach (HUD.Menu menu in menus)
            {
                if (menu.isActive)
                {
                    foreach (HUD.UiElement element in menu.elements)
                    {
                        if (element.isEnabled)
                        {
                            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(element.textureIndex);
                            uiSpriteBatch.Draw(texture, new Vector2(element.xOffset, element.yOffset), rectangle, Color.White, 0f, new Vector2(0,0), new Vector2(Settings.uiScaleX, Settings.uiScaleY), SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }
        public void AddToDrawBuffer(List<GameObject> gameObjectList)
        {
            foreach (Entity entity in gameObjectList)
            {
                if (entity.isFlipped == false)
                    AddToDrawBuffer(entity, SpriteEffects.None);
                else
                    AddToDrawBuffer(entity, SpriteEffects.FlipHorizontally);
            }
        }

        public void BeginUiBuffer() => uiSpriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);
        public void DrawUiBuffer() => uiSpriteBatch.End();
        public void DrawBuffer() => spriteBatch.End(); //Draws what is in the buffer
        public void BeginBuffer(ContentManagers.Camera.Camera camera) => spriteBatch.Begin(SpriteSortMode.BackToFront,null,SamplerState.PointClamp,transformMatrix: camera.Transform); //Begins the buffer with a matrix transform
        public void BeginBuffer() => spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp); //Begins the buffer without camera matrix
    }
}
