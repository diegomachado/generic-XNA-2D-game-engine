using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.Entities
{
    class Entity
    {
        protected Camera camera = Camera.Instance;
        protected Texture2D skin;
        
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Vector2 Gravity { get; set; }
        public double LastUpdateTime { get; set; }
        public int Width { get { return skin.Width; } }
        public int Height { get { return skin.Height; } }
        public bool isMovingHorizontally { get { return (speed.X == 0); } }
        public bool isMovingVertically { get { return (speed.Y == 0); } }
        public Vector2 Center { get { return Position + TextureCenter; } } // TODO: Guardar esse valor pra não calcular sempre
        public Vector2 TextureCenter { get { return new Vector2(Width / 2, Height / 2); } } // TODO: Guardar esse valor pra não calcular sempre

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

        public Entity(Texture2D texture, Vector2 position, Rectangle boundingBox)
        {
            Gravity = new Vector2(0, 20f);
            skin = texture;
            Position = position;
            BoundingBox = boundingBox;
            collisionBox = boundingBox;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
