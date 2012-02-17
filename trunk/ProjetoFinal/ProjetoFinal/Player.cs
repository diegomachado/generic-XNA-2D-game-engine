using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal
{
    class Player
    {
        public Texture2D skin;
        public Vector2 position;
        public float speed;
        public string nickname;

        public Player(Texture2D playerSkin, Vector2 playerPosition, float playerSpeed)
        {
            this.skin = playerSkin;
            this.position = playerPosition;
            this.speed = playerSpeed;
        }

        public Player(string playerNickname, Texture2D playerSkin, Vector2 playerPosition, float playerSpeed)
        {
            this.nickname = playerNickname;
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

        public void Initialize(Texture2D playerSkin, Vector2 playerPosition, float playerSpeed)
        {
            skin = playerSkin;
            position = playerPosition;
            speed = playerSpeed;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(skin, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        } 
    }
}