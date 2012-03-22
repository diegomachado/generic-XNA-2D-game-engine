using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities
{
    public enum PlayerState : short
    {
        Idle,
        WalkingLeft,
        WalkingRight,
        Jumping,
        JumpingRight,
        JumpingLeft
    }

    class Player
    {
        public PlayerState state { get; set; }
        public Texture2D skin   { get; set; }
        public Vector2 position { get; set; }

        public float friction   { get; set; }
        public float gravity    { get; set; }
        public float jumpForce  { get; set; }

        public Vector2 speed = Vector2.Zero;

        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition)
        {
            skin      = playerSkin;
            position  = playerPosition;

            speed = Vector2.Zero;
            friction  = 0.85f;
            gravity   = 0.3f;
            jumpForce = -8.0f;
            state = PlayerState.Idle;
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
