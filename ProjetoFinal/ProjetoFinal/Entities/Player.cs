using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities
{
    class Player
    {
        public short id { get; set; }
        public Texture2D skin { get; set; }
        public Vector2 position { get; set; }
        protected float speed;

        // TODO: Tirar isso daqui ou não
        public double LastUpdateTime { get; set; }

        public Player(short id, Texture2D playerSkin, Vector2 playerPosition)
        {
            this.id = id;
            this.skin = playerSkin;
            this.position = playerPosition;
            this.speed = 8;
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
