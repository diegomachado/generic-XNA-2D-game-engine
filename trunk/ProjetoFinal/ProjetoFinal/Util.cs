using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers;

namespace ProjetoFinal
{
    static class Util
    {
        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        static Texture2D textureColor;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle r, int borderWidth, Color color)
        {
            textureColor = TextureManager.Instance.GetPixelTextureByColor(color);
            spriteBatch.Draw(textureColor, new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, borderWidth, r.Height), Color.White);
            spriteBatch.Draw(textureColor, new Rectangle(r.Right - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, borderWidth, r.Height), Color.White);
            spriteBatch.Draw(textureColor, new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, r.Width, borderWidth), Color.White);
            spriteBatch.Draw(textureColor, new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Bottom - (int)Camera.Instance.Position.Y, r.Width, borderWidth), Color.White);
        }
    }
}
