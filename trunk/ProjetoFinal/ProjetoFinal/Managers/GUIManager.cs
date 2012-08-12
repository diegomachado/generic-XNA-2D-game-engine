using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.GUI.Elements;
using ProjetoFinal.Managers;

namespace ProjetoFinal.GUI
{
    class GUIManager
    {
        List<GUIElement> elements;

        public GUIManager()
        {
            elements = new List<GUIElement>();
        }

        public void Update(InputManager inputManager)
        {
            foreach (GUIElement element in elements)
            {
                element.Update(inputManager);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (GUIElement element in elements)
            {
                element.Draw(spriteBatch, spriteFont);
            }
        }

        public void AddElement(GUIElement button)
        {
            elements.Add(button);
        }
    }
}
