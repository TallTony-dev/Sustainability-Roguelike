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
                elements.Add(new Button(0, 0, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(0, 64, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(30, 96, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(0, 256, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(30, 288, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(30, 384, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(0, 416, 0, new Rectangle(0, 0, 32, 32), 0f));
                elements.Add(new Button(90, 448, 0, new Rectangle(0, 0, 32, 32), 0f));
            }
            if (menuType == MenuType.settings)
            {
                elements.Add(new UiElement(23, 28, 28, new Rectangle(0, 0, 99, 98), 0.01f)); //resolution background and buttons below
                elements.Add(new Button(24, 39, 64, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(25, 39, 74, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(26, 39, 84, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(27, 39, 94, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(28, 39, 104, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(29, 80, 64, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(30, 80, 74, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(31, 80, 84, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(32, 80, 94, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new Button(33, 80, 104, new Rectangle(0, 0, 36, 7), 0f));
                elements.Add(new AnimButton(44, 300, 196, new Rectangle(0, 0, 146, 60), 0f)); //apply button
                elements.Add(new AnimButton(48, 300, 28, new Rectangle(0, 0, 146, 60), 0f)); //fullscreen button
                elements.Add(new UiElement(68, 128, 28, new Rectangle(0, 0, 99, 98), 0.01f)); //Volume BG
                elements.Add(new Button(65, 138, 68, new Rectangle(0,0,12,12), 0f)); //minus button
                elements.Add(new Button(63, 206, 68, new Rectangle(0, 0, 12, 12), 0f)); //plus button
                elements.Add(new Button(67, 150, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
                elements.Add(new Button(67, 158, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
                elements.Add(new Button(67, 166, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
                elements.Add(new Button(67, 174, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
                elements.Add(new Button(67, 182, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
                elements.Add(new Button(67, 190, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
                elements.Add(new Button(67, 198, 66, new Rectangle(0, 0, 8, 16), 0f)); //volume tick
            }
            if (menuType == MenuType.inGameUi)
            {
                elements.Add(new MiniMap(1, 0, 0, new Rectangle(0, 0, 32, 32))); //minimap
                elements.Add(new Meter(60, 61, 120, 20, new Rectangle(2, 2, 52, 8), false, 5000, 0)); //health meter
                elements.Add(new Inventory(2, 178, 241, new Rectangle(0, 0, 300, 48), 8, 1, 32, 8, 33)); //inventory

            }
            if (menuType == MenuType.opening)
            {
                elements.Add(new AnimButton(7, 14, 14, new Rectangle(0, 0, 146, 60), 0f)); //start button
                elements.Add(new AnimButton(3, 14, 196, new Rectangle(0, 0, 146, 60), 0f)); //exit button
                elements.Add(new AnimButton(19, 14, 84, new Rectangle(0, 0, 146, 60), 0f)); //settings button
            }
            if (menuType == MenuType.pauseMenu)
            {
                elements.Add(new AnimButton(11, 14, 100, new Rectangle(0, 0, 146, 60), 0f)); //resume button
                elements.Add(new AnimButton(19, 167, 100, new Rectangle(0, 0, 146, 60), 0f)); //settings button
                elements.Add(new AnimButton(15, 320, 100, new Rectangle(0, 0, 146, 60), 0f)); //menu button
            }
            if (menuType == MenuType.win)
            {
                elements.Add(new AnimButton(15, 18, 100, new Rectangle(0, 0, 146, 60), 0f)); //menu button
                elements.Add(new UiElement(74, 215, 48, new Rectangle(0, 0, 99, 98), 0.01f)); //youWin
            }
            if (menuType == MenuType.lose)
            {
                elements.Add(new AnimButton(15, 18, 100, new Rectangle(0, 0, 146, 60), 0f)); //menu button
                elements.Add(new UiElement(73, 215, 48, new Rectangle(0, 0, 99, 98), 0.01f)); //youLose
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
        public virtual bool IsButtonHovered(ushort buttonIndex)
        {
            if (elements[buttonIndex].IsHovered())
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

        public enum MenuType { pauseMenu, levelEditor, settings, inGameUi, opening, win, lose }
    }
}
