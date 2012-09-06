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
        InputManager inputManager = InputManager.Instance;

        public GUIManager()
        {
            elements = new List<GUIElement>();
        }

        public void Update()
        {
            foreach (GUIElement element in elements)
                element.Update(inputManager);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (GUIElement element in elements)
                element.Draw(spriteBatch, spriteFont);
        }

        public void AddElement(GUIElement element)
        {
            elements.Add(element);
        }
    }
}
