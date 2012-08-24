using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjetoFinal.Managers;
using ProjetoFinal.Entities.Utils;
using OgmoLibrary;

namespace ProjetoFinal.Entities
{
    class Entity
    {
        enum Type
        {
            Generic,
            Player,
            Arrow,
            Sword
        }

        [Flags] enum Flags
        {            
            None = 0x0,
            Gravity = 0x1,
            Ghost = 0x2,
            MapOnly = 0x4

            /* 
            * Usage:
            *      Modifiers flags = Modifiers.Gravity | Modifiers.MapOnly | Modifiers.Ghost
            *      if ((flags & Modifiers.MapOnly))
            *          DoStuff();
            */
        }
        static List<Entity> Entities;

        public int type,flags;
        public bool Dead { get; set; }                
        public Animation baseAnimation;                
        public double LastUpdateTime { get; set; }        
        public int Width { get { return baseAnimation.FrameSize.X; } }
        public int Height { get { return baseAnimation.FrameSize.Y; } }
        public bool IsMovingHorizontally { get { return (speed.X == 0); } }
        public bool IsMovingVertically { get { return (speed.Y == 0); } }
        private Vector2 speed, minSpeed, maxSpeed;
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
        public Vector2 acceleration;
        public Vector2 Acceleration { get; set; }
        public Vector2 Gravity { get; protected set; }
        public Vector2 Position { get; set; }

        // TODO: Guardar esses valores pra não calcular sempre
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
            Type type = Type.Generic;
            Flags flags = Flags.Gravity;
            Dead = false;

            speed = Vector2.Zero;
            minSpeed = new Vector2(30, -500);
            maxSpeed = new Vector2(5, 500);
            acceleration = Vector2.Zero;

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

        public bool Collides(Rectangle collisionBox)
        {
            return CollisionBox.Intersects(collisionBox);
        }

        public virtual void OnCollision(Entity entity)
        {
        }        
    }

    // TODO: Extrair pra classe própria
    class EntityCollision
    {
        public static List<EntityCollision> EntityCollisions;

        public Entity entityA, entityB;

        public EntityCollision()
        {
        }        
    }
}
