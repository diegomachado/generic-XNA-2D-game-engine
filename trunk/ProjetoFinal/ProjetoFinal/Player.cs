using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal
{
    class Player
    {
        public Texture2D PlayerSkin;

        public Vector2 Position;

        public int Width
        {
            get { return PlayerSkin.Width; }
        }

        public int Height
        {
            get { return PlayerSkin.Height; }
        }


        public void Initialize(Texture2D skin, Vector2 position)
        {
            PlayerSkin = skin;            
            Position = position;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerSkin, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        } 
    }
}