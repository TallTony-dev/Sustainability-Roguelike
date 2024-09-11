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
        public List<UiElement> elements;
        public Menu(List<UiElement> elements)
        {
            this.elements = elements;
        }

        public virtual bool Update()
        {
            bool isPressed = false;
            foreach (Button button in elements)
            {
                if (button.IsPressed())
                {
                    isPressed = true;
                }
            }
            return isPressed;
        }

        public void EnableMenu()
        {
            isActive = true;
            foreach (var element in elements)
            {
                element.isEnabled = true;
            }
        }
        public void DisableMenu()
        {
            isActive = false;
            foreach (var element in elements)
            {
                element.Enable();
            }
        }
    }
}
