using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities
{
    class Char
    {
        protected Texture2D skin;
        protected Vector2 position;
        protected float speed;

        public Char(Texture2D playerSkin, Vector2 playerPosition, float playerSpeed)
        {
            this.skin = playerSkin;
            this.position = playerPosition;
            this.speed = playerSpeed;
        }

        public int Width
        {
            get { return skin.Width; }
        }

        public int Height
        {
            get { return skin.Height; }
        }

        public void Initialize()
        {

        }

        public virtual void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(skin, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
