using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class Inventory : UiElement
    {
        int inventorySizeX;
        int inventorySizeY;
        int inventorySizeTotal { get => inventorySizeX * inventorySizeY; }
        float xborderSizePx;
        float yborderSizePx;
        float itemSpacingPx;
        List<(ushort textureIndex, int durability, Meter meter)> items = new List<(ushort textureIndex, int durability, Meter meter)>();
        int highlightedWeapon;

        public Inventory(ushort backTextureIndex, int xOffset, int yOffset, Rectangle inventoryTextureSize, int inventorySizeX, int inventorySizeY, float xborderSizePx, float yborderSizePx, float itemSpacingPx) : base(backTextureIndex, xOffset, yOffset, inventoryTextureSize)
        {
            this.inventorySizeX = inventorySizeX;
            this.inventorySizeY = inventorySizeY;
            this.xborderSizePx = xborderSizePx;
            this.yborderSizePx = yborderSizePx;
            this.itemSpacingPx = itemSpacingPx;
            scale = 0.5f;
        }

        public void Update<T>(List<T> items, int highlightedWeapon) where T : Weapon
        {
            this.highlightedWeapon = highlightedWeapon;
            if (items is List<Weapon>)
            {
                if (this.items.Count > items.Count)
                {
                    this.items.Clear();
                }
                for (int x = 0; x < items.Count; x++)
                {
                    Weapon weapon = items[x];
                    weapon.Update();
                    if (this.items.Count > x)
                    {
                        this.items[x] = (weapon.textureIndex, weapon.durability, this.items[x].meter);
                    }
                    else
                    {
                        Meter meter = new Meter(58, 59, 110, 110, new Rectangle(0,0,28,3), false, weapon.maxDurability, 0);
                        this.items.Add((weapon.textureIndex, weapon.durability, meter));
                    }
                }
                foreach (var item in this.items)
                {
                    item.meter.Update(item.durability);
                }
            }
            Update();
        }

        public void Draw(SpriteBatch uiSpriteBatch)
        {
            (Texture2D backTexture, Rectangle backRectangle) = ContentLoader.GetLoadedOtherTexture(textureIndex); //TEMP scaling below
            uiSpriteBatch.Draw(backTexture, new Vector2(xOffset, yOffset), null, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * scale, Settings.uiScaleY * scale), SpriteEffects.None, 0.11f);
            for (int x = 0; x < inventorySizeX; x++)
            {
                for (int y = 0; y < inventorySizeY; y++)
                {
                    if ((items.Count > y * inventorySizeX + x))
                    {
                        (ushort itemTextureIndex, int itemDurability, Meter meter) = items[y * inventorySizeX + x];
                        Vector2 position = new Vector2(xOffset + xborderSizePx * scale * Settings.uiScaleX + x * itemSpacingPx * scale * Settings.uiScaleX, yOffset + yborderSizePx * scale * Settings.uiScaleX + y * itemSpacingPx * scale * Settings.uiScaleY);

                        (Texture2D backMeterTexture, Rectangle backMeterRectangle) = ContentLoader.GetLoadedOtherTexture(meter.textureIndex);
                        (Texture2D frontTexture, Rectangle frontRectangle) = ContentLoader.GetLoadedOtherTexture(meter.frontTextureIndex);

                        uiSpriteBatch.Draw(backMeterTexture, new Vector2(position.X + 2 * Settings.uiScaleX * scale, position.Y + 27 * Settings.uiScaleY * scale), null, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * scale, Settings.uiScaleY * scale), SpriteEffects.None, 0.1f);
                        uiSpriteBatch.Draw(frontTexture, new Vector2(position.X + 2 * Settings.uiScaleX * scale, position.Y + 27 * Settings.uiScaleY * scale), meter.drawingMask, Color.White, 0f, new Vector2(0, 0), new Vector2(Settings.uiScaleX * scale, Settings.uiScaleY * scale), SpriteEffects.None, 0f);

                        (Texture2D itemTexture, Rectangle itemRectangle) = ContentLoader.GetLoadedTileTexture(itemTextureIndex);
                        
                        uiSpriteBatch.Draw(itemTexture, position, itemRectangle, Color.White, 0f, Vector2.Zero, new Vector2(Settings.uiScaleX * scale, Settings.uiScaleY * scale), SpriteEffects.None, 0f);
                        if (y * inventorySizeX + x == highlightedWeapon)
                        {
                            (Texture2D borderTexture, Rectangle borderRectangle) = ContentLoader.GetLoadedTileTexture(99);
                            uiSpriteBatch.Draw(borderTexture, position, borderRectangle, Color.White, 0f, Vector2.Zero, new Vector2(Settings.uiScaleX * scale, Settings.uiScaleY * scale), SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }
    }
}
