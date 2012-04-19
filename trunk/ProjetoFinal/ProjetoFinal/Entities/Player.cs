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
        public PlayerState State { get; set; }
        public Texture2D Skin   { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle BoundingBox { get; set; }
        public Rectangle CollisionBox;
        bool OnGround { get; set; } 

        public float Friction   { get; set; }
        public Vector2 Gravity  { get; set; }
        public float JumpForce  { get; set; }

        public Vector2 speed = Vector2.Zero;

        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox)
        {
            Skin = playerSkin;
            Position = playerPosition;
            Friction = 0.85f;
            Gravity = new Vector2(0.0f, 0.3f);
            JumpForce = -8.0f;
            State = PlayerState.Idle;
            BoundingBox = boundingBox;

            CollisionBox = new Rectangle((int)Position.X + BoundingBox.X, (int)Position.Y + BoundingBox.Y, BoundingBox.Width, BoundingBox.Height);
        }

        public int Width { get { return Skin.Width; } }

        public int Height { get { return Skin.Height; } }

        public void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Skin, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);            
        }

    }
}
