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

        public float lifeSpan;
        public short ownerId;
        public bool Collided { get; set; }

        float fadeDelay = 700;
        float fadeRate = 0.01f;
        public float alpha = 1;
        
        // TODO: Refatorar Rotação da Flecha
        Vector2 textureCenterToBoundingBoxCenter = new Vector2();
        Rectangle newBoundingBox;
        public override Rectangle BoundingBox
        {
            get
            {
                newBoundingBox = base.BoundingBox;
                textureCenterToBoundingBoxCenter.X = newBoundingBox.Center.X - spriteMap.width/2;
                textureCenterToBoundingBoxCenter.Y = newBoundingBox.Center.Y - spriteMap.height/2;                
                textureCenterToBoundingBoxCenter = Vector2.Transform(textureCenterToBoundingBoxCenter, Matrix.CreateRotationZ(angle));
                newBoundingBox.Location = Util.Vector2ToPoint(textureCenterToBoundingBoxCenter - 
                    new Vector2(newBoundingBox.Center.X - newBoundingBox.Left, newBoundingBox.Center.Y - newBoundingBox.Top));

                return newBoundingBox;
            }
        }

        public Arrow(short _ownerId, Vector2 position, Vector2 _speed) : base(position)
        {
            spriteMap = new SpriteMap(TextureManager.Instance.getTexture(TextureList.Arrow), 25, 7);
            ownerId = _ownerId;
            speed = _speed;
            gravity = 0.2f;
            friction = 0.95f;
            BoundingBox = new Rectangle(19, 1, 5, 5);
            lifeSpan = 3000;
        }

        public override void LoadContent()
        {
            spriteMap.Add("base", new int[0], 1, true);
        }

        public override void Update(GameTime gameTime)
        {
            lifeSpan -= gameTime.ElapsedGameTime.Milliseconds;
            
            if (!Collided)
            {
                if (OnGround() || MapCollideY(-1) || MapCollideX(1) || MapCollideX(-1))
                {
                    Collided = true;
                    active = false;
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

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteMap.Draw(spriteBatch, camera.WorldToCamera(position), alpha, angle, scale);
            //Util.DrawRectangle(spriteBatch, CollisionBox, 1, Color.Red);
        }

        public override bool OnCollision(Entity entity)
        {
            if (entity is Player)
            { 
                if (((Player)entity).id != ownerId)
                {
                    active = false;
                    Collided = true;
                    return true;
                }
            }
            return false;
        }
    }
}