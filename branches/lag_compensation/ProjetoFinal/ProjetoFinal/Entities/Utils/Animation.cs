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

        Rectangle sourceRectangle, destinationRectangle;

        private int TotalFrames
        {
            get { return Rows * Columns; }
        }

        public Point FrameSize
        {
            get { return new Point(SpriteSheet.Width / Columns, SpriteSheet.Height / Rows); }
        }
        
        public Vector2 TextureCenter
        {
            get { return new Vector2(FrameSize.X / 2, FrameSize.Y / 2); }
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

        public void Draw(SpriteBatch spriteBatch, Vector2 location, bool flip)
        {
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;
            sourceRectangle = new Rectangle(FrameSize.X * column, FrameSize.Y * row, FrameSize.X, FrameSize.Y);
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, FrameSize.X, FrameSize.Y);            
            destinationRectangle.Offset((int) -Camera.Instance.Position.X,(int) -Camera.Instance.Position.Y);    

            if(flip)
                spriteBatch.Draw(SpriteSheet, destinationRectangle, sourceRectangle, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatch.Draw(SpriteSheet, destinationRectangle, sourceRectangle, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }
    }
}
