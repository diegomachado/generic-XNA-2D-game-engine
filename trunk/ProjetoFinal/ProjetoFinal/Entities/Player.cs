using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers.LocalPlayerStates;

namespace ProjetoFinal.Entities
{
    class Player
    {
        private Camera camera = Camera.Instance;
        public VerticalStateType LastVerticalState { get; set; }
        public HorizontalStateType LastHorizontalState { get; set; }
        public Texture2D Skin { get; set; }
        bool flipped = false;
        public bool FacingLeft 
        {
            get { return flipped; }
            set { flipped = value; }
        }

        public Vector2 Position { get; set; }
        public Rectangle NextPosition { get; set; }
        public Rectangle BoundingBox { get; set; }

        private Rectangle collisionBox;
        public Rectangle CollisionBox
        {
            get
            {
                collisionBox.X = (int)Position.X + BoundingBox.X;
                collisionBox.Y = (int)Position.Y + BoundingBox.Y;
                return collisionBox;
            }
        }

        public Vector2 Gravity { get; set; }
        public Vector2 JumpForce { get; set; }

        public Vector2 walkForce { get; set; }        
        public float Friction { get; set; }
        private Vector2 speed;

        public Vector2 Speed 
        {
            get { return speed; }
            set { speed = value; }
        }

        public float SpeedX
        {
            get { return speed.X; }
            set 
            {
                if (Math.Abs(value) < 30)
                    speed.X = 0;
                else
                    speed.X = value; 
            }
        }
        
        public float SpeedY
        {
            get { return speed.Y; }
            set 
            {
                if (Math.Abs(value) > 500)
                    speed.Y = 500;
                else
                    speed.Y = value;
            }
        }

        public bool isMovingHorizontally
        {
            get
            {
                return (speed.X == 0);
            }
        }

        public bool isMovingVertically
        {
            get
            {
                return (speed.Y == 0);
            }
        }
        
        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox)
        {            
            walkForce = new Vector2(60, 0);
            Friction = 0.85f;
            Gravity = new Vector2(0, 20f);
            JumpForce = new Vector2(0, -480f);
            LastVerticalState = VerticalStateType.Idle;
            LastHorizontalState = HorizontalStateType.Idle;
            Skin = playerSkin;
            Position = playerPosition;
            BoundingBox = boundingBox;
            collisionBox = boundingBox;
        }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox, Vector2 gravity)
        {
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
            if (FacingLeft)
                spriteBatch.Draw(Skin, Position - camera.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(Skin, Position - camera.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
