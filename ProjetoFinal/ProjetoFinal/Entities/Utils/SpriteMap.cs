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

        public Rectangle source;
        public Texture2D spriteSheet;
        public int width, height, columns, rows;
        public int frameWidth, frameHeight;
        public Vector2 FrameCenter
        {
            get { return new Vector2((float) source.Width/2, (float) source.Height/2); }
        }
        public int frameCount;
        
        public int index;
        public Animation currentAnimation;
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        public double timer = 0;
        protected int frame;
        Random rand = new Random();

        public SpriteMap(Texture2D spriteSheet, int frameWidth = 0, int frameHeight = 0)
        {
            this.spriteSheet = spriteSheet;
            source = new Rectangle(0, 0, frameWidth, frameHeight);
            if (frameWidth == 0) source.Width = spriteSheet.Width;
            if (frameHeight == 0) source.Height= spriteSheet.Height;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            width = spriteSheet.Width;
            height = spriteSheet.Height;
            columns = (int)Math.Ceiling((double)width/source.Width);
            rows = (int)Math.Ceiling((double)height/source.Height);
            frameCount = columns * rows;
        }

        public void Update(GameTime gametime)
        {
            if (currentAnimation != null && !complete)
            {
                timer += currentAnimation.frameRate * rate * gametime.ElapsedGameTime.TotalSeconds;
                if (timer >= 1)
                {
                    while (timer >= 1)
                    {
                        timer = 0;
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

                    if (currentAnimation != null) frame = currentAnimation.frames[index];
                    UpdateFrame();
                }
                
            }
        }

        Rectangle destinationRectangle;
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flip = false)
        {
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, source.Width, source.Height);

            if (flip)
                spriteBatch.Draw(spriteSheet, destinationRectangle, source, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatch.Draw(spriteSheet, destinationRectangle, source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float alpha, float angle, float scale)
        {
            spriteBatch.Draw(spriteSheet, position, null, Color.White * alpha, angle, FrameCenter, scale, SpriteEffects.None, 0);
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
            UpdateFrame();
            return currentAnimation;
        }

        public void UpdateFrame()
        {
            source.X = source.Width * (frame % columns);
            source.Y = source.Height * (frame / columns);
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
            UpdateFrame();
        }

        public void RandFrame()
        {
            frame = rand.Next(frameCount);
        }
    }
}
