using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities.Utils
{
    class SpriteMap
    {
        public bool complete = true;
        public float rate = 1;

        public Rectangle sourceRect;
        public Texture2D spriteSheet;
        public int width, height, columns, rows;
        public int frameWidth, frameHeight;
        public Vector2 Center
        {
            get { return new Vector2((float) sourceRect.Width/2, (float) sourceRect.Height/2); }
        }
        public int frameCount;
        
        public int index;
        public Animation currentAnimation;
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        public float timer = 0;
        protected int frame;
        Random rand = new Random();

        public SpriteMap(Texture2D spriteSheet, int frameWidth = 0, int frameHeight = 0)
        {
            this.spriteSheet = spriteSheet;
            sourceRect = new Rectangle(0, 0, frameWidth, frameHeight);
            if (frameWidth == 0) sourceRect.Width = spriteSheet.Width;
            if (frameHeight == 0) sourceRect.Height= spriteSheet.Height;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            width = spriteSheet.Width;
            height = spriteSheet.Height;
            columns = (int)Math.Ceiling((double)width/sourceRect.Width);
            rows = (int)Math.Ceiling((double)height/sourceRect.Height);
            frameCount = columns * rows;            
        }

        public void Update(GameTime gametime)
        {
            if (currentAnimation != null && !complete)
            {
                timer += currentAnimation.frameRate * rate * gametime.ElapsedGameTime.Seconds;
                if (timer >= 1)
                {
                    while (timer >= 1)
                    {
                        timer--;
                        index++;
                        if (index == currentAnimation.frameCount)
                        {
                            if (currentAnimation.loop)
                            {
                                index = 0;
                            }
                            else
                            {
                                index = currentAnimation.frameCount - 1;
                                complete = true;
                                break;
                            }
                        }
                    }

                    if (currentAnimation != null) 
                        frame = currentAnimation.frames[index];
                }
                
            }
        }

        Rectangle destinationRectangle;
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flip = false)
        {
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, sourceRect.Width, sourceRect.Height);

            if (flip)
                spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRect, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float alpha, float angle, float scale)
        {
            spriteBatch.Draw(spriteSheet, position, null, Color.White * alpha, angle, Center, scale, SpriteEffects.None, 0);
        }


        public Animation Add(string name, int[] frames, float frameRate = 0, bool loop = true)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] %= frameCount;
                if (frames[i] < 0) frames[i] += frameCount;
            }
            animations.Add(name, new Animation(name, frames, frameRate, loop));
            animations[name].parent = this;
            return animations[name];
        }

        public Animation Play(string name, bool reset = false, int frame = 0)
        {
            if (!reset && currentAnimation != null && currentAnimation == animations[name]) return currentAnimation;
            currentAnimation = animations[name];
            if (currentAnimation == null)
            {
                frame = index = 0;
                complete = true;
                return null;
            }
            index = 0;
            timer = 0;
            this.frame = currentAnimation.frames[frame % currentAnimation.frameCount];
            complete = false;
            return currentAnimation;
        }

        public int GetFrame(int column = 0, int row = 0)
        {
            return (row % rows) * columns + (column % columns);
        }

        public void SetFrame(int column = 0, int row = 0)
        {
            currentAnimation = null;
            int frame = (row % rows) * columns + (column % columns);
            if (this.frame == frame) return;
            this.frame = frame;
            timer = 0;
        }

        public void RandFrame()
        {
            frame = rand.Next(frameCount);
        }
    }
}
