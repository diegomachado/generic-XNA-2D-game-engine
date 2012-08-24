using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities.Utils
{
    public class Animation
    {
        private int currentFrame;
        public Texture2D SpriteSheet { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        private int TotalFrames
        {
            get { return Rows * Columns; }
        }

        public Point FrameSize
        {
            get { return new Point(SpriteSheet.Width / Columns, SpriteSheet.Height / Rows); }
        }

        public Animation(Texture2D spriteSheet, int rows, int columns)
        {
            currentFrame = 0;
            SpriteSheet = spriteSheet;
            Rows = rows;
            Columns = columns;
        }

        public void Update()
        {
            currentFrame++;
            if (currentFrame == TotalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(FrameSize.X * column, FrameSize.Y * row, FrameSize.X, FrameSize.Y);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, FrameSize.X, FrameSize.Y);

            spriteBatch.Begin();
            spriteBatch.Draw(SpriteSheet, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }
    }
}
