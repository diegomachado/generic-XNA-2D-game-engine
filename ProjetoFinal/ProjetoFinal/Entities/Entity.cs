using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjetoFinal.Managers;

namespace ProjetoFinal.Entities
{
    class Entity
    {
        protected Texture2D skin;

        public Rectangle BoundingBox { get; protected set; }
        public Vector2 Gravity { get; protected set; }

        public Vector2 Position { get; set; }
        public double LastUpdateTime { get; set; }
        
        public int Width { get { return skin.Width; } }
        public int Height { get { return skin.Height; } }
        public bool isMovingHorizontally { get { return (speed.X == 0); } }
        public bool isMovingVertically { get { return (speed.Y == 0); } }
        public Vector2 Center { get { return Position + TextureCenter; } } // TODO: Guardar esse valor pra não calcular sempre
        public Vector2 TextureCenter { get { return new Vector2(Width / 2, Height / 2); } } // TODO: Guardar esse valor pra não calcular sempre

        // TODO: Refatorar?
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle collisionBox = new Rectangle();
                collisionBox.Width = BoundingBox.Width;
                collisionBox.Height = BoundingBox.Height;
                collisionBox.X = (int)Position.X + BoundingBox.X;
                collisionBox.Y = (int)Position.Y + BoundingBox.Y;
                return collisionBox;
            }
        }

        // TODO: Refatorar?
        public Rectangle CenteredCollisionBox
        {
            get
            {
                Rectangle collisionBox = new Rectangle();
                collisionBox.Width = BoundingBox.Width;
                collisionBox.Height = BoundingBox.Height;
                collisionBox.X = (int)(Position.X - TextureCenter.X) + BoundingBox.X;
                collisionBox.Y = (int)(Position.Y - TextureCenter.Y) + BoundingBox.Y;
                return collisionBox;
            }
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

        private Vector2 speed;
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Entity(Vector2 position)
        {
            this.Gravity = new Vector2(0, 20f);
            this.Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {

        }
    }
}
