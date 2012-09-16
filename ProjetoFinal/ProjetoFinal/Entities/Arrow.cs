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
        TextureManager textureManager = TextureManager.Instance;
        Camera camera = Camera.Instance;

        public float angle;
        public float lifeSpan;
        public float scale = 1f;
        public short OwnerId { get; private set; }
        public bool Collided { get; set; }

        float fadeDelay = 700;
        float fadeRate = 0.01f;
        public float alpha = 1;
        
        // TODO: Refatorar isso
        Vector2 textureCenterToBoundingBoxCenter = new Vector2();
        Rectangle newBoundingBox;
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
            angle = 0;
            baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Arrow), 1, 1);
            OwnerId = ownerId;
            speed = _speed;
            gravity = 0.2f;
            friction = 0.95f;
            BoundingBox = new Rectangle(19, 1, 5, 5);
            lifeSpan = 0;
            Entity.Entities.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            lifeSpan += gameTime.ElapsedGameTime.Milliseconds;

            if (!Collided)
            {
                if (OnGround() || MapCollideY(-1) || MapCollideX(1) || MapCollideX(-1))
                {
                    Collided = true;
                }
                else
                {
                    angle = (float)Math.Acos(Vector2.Dot(speed, Vector2.UnitX) / (speed.Length()));
                    speed.Y += gravity;
                    if (speed.Y < 0) angle = -angle;

                    MoveBy(speed);
                }
            }
            else
            {
                fadeDelay -= gameTime.ElapsedGameTime.Milliseconds;
                if (fadeDelay <= 0)
                    alpha -= fadeRate;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(baseAnimation.SpriteSheet, camera.WorldToCamera(position), null, Color.White * alpha, angle, baseAnimation.TextureCenter, scale, SpriteEffects.None, 0);
            //Util.DrawRectangle(spriteBatch, CollisionBox, 1, Color.Red);
        }
    }
}