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
        /// Adds something to the draw buffer which is drawn after DrawBuffer is called. This overload includes a sourceRectangle for spritesheets.
        /// </summary>
        public void AddToDrawBuffer(Entity entity, SpriteEffects spriteEffect)
        {
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(entity.textureIndex);
            spriteBatch.Draw(texture, entity.position, rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0.03f);

            //(Texture2D weaponTexture, Rectangle weaponRectangle) = ContentLoader.GetLoadedTileTexture(entity.activeWeapon.textureIndex);
            //if (entity.activeWeapon.rotation > 1.573 || entity.activeWeapon.rotation < -1.573)
            //    spriteBatch.Draw(weaponTexture, entity.activeWeapon.position, weaponRectangle, Color.White, entity.activeWeapon.rotation, new Vector2(weaponRectangle.Width / 2, weaponRectangle.Height / 2), Vector2.One, SpriteEffects.FlipVertically, 0.02f);
            //else
            //    spriteBatch.Draw(weaponTexture, entity.activeWeapon.position, weaponRectangle, Color.White, entity.activeWeapon.rotation, new Vector2(weaponRectangle.Width / 2, weaponRectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.02f);
        }
        public void AddToDrawBuffer(Tile tile, int tileMapX, int tileMapY)
        {
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(tile.textureIndex);
            //really temp for testing only
            if (!TileMap.tileMap[tileMapX,tileMapY].isBeingPathfoundTo)
                spriteBatch.Draw(texture, new Vector2(tileMapX * 32, tileMapY * 32), rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.04f);
            else
                spriteBatch.Draw(texture, new Vector2(tileMapX * 32, tileMapY * 32), rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.04f);
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
                            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(element.textureIndex);
                            uiSpriteBatch.Draw(texture, new Vector2(element.xOffset, element.yOffset), rectangle, Color.White, 0f, new Vector2(0,0), new Vector2(Settings.uiScaleX * element.scale, Settings.uiScaleY * element.scale), SpriteEffects.None, 0f);
                        }
                        else if (element is Meter && element.isEnabled)
                        {
                            Meter meter = (Meter)element;
                            (Texture2D backTexture, Rectangle backRectangle) = ContentLoader.GetLoadedTileTexture(element.textureIndex);
                            (Texture2D frontTexture, Rectangle frontRectangle) = ContentLoader.GetLoadedTileTexture(meter.frontTextureIndex);
                            uiSpriteBatch.Draw(backTexture, new Vector2(element.xOffset * Settings.uiScaleX, element.yOffset * Settings.uiScaleY), backRectangle, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * meter.xScale, Settings.uiScaleY * meter.yScale), SpriteEffects.None, 0.1f);
                            uiSpriteBatch.Draw(frontTexture, new Vector2(element.xOffset * Settings.uiScaleX, element.yOffset * Settings.uiScaleY), meter.drawingMask, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * meter.xScale, Settings.uiScaleY * meter.yScale), SpriteEffects.None, 0f);
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
        public void AddToDrawBuffer(List<GameObject> gameObjectList)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                if (gameObject.isEnabled)
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
                    else
                        AddToDrawBuffer(gameObject);
                }
            }
        }
        public void AddToDrawBuffer(GameObject gameObject)
        {
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(gameObject.textureIndex);
            spriteBatch.Draw(texture, gameObject.position, rectangle, Color.White, gameObject.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.02f);
        }
        public void AddToDrawBuffer(GameObject gameObject, SpriteEffects spriteEffect)
        {
            (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(gameObject.textureIndex);
            spriteBatch.Draw(texture, gameObject.position, rectangle, Color.White, gameObject.rotation, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, spriteEffect, 0.02f);
        }
        public void AddToDrawBuffer(List<Projectile> gameObjectList)
        {
            foreach (Projectile projectile in gameObjectList)
            {
                (Texture2D texture, Rectangle rectangle) = ContentLoader.GetLoadedTileTexture(projectile.textureIndex);
                spriteBatch.Draw(texture, projectile.position, rectangle, Color.White, projectile.travelAngle, new Vector2(rectangle.Width / 2, rectangle.Height / 2), Vector2.One, SpriteEffects.None, 0.02f);
            }
        }

        public void BeginUiBuffer() => uiSpriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);
        public void DrawUiBuffer() => uiSpriteBatch.End();
        public void DrawBuffer() => spriteBatch.End(); //Draws what is in the buffer
        public void BeginBuffer(ContentManagers.Camera.Camera camera) => spriteBatch.Begin(SpriteSortMode.BackToFront,null,SamplerState.PointClamp,transformMatrix: camera.Transform); //Begins the buffer with a matrix transform
        public void BeginBuffer() => spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp); //Begins the buffer without camera matrix
    }
}
