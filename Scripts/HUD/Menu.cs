using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class Menu
    {
        public bool isActive { get; private set; } = false; //should be false eventually
        public MenuType menuType;
        public List<UiElement> elements { get; private set; }
        public Menu(List<UiElement> elements)
        {
            this.elements = elements;
        }
        public Menu(UiElement element)
        {
            elements.Add(element);
        }
        public Menu(MenuType menuType)
        {
            elements = new List<UiElement>();
            this.menuType = menuType;
            if (menuType == MenuType.levelEditor)
            {
                elements.Add(new Button(0, 0, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 64, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(30, 96, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 256, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(30, 288, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(30, 384, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 416, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(90, 448, 0, new Rectangle(0, 0, 32, 32)));
            }
            if (menuType == MenuType.settings)
            {
                elements.Add(new Button(1, 0, 50, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 100, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 150, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 200, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 238, new Rectangle(0, 0, 32, 32)));
            }
            if (menuType == MenuType.inGameUi)
            {
                elements.Add(new MiniMap(1, 0, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Meter(16, 32, 120, 20, new Rectangle(0, 0, 32, 32), false, 10000, 0));
                elements.Add(new Inventory(2, 178, 241, new Rectangle(0, 0, 128, 32), 8, 1, 14, 8, 33));

            }
            if (menuType == MenuType.opening)
            {
                elements.Add(new Button(7, 34, 34, new Rectangle(0, 0, 146, 60)));
                elements.Add(new Button(3, 34, 176, new Rectangle(0, 0, 146, 60)));
            }
            if (menuType == MenuType.pauseMenu)
            {
                elements.Add(new Button(15, 34, 100, new Rectangle(0, 0, 146, 60)));
                elements.Add(new Button(11, 300, 100, new Rectangle(0, 0, 146, 60)));
            }
            if (menuType == MenuType.win)
            {
                elements.Add(new Button(15, 300, 100, new Rectangle(0, 0, 146, 60)));
            }
            Game1.menus.Add(this);
        }
        public void WipeLeft()
        {
            foreach (UiElement element in elements)
            {
                element.MoveTo(-1920, (int)element.yOffset, 1f, 100, UiElement.MovementType.bounce);
            }
        }
        public void UpdateElements()
        {
            foreach (UiElement element in elements)
            {
                if (!(element is Meter))
                    element.Update();
            }
        }
        public void AddElement(UiElement elementToAdd)
        {
            elements.Add(elementToAdd);
        }
        /// <summary>
        /// Checks if button in the menu of buttonidex is pressed, will return false if uielement at index is not a button
        /// </summary>
        public virtual bool IsButtonPressed(ushort buttonIndex)
        {
            if (elements[buttonIndex].IsPressed())
            {
                return true;
            }
            return false;
        }

        public void EnableMenu()
        {
            isActive = true;
            Game1.menus.Add(this);
            foreach (var element in elements)
            {
                element.isEnabled = true;
                element.Update();
            }
        }
        public void DisableMenu()
        {
            isActive = false;
            Game1.menus.Remove(this);
            foreach (var element in elements)
            {
                element.Disable();
            }
        }

        public enum MenuType { pauseMenu, levelEditor, settings, inGameUi, opening, win }
    }
}
