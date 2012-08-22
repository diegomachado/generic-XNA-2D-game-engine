using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjetoFinal.Managers;
using ProjetoFinal.Entities.Utils;

namespace ProjetoFinal.Entities
{
    class Entity
    {
        public Animation baseAnimation;

        //public Dictionary<string, Animation> animations;

        public Vector2 Gravity { get; protected set; }
        public Vector2 Position { get; set; }
        public double LastUpdateTime { get; set; }
        
        public int Width { get { return baseAnimation.FrameSize.X; } }
        public int Height { get { return baseAnimation.FrameSize.Y; } }
        public bool isMovingHorizontally { get { return (speed.X == 0); } }
        public bool isMovingVertically { get { return (speed.Y == 0); } }
        public Vector2 Center { get { return Position + TextureCenter; } } // TODO: Guardar esse valor pra não calcular sempre
        public Vector2 TextureCenter { get { return new Vector2(Width / 2, Height / 2); } } // TODO: Guardar esse valor pra não calcular sempre

        public Rectangle CollisionBox
        {
            get
            {
                Rectangle collisionBox = BoundingBox;
                collisionBox.Offset((int)Position.X, (int)Position.Y);
                return collisionBox;
            }
        }

        protected Rectangle boundingBox;
        public virtual Rectangle BoundingBox
        {
            get
            {
                return boundingBox;
            }
            protected set
            {
                boundingBox = value;
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

        public virtual void LoadContent()
        {
            baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Bear), 1, 1);
        }

        public virtual void Update()
        {
            baseAnimation.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            baseAnimation.Draw(spriteBatch, new Vector2(0,0));
        }

        public virtual void UnloadContent()
        {
        }
    }
}
