using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities.Utils;
using OgmoEditorLibrary;

namespace ProjetoFinal.Entities
{
    // TODO: Setar todo mundo que não muda como CONST
    class Entity
    {
        [Flags]
        public enum Flags
        {
            None = 0,
            Gravity = 1,
            Ghost = 2,
            MapOnly = 4
        }
        public Flags flags;

        static public List<Entity> Entities = new List<Entity>();

        public SpriteMap spriteMap;

        public bool visible = true;
        public bool collidable = true;
        public bool active = true;

        public float angle;
        public Vector2 position;
        public float scale; 
     
        public int Width      { get { return BoundingBox.Width; } }
        public int Height     { get { return BoundingBox.Height; } }
        public Vector2 Center { get { return position + new Vector2(Width / 2, Height / 2); } }        

        protected Rectangle boundingBox;
        public virtual Rectangle BoundingBox
        {
            get { return boundingBox; }
            protected set { boundingBox = value; }
        }
        protected Rectangle collisionBox;
        public Rectangle CollisionBox
        {
            get
            {
                collisionBox = BoundingBox;
                collisionBox.Offset((int)position.X, (int)position.Y);
                return collisionBox;
            }
        }

        public Entity(Vector2 _position, Rectangle _boundingBox = new Rectangle())
        {
            flags = Flags.None;
            angle = 0;
            position = _position;
            scale = 1;
            boundingBox = _boundingBox;
            Entities.Add(this);
        }

        ~Entity()
        {
            Entity.Entities.Remove(this);
        }

        #region Game Logic

        public virtual void LoadContent(){}

        public virtual void Update(GameTime gameTime)
        {
            if (active)
            {
                for (int i = 0; i < Entities.Count; i++)
                {
                    if (Entities[i] == this || !Entities[i].active)
                        continue;

                    if (this.Collides(Entities[i]))
                        entityCollision = new EntityCollision(this, Entities[i]);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch){}

        #endregion

        #region Collision

        EntityCollision entityCollision;
        private int sign;
        private Point corner1, corner2;
        Vector2 moveTemp = Vector2.Zero;
        float moveTempX = 0;
        float moveTempY = 0;

        public void MoveXBy(float moveAmountX)
        {
            if (moveAmountX == 0) return;

            moveTempX += moveAmountX;
            moveAmountX = (float)Math.Round(moveAmountX, MidpointRounding.AwayFromZero);
            moveTempX -= moveAmountX;

            if ((flags & ~Flags.Ghost) == Flags.Ghost)
            {
                position.X += moveAmountX;
            }
            else
            {
                if (moveAmountX != 0)
                {
                    sign = Math.Sign(moveAmountX);

                    while (moveAmountX != 0)
                    {
                        if (MapCollideX(sign)) break;
                        else position.X += sign;
                        moveAmountX -= sign;
                    }
                }
            }
        }

        public void MoveYBy(float moveAmountY)
        {
            if (moveAmountY == 0) return;

            moveTempY += moveAmountY;
            moveAmountY = (float)Math.Round(moveAmountY, MidpointRounding.AwayFromZero);
            moveTempY -= moveAmountY;

            if ((flags & ~Flags.Ghost) == Flags.Ghost)
            {
                position.Y += moveAmountY;
            }
            else
            {
                if (moveAmountY != 0)
                {
                    sign = Math.Sign(moveAmountY);

                    while (moveAmountY != 0)
                    {
                        if (MapCollideY(sign)) break;
                        else position.Y += sign;
                        moveAmountY -= sign;
                    }
                }
            }
        }

        public void MoveBy(Vector2 moveAmount)
        {
            if (moveAmount == Vector2.Zero) return;

            moveTemp += moveAmount;
            moveAmount.X = (float)Math.Round(moveAmount.X, MidpointRounding.AwayFromZero);
            moveAmount.Y = (float)Math.Round(moveAmount.Y, MidpointRounding.AwayFromZero);
            moveTemp -= moveAmount;

            if ((flags & ~Flags.Ghost) == Flags.Ghost)
            {
                position += moveAmount;
            }
            else
            {
                if (moveAmount.X != 0)
                {
                    sign = Math.Sign(moveAmount.X);

                    while (moveAmount.X != 0)
                    {
                        if (MapCollideX(sign)) break;
                        else position.X += sign;
                        moveAmount.X -= sign;
                    }
                }
                if (moveAmount.Y != 0)
                {
                    sign = Math.Sign(moveAmount.Y);

                    while (moveAmount.Y != 0)
                    {
                        if (MapCollideY(sign)) break;
                        else position.Y += sign;
                        moveAmount.Y -= sign;
                    }
                }
            }
        }

        public bool MapCollideX(float moveAmount)
        {
            if (moveAmount < 0)
            {
                corner1.X = CollisionBox.Left;
                corner1.Y = CollisionBox.Top + 1;
                corner2.X = CollisionBox.Left;
                corner2.Y = CollisionBox.Bottom - 1;
            }
            else
            {
                corner1.X = CollisionBox.Right;
                corner1.Y = CollisionBox.Top + 1;
                corner2.X = CollisionBox.Right;
                corner2.Y = CollisionBox.Bottom - 1;
            }

            return (TilePixelCollision(corner1.X, corner1.Y) || TilePixelCollision(corner2.X, corner2.Y));
        }

        public bool MapCollideY(float moveAmount)
        {
            if (moveAmount < 0)
            {
                corner1.X = CollisionBox.Left + 1;
                corner1.Y = CollisionBox.Top;
                corner2.X = CollisionBox.Right - 1;
                corner2.Y = CollisionBox.Top;
            }
            else
            {
                corner1.X = CollisionBox.Left + 1;
                corner1.Y = CollisionBox.Bottom;
                corner2.X = CollisionBox.Right - 1;
                corner2.Y = CollisionBox.Bottom;
            }

            return (TilePixelCollision(corner1.X, corner1.Y) || TilePixelCollision(corner2.X, corner2.Y));
        }        

        private bool TileCollision(int x, int y)
        {
            return (LevelManager.Instance.Grid.TileAt(x, y));
        }

        private bool TilePixelCollision(int x, int y)
        {
            x = x / LevelManager.Instance.Grid.tile.Width;
            y = y / LevelManager.Instance.Grid.tile.Height;
            return (LevelManager.Instance.Grid.TileAt(x,y));
        }        

        public bool Collides(Entity entity)
        {
            return CollisionBox.Intersects(entity.CollisionBox);
        }

        public virtual bool OnCollision(Entity entity)
        {
            return true;
        }

        #endregion
    }
}
