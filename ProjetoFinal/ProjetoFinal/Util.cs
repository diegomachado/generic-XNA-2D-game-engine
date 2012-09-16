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
        public static Camera camera = Camera.Instance;

        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        static Texture2D pixelTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle r, int borderWidth, Color color)
        {
            pixelTexture = TextureManager.Instance.GetPixelTexture();
            spriteBatch.Draw(pixelTexture, new Rectangle(r.Left - (int)camera.Position.X, r.Top - (int)camera.Position.Y, borderWidth, r.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(r.Right - (int)camera.Position.X, r.Top - (int)camera.Position.Y, borderWidth, r.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(r.Left - (int)camera.Position.X, r.Top - (int)camera.Position.Y, r.Width, borderWidth), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(r.Left - (int)camera.Position.X, r.Bottom - (int)camera.Position.Y, r.Width, borderWidth), color);
        }
    }
}
