using System;
using System.Collections.Generic;
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
        public enum Type
        {
            Generic,
            Player,
            Arrow,
            Sword
        }

        [Flags]
        public enum Flags
        {
            /*
             *  Usage:
             *      if ((flags & Flags.MapOnly) == Flags.MapOnly) -> Has 
             *      if ((flags & ~Flags.MapOnly) == Flags.MapOnly) -> Has Not
             */

            None = 0,
            Gravity = 1,
            Ghost = 2,
            MapOnly = 4
        }

        static public List<Entity> Entities = new List<Entity>();

        public Type type;
        public Flags flags;
        public bool Dead { get; set; }                
        public Animation baseAnimation;                
        public double LastUpdateTime { get; set; }        
        public int Width { get { return baseAnimation.FrameSize.X; } }
        public int Height { get { return baseAnimation.FrameSize.Y; } }

        private Vector2 position;
        public Vector2 Position { get; set; }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                position.X = value;
            }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
            }
        }

        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }

        protected Vector2 speed, minSpeed, maxSpeed;
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

        public bool IsMovingHorizontally { get { return (speed.X == 0); } }
        public bool IsMovingVertically { get { return (speed.Y == 0); } }

        public Vector2 acceleration;
        public Vector2 Acceleration { get; set; }
        public float AccelerationX
        {
            get { return acceleration.X; }
            set
            {
                acceleration.X = value;
            }
        }
        public float AccelerationY
        {
            get { return acceleration.Y; }
            set
            {
                acceleration.Y = value;
            }
        }
        public Vector2 Gravity { get; protected set; }

        // TODO: Guardar esses valores pra não calcular sempre
        public Vector2 TextureCenter { get { return new Vector2(Width / 2, Height / 2); } } 
        public Vector2 Center { get { return Position + TextureCenter; } }        

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
        protected Rectangle collisionBox;
        public Rectangle CollisionBox
        {
            get
            {
                collisionBox = BoundingBox;
                collisionBox.Offset((int)Position.X, (int)Position.Y);
                return collisionBox;
            }
        }

        public Entity(Vector2 position)
        {
            type = Type.Generic;
            flags = Flags.Gravity;
            Dead = false;
            Position = position;
            MoveLeft = false;
            MoveRight = false;            
            speed = Vector2.Zero;
            minSpeed = new Vector2(0, 0);
            maxSpeed = new Vector2(0, 0);
            Acceleration = Vector2.Zero;
            Gravity = new Vector2(0, 20f);            
        }

        ~Entity()
        {
            Entity.Entities.Remove(this);
        }

        public virtual void LoadContent()
        {
            baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Bear), 1, 1);
        }

        public virtual void Update()
        {
            if (!MoveLeft && !MoveRight)
                StopMove();

            if (MoveLeft)
                AccelerationX = -0.5f;
            else if (MoveRight)
                AccelerationX = 0.5f;

            if ((flags & Flags.Gravity) == Flags.Gravity)
                AccelerationY = 0.75f;

            baseAnimation.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            baseAnimation.Draw(spriteBatch, new Vector2(0,0));
        }

        // TODO: Generalizar pra qualquer Layer?
        public void OnMove(Vector2 offset)
        {
            if (offset == Vector2.Zero) return;

            /*
                double NewX = 0;
                double NewY = 0;
 
                MoveX *= CFPS::FPSControl.GetSpeedFactor();
                MoveY *= CFPS::FPSControl.GetSpeedFactor();
 
                if(MoveX != 0) {
                    if(MoveX >= 0)     NewX =  CFPS::FPSControl.GetSpeedFactor();
                    else             NewX = -CFPS::FPSControl.GetSpeedFactor();
                }
 
                if(MoveY != 0) {
                    if(MoveY >= 0)     NewY =  CFPS::FPSControl.GetSpeedFactor();
                    else             NewY = -CFPS::FPSControl.GetSpeedFactor();
                }
            */

            Vector2 newPosition = Position + offset;            
            Point newPositionPoint = new Point((int)newPosition.X, (int)newPosition.Y);

            while (true)
            {
                if ((flags & Flags.Ghost) == Flags.Ghost)
                {
                    PositionValidOnMap(newPositionPoint);
                    Position += newPosition;
                }
                else
                {
                    if(PositionValidOnMap(new Point(newPositionPoint.X, (int)Position.Y)))
                        PositionX += newPosition.X;
                    else
                        SpeedX = 0;

                    if(PositionValidOnMap(new Point((int)Position.X, newPositionPoint.Y)))
                        PositionY += newPosition.Y;
                    else
                        SpeedY = 0;
                }

                offset -= newPosition;

                if(newPosition.X > 0 && offset.X <= 0) newPosition.X = 0;
                if(newPosition.X < 0 && offset.X >= 0) newPosition.X = 0;
                if(newPosition.Y > 0 && offset.Y <= 0) newPosition.Y = 0;
                if(newPosition.Y < 0 && offset.Y >= 0) newPosition.Y = 0;
                if(offset.X == 0) newPosition.X = 0;
                if(offset.Y == 0) newPosition.Y = 0;
                if(offset.X == 0 && offset.Y == 0) break;
                if(newPosition.X == 0 && newPosition.Y == 0) break;
            }

        }
        public void StopMove()
        {
            if (SpeedX > 0)
                AccelerationX = -1;

            if (SpeedX < 0)
                AccelerationX = 1;

            if (SpeedX < 2.0f && SpeedX > -2.0f)
            {
                AccelerationX = 0;
                SpeedX = 0;
            }
        }

        public bool PositionValidOnMap(Point offset)
        {
            bool valid = true;
            Point tileSize = MapManager.Instance.TileSize;

            int startX  = (offset.X + CollisionBox.X) / tileSize.X;
            int endX    = ((offset.X + CollisionBox.X) + Width - CollisionBox.Width - 1) / tileSize.X;
            int startY  = (offset.Y + CollisionBox.Y) / tileSize.Y;            
            int endY    = ((offset.Y + CollisionBox.Y) + Width - CollisionBox.Width - 1) / tileSize.Y;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    Point tilePosition = new Point(x * tileSize.X, y * tileSize.Y);

                    if (!MapManager.Instance.CollisionLayer.TileIdByPixelPosition(tilePosition))
                        valid = false;
                }
            }

            if((flags & ~Flags.MapOnly) == Flags.MapOnly)
            {
                foreach(Entity entity in Entities)
                    if (!PositionValidOnEntity(entity, offset))
                        valid = false;                
            }

            return valid;
        }
        public bool PositionValidTile(Tile tile)
        {            
            return true;
        }
        public bool PositionValidOnEntity(Entity entity, Point offset)
        {
            Rectangle collisionBoxOffset = entity.CollisionBox;
            collisionBoxOffset.Offset(offset);

            if (this != entity && !entity.Dead && entity.Collides(collisionBoxOffset))
            {
                EntityCollision entityCollision = new EntityCollision(this, entity);
                EntityCollision.EntityCollisions.Add(entityCollision);

                return false;
            }

            return true;
        }

        public bool Collides(Rectangle collisionBox)
        {
            return CollisionBox.Intersects(collisionBox);
        }
        public virtual bool OnCollision(Entity entity)
        {
            return true;
        }        
    }
}
