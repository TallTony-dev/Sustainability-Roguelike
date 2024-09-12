using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.HUD
{
    internal class Menu
    {
        public bool isActive { get; private set; } = true; //should be false eventually
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
            if (menuType == MenuType.levelEditor)
            {
                elements.Add(new Button(0, 0, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 64, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 96, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 256, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(0, 288, 0, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 1888, 0, new Rectangle(0, 0, 32, 32)));
            }
            if (menuType == MenuType.settings)
            {
                elements.Add(new Button(1, 0, 200, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 300, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 400, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 500, new Rectangle(0, 0, 32, 32)));
                elements.Add(new Button(1, 0, 600, new Rectangle(0, 0, 32, 32)));
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
            Game1.activeMenus.Add(this);
            foreach (var element in elements)
            {
                element.isEnabled = true;
            }
        }
        public void DisableMenu()
        {
            isActive = false;
            Game1.activeMenus.Remove(this);
            foreach (var element in elements)
            {
                element.Enable();
            }
        }

        public enum MenuType { pauseMenu, levelEditor, settings }
    }
}
