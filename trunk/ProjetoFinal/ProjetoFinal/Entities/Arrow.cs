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
    class Arrow : DynamicEntity
    {
        bool isFlying = false;
        Vector2 direction;
        float scaling = 0.1f;

        float angle;
        public short OwnerId { get; private set; }
        public bool Collided { get; set; }
        public float Timer { get; set; }

        Vector2 textureCenterToBoundingBoxCenter = new Vector2();
        Rectangle newBoundingBox;

        TextureManager textureManager = TextureManager.Instance;
        Camera camera = Camera.Instance;
                
        public override Rectangle BoundingBox
        {
            get
            {
                newBoundingBox = base.BoundingBox;
                textureCenterToBoundingBoxCenter.X = newBoundingBox.Center.X - baseAnimation.TextureCenter.X;
                textureCenterToBoundingBoxCenter.Y = newBoundingBox.Center.Y - baseAnimation.TextureCenter.Y;                
                textureCenterToBoundingBoxCenter = Vector2.Transform(textureCenterToBoundingBoxCenter, Matrix.CreateRotationZ(angle));
                newBoundingBox.Location = Util.Vector2ToPoint(textureCenterToBoundingBoxCenter - 
                    new Vector2(newBoundingBox.Center.X - newBoundingBox.Left, newBoundingBox.Center.Y - newBoundingBox.Top));

                return newBoundingBox;
            }
        }

        public Arrow(short ownerId, Vector2 position, Vector2 _speed) : base(position)
        {
            origin = new Vector2(11, 3);
            baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Arrow), 1, 1);
            OwnerId = ownerId;
            speed = _speed;
            gravity = 0.2f;
            friction = 0.95f;
            BoundingBox = new Rectangle(19, 1, 5, 5);
            Timer = 0;
            Entity.Entities.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (OnGround() || MapCollideY(-1)) speed.Y = 0;
            speed.Y += gravity;
            
            if(OnGround())
                speed.X *= friction;

            MoveBy(speed);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            angle = (float)Math.Acos(Vector2.Dot(speed, Vector2.UnitX) / (speed.Length()));

            if (speed.Y < 0)
                angle = -angle;

            spriteBatch.Draw(baseAnimation.SpriteSheet, camera.WorldToCamera(position + origin), null, Color.White, angle, baseAnimation.TextureCenter, 1f, SpriteEffects.None, 0f);
            Util.DrawRectangle(spriteBatch, CollisionBox, 1, Color.Red);            
        }
    }
}