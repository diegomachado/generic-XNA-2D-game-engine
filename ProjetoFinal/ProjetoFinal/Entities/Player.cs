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
        public Rectangle NextPosition { get; set; }
        public bool OnGround { get; set; } 

        public Vector2 Speed { get; set; }
        public Vector2 Gravity { get; set; }
        public Vector2 JumpForce  { get; set; }
        public float Friction { get; set; }

        public Vector2 speed = Vector2.Zero;

        public Point debugCorner1 { get; set; }
        public Point debugCorner2 { get; set; }
        public Point debugCorner3 { get; set; }
        public Point debugCorner4 { get; set; }

        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox)
        {            
            Speed = new Vector2(1.0f, 0.0f);
            Friction = 0.85f;
            Gravity = new Vector2(0.0f, 0.3f);
            JumpForce = new Vector2(0.0f, - 8.0f);
            State = PlayerState.Idle;

            Skin = playerSkin;
            Position = playerPosition;
            BoundingBox = boundingBox;
            CollisionBox = new Rectangle((int)Position.X + BoundingBox.X, (int)Position.Y + BoundingBox.Y, BoundingBox.Width, BoundingBox.Height);
        }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox, Vector2 gravity)
        {            
            Speed = new Vector2(1.0f, 0.0f);
            Friction = 0.85f;            
            JumpForce = new Vector2(0.0f, -8.0f);
            State = PlayerState.Idle;

            Skin = playerSkin;
            Position = playerPosition;
            BoundingBox = boundingBox;
            CollisionBox = new Rectangle((int)Position.X + BoundingBox.X, (int)Position.Y + BoundingBox.Y, BoundingBox.Width, BoundingBox.Height);
            Gravity = gravity;
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
            if (State == PlayerState.WalkingLeft || State == PlayerState.JumpingLeft)
                spriteBatch.Draw(Skin, Position - Camera.Instance.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(Skin, Position - Camera.Instance.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}
