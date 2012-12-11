using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities.Utils
{
    class Animation
    {
        public SpriteMap parent;
        public string name;
        public int[] frames;
        public float frameRate;
        public int frameCount;
        public bool loop;

        public Animation(string name, int[] frames, float frameRate, bool loop)
        {
            this.name = name;
            this.frames = frames;
            this.frameRate = frameRate;
            this.frameCount = frames.Length;
            this.loop = loop;
        }

        public void Play(bool reset = false)
        {
            parent.Play(name, reset);
        }
    }
}
