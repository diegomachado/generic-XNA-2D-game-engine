using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.GUI.Elements
{
    class TextField : GUIElement
    {
        Texture2D backgroundImage;
        float textHorizontalOffset = 20f;
        bool focus = false;

        public String Text { get; private set; }

        public TextField(String text, Rectangle frame, Texture2D texture) : base(frame)
        {
            this.backgroundImage = texture;
            Text = text;
        }

        public override void Update(Managers.InputManager inputManager)
        {
            if (inputManager.MouseLeftButton)
            {
                if(frame.Contains(inputManager.MousePosition))
                    focus = true;
                else
                    focus = false;
            }

            if(focus)
            {
                Text += inputManager.TextInput;

                if (inputManager.BackSpace && Text.Length > 0)
                    Text = Text.Substring(0, Text.Length - 1);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Vector2 stringSize = spriteFont.MeasureString(Text);

            spriteBatch.Draw(backgroundImage, frame, Color.White);
            spriteBatch.DrawString(spriteFont, Text, new Vector2(frame.Left + textHorizontalOffset, frame.Center.Y - (stringSize.Y / 2)), (focus) ? Color.White : Color.Black);
        }
    }
}
