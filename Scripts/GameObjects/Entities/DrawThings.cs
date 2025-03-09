using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Monogame_Cross_Platform.Scripts.ContentManagers;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;
using Monogame_Cross_Platform.Scripts.GameObjects.Tiles;

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

        Effect effect;
        public DrawThings(Effect effect)
        {
            graphics = Game1._graphics;
            spriteBatch = new SpriteBatch(Game1._graphics.GraphicsDevice);
            uiSpriteBatch = new SpriteBatch(Game1._graphics.GraphicsDevice);
            this.effect = effect;
        }
        /// <summary>
        /// Adds something to the draw buffer which is drawn after DrawBuffer is called. This overload includes a sourceRectangle for spritesheets.
        /// </summary>
        public void AddToDrawBuffer(Entity entity, SpriteEffects spriteEffect)
        {
            (Texture2D texture, Rectangle rectangle, Texture2D normalMap /*TODO IMPLEMENT THIS*/) = ContentLoader.GetLoadedTileTexture(entity.textureIndex);
            spriteBatch.Draw(texture, entity.position, rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0.02f - 0.00000001f * entity.position.Y);
            
        }
        public void AddToDrawBuffer(Tile tile, int tileMapX, int tileMapY)
        {
            (Texture2D texture, Rectangle rectangle, Texture2D normalMap /*TODO IMPLEMENT THIS*/) = ContentLoader.GetLoadedTileTexture(tile.textureIndex);
            float rot = 0f;
            if ((tile.textureIndex + 20) % 30 == 0)
            {
                rot = tile.rotation * 1.57079f;
            }

            //effect.Parameters["SpriteTexture"].SetValue(texture);

            if (normalMap != null)
            {
                effect.Parameters["NormalTexture"].SetValue(normalMap);
            }


            spriteBatch.Draw(texture, new Vector2(tileMapX * 32, tileMapY * 32), rectangle, Color.White, rot, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.04f);

            if (tile.decorationIndex != 0)
            {
                (Texture2D decoTexture, Rectangle decoRectangle, Texture2D decoNormalMap /*TODO IMPLEMENT THIS*/) = ContentLoader.GetLoadedOtherTexture(tile.decorationIndex);
                if (decoRectangle != Rectangle.Empty)
                    spriteBatch.Draw(decoTexture, new Vector2(tileMapX * 32, tileMapY * 32), decoRectangle, Color.White, 0f, new Vector2(decoRectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.039f);
                else
                    spriteBatch.Draw(decoTexture, new Vector2(tileMapX * 32, tileMapY * 32), null, Color.White, 0f, new Vector2(16, 16), Vector2.One, SpriteEffects.None, 0.02f - 0.00000001f * tileMapY * 32);
            }
        }
        public void AddToUiBuffer(List<Menu> menus)
        {
            foreach (Menu menu in menus)
            {
                if (menu.isActive)
                {
                    foreach (UiElement element in menu.elements)
                    {
                        if (!(element is Meter || element is MiniMap || element is Inventory) && element.isEnabled)
                        {
                            if (menu.menuType != Menu.MenuType.levelEditor)
                            {
                                (Texture2D texture, Rectangle rectangle, _) = ContentLoader.GetLoadedOtherTexture(element.textureIndex);
                                if (rectangle != Rectangle.Empty)
                                    uiSpriteBatch.Draw(texture, new Vector2(element.xOffset, element.yOffset), rectangle, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * element.scale, Settings.uiScaleY * element.scale), SpriteEffects.None, element.drawOrder);
                                else
                                    uiSpriteBatch.Draw(texture, new Vector2(element.xOffset, element.yOffset), null, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * element.scale, Settings.uiScaleY * element.scale), SpriteEffects.None, element.drawOrder);
                            }
                            else
                            {
                                if (element is Button)
                                {
                                    (Texture2D texture, Rectangle rectangle, _) = ContentLoader.GetLoadedTileTexture(element.textureIndex);
                                    uiSpriteBatch.Draw(texture, new Vector2(element.xOffset, element.yOffset), rectangle, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * element.scale, Settings.uiScaleY * element.scale), SpriteEffects.None, element.drawOrder);
                                }
                                else
                                {
                                    (Texture2D texture, Rectangle rectangle, _) = ContentLoader.GetLoadedTileTexture(element.textureIndex);
                                    uiSpriteBatch.Draw(texture, new Vector2(element.xOffset, element.yOffset), rectangle, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * element.scale, Settings.uiScaleY * element.scale), SpriteEffects.None, element.drawOrder);
                                }
                               
                            }
                        }
                        else if (element is Meter && element.isEnabled)
                        {
                            Meter meter = (Meter)element;
                            (Texture2D backTexture, Rectangle backRectangle, _) = ContentLoader.GetLoadedOtherTexture(element.textureIndex);
                            (Texture2D frontTexture, Rectangle frontRectangle, _) = ContentLoader.GetLoadedOtherTexture(meter.frontTextureIndex);
                            uiSpriteBatch.Draw(backTexture, new Vector2(element.xOffset, element.yOffset), null, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * meter.xScale, Settings.uiScaleY * meter.yScale), SpriteEffects.None, 0.1f);
                            uiSpriteBatch.Draw(frontTexture, new Vector2(element.xOffset + meter.xMeterOffset * Settings.uiScaleX, element.yOffset + meter.yMeterOffset * Settings.uiScaleY), meter.drawingMask, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * meter.xScale, Settings.uiScaleY * meter.yScale), SpriteEffects.None, 0f);
                        }
                        else if (element is MiniMap && element.isEnabled)
                        {
                            MiniMap miniMap = (MiniMap)element;
                            miniMap.DrawMiniMap(uiSpriteBatch);
                        }
                        else if (element is Inventory)
                        {
                            Inventory inventory = (Inventory)element;
                            inventory.Draw(uiSpriteBatch);
                        }
                    }
                }
            }
        }
        public void AddToDrawBuffer(List<Particle> particleList)
        {
            foreach(Particle particle in particleList)
            {
                (Texture2D texture, Rectangle rectangle, _) = ContentLoader.GetLoadedTileTexture(particle.textureIndex);
                spriteBatch.Draw(texture, particle.position, rectangle, Color.White, particle.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), particle.scale, SpriteEffects.None, 0.01f);
            }
        }
        public void AddToDrawBuffer(List<GameObject> gameObjectList)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                if (gameObject.isDrawn)
                {
                    if (gameObject is Entity)
                    {
                        Entity entity = (Entity)gameObject;
                        if (entity.isFlipped == false)
                            AddToDrawBuffer(entity, SpriteEffects.None);
                        else
                            AddToDrawBuffer(entity, SpriteEffects.FlipHorizontally);
                    }
                    else if (gameObject is Weapon)
                    {
                        if (gameObject.rotation > 1.573 || gameObject.rotation < -1.573)
                            AddToDrawBuffer(gameObject, SpriteEffects.FlipVertically);
                        else
                            AddToDrawBuffer(gameObject, SpriteEffects.None);
                    }
                    else if (gameObject is Objects.Potion)
                    {
                        (Texture2D texture, Rectangle rectangle, Texture2D normalMap /*TODO IMPLEMENT THIS*/) = ContentLoader.GetLoadedOtherTexture(gameObject.textureIndex);
                        if (rectangle != Rectangle.Empty)
                            spriteBatch.Draw(texture, gameObject.position, rectangle, Color.White, gameObject.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.02f - 0.00000001f * gameObject.position.Y);
                        else
                            spriteBatch.Draw(texture, gameObject.position, null, Color.White, gameObject.rotation, new Vector2(16, 16), Vector2.One, SpriteEffects.None, 0.02f - 0.00000001f * gameObject.position.Y);
                    }
                    else
                        AddToDrawBuffer(gameObject);
                }
            }
        }
        public void AddToDrawBuffer(GameObject gameObject)
        {
            (Texture2D texture, Rectangle rectangle, Texture2D normalMap /*TODO IMPLEMENT THIS*/) = ContentLoader.GetLoadedTileTexture(gameObject.textureIndex);
            spriteBatch.Draw(texture, gameObject.position, rectangle, Color.White, gameObject.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.02f - 0.00000001f * gameObject.position.Y);
        }
        public void AddToDrawBuffer(GameObject gameObject, SpriteEffects spriteEffect)
        {
            (Texture2D texture, Rectangle rectangle, Texture2D normalMap /*TODO IMPLEMENT THIS*/) = ContentLoader.GetLoadedTileTexture(gameObject.textureIndex);
            if (rectangle != Rectangle.Empty)
                spriteBatch.Draw(texture, gameObject.position, rectangle, Color.White, gameObject.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0.02f - 0.00000001f * gameObject.position.Y);
            else
                spriteBatch.Draw(texture, gameObject.position, null, Color.White, gameObject.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0.02f - 0.00000001f * gameObject.position.Y);
        }

        public void BeginUiBuffer() => uiSpriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);
        public void DrawUiBuffer() => uiSpriteBatch.End();
        public void DrawBuffer() => spriteBatch.End(); //Draws what is in the buffer
        public void BeginBuffer(ContentManagers.Camera.Camera camera) => spriteBatch.Begin(SpriteSortMode.Immediate,null,SamplerState.PointClamp,transformMatrix: camera.Transform, effect: effect); //Begins the buffer with a matrix transform
        public void BeginBuffer() => spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp); //Begins the buffer without camera matrix
    }
}
