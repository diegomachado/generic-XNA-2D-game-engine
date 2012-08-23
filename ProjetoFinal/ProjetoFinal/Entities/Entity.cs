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
        public int type,
                   flag;
        enum Type
        {
            Generic,
            Player
        }

        enum Flags
        {
            None = 0,
            Gravity = 0x00000001,
            Ghost = 0x00000002,
            MapOnly = 0x00000004
        }

        static List<Entity> Entities;

        public Animation baseAnimation;        
        public bool Dead { get; set; }                
        public double LastUpdateTime { get; set; }        
        public int Width { get { return baseAnimation.FrameSize.X; } }
        public int Height { get { return baseAnimation.FrameSize.Y; } }

        private Vector2 speed, maxSpeed, minSpeed;
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public Vector2 MinSpeed
        {
            get { return minSpeed; }
            set { minSpeed = value; }
        }
        public Vector2 MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
        public float SpeedX
        {
            get { return speed.X; }
            set
            {
                if (Math.Abs(value) < MinSpeed.X)
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
                if (Math.Abs(value) > MaxSpeed.Y)
                    speed.Y = MaxSpeed.Y;
                else
                    speed.Y = value;
            }
        }
        public bool isMovingHorizontally { get { return (speed.X == 0); } }
        public bool isMovingVertically { get { return (speed.Y == 0); } }
        public Vector2 Gravity { get; protected set; }
        public Vector2 Position { get; set; }

        // TODO: Guardar esses valores pra não calcular sempre
        // Refatorar isso pra flecha, acho que dá pra setar o eixo de rotação no meio do sprite e
        // eliminar esse código
        public Vector2 TextureCenter { get { return new Vector2(Width / 2, Height / 2); } } 
        public Vector2 Center { get { return Position + TextureCenter; } }        

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

        public Entity(Vector2 position)
        {
            minSpeed = new Vector2(30, 0);
            maxSpeed = new Vector2(500, 500);
            Gravity = new Vector2(0, 20f);
            Position = position;
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
