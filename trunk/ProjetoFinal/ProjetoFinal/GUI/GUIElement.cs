using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers;

namespace ProjetoFinal.GUI
{
    abstract class GUIElement
    {
        protected Rectangle frame;

        public GUIElement(Rectangle frame)
        {
            this.frame = frame;
        }

        public abstract void Update(InputManager inputManager);
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont);
    }
}
