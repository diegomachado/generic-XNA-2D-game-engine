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
        public Texture2D Skin { get; set; }
        bool flipped = false;
        public bool Flipped 
        {
            get { return flipped; }
            set { flipped = value; }
        }

        public Vector2 Position { get; set; }
        public Rectangle NextPosition { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Rectangle CollisionBox;

        public Vector2 Gravity { get; set; }
        public Vector2 JumpForce { get; set; }
        public bool OnGround { get; set; } 

        public Vector2 walkForce { get; set; }        
        public float Friction { get; set; }
        
        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox)
        {            
            walkForce = new Vector2(1.0f, 0.0f);
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
            walkForce = new Vector2(1.0f, 0.0f);
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
            // TODO: Tirar a conta Camera.Instance.Position - Position daqui e jogar ela dentro de Camera tipo: Camera.Instance.ScreenToCameraCoordinates(Position)
            if (Flipped)
                spriteBatch.Draw(Skin, Position - Camera.Instance.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(Skin, Position - Camera.Instance.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
