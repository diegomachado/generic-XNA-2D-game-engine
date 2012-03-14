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
        public Texture2D skin { get; set; }
        public Vector2 position { get; set; }
        public float speed { get; set; }

        // TODO: Tirar isso daqui ou não
        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition)
        {
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
