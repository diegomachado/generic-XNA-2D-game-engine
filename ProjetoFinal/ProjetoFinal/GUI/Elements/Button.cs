using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers;
using Microsoft.Xna.Framework.Input;

namespace ProjetoFinal.GUI.Elements
{
    class Button : GUIElement
    {
        String label;
        bool hover;
        Texture2D backgroundImage;
        OnClicked onClicked;
        
        // Events
        public delegate void OnClicked();

        public Button(String label, Rectangle frame)
            : base(frame)
        {
            this.label = label;
            this.hover = false;
        }

        public Button(String label, Rectangle frame, Texture2D texture2D, OnClicked onClicked)
            : base(frame)
        {
            this.label = label;
            this.hover = false;
            this.backgroundImage = texture2D;
            this.onClicked = onClicked;
        }

        public override void Update(InputManager inputManager)
        {
            if (frame.Contains(inputManager.MousePosition))
            {
                hover = true;

                if (inputManager.MouseLeftButton)
                    onClicked();
            }
            else
            {
                hover = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Vector2 stringSize = spriteFont.MeasureString(label);

            spriteBatch.Draw(backgroundImage, frame, Color.White);
            spriteBatch.DrawString(spriteFont, label, new Vector2(frame.Center.X - (stringSize.X / 2), frame.Center.Y - (stringSize.Y / 2)), (hover) ? Color.Red : Color.Blue);
        }
    }
}
