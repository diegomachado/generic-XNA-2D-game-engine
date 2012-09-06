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
        float rotation;
        public short OwnerId { get; private set; }
        public bool Collided { get; set; }
        public float Timer { get; set; }
        public static float speedFactor = 1.0f,
                            gravityFactor = 0.5f;

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
                textureCenterToBoundingBoxCenter = Vector2.Transform(textureCenterToBoundingBoxCenter, Matrix.CreateRotationZ(rotation));
                newBoundingBox.Location = Util.Vector2ToPoint(textureCenterToBoundingBoxCenter - 
                    new Vector2(newBoundingBox.Center.X - newBoundingBox.Left, newBoundingBox.Center.Y - newBoundingBox.Top));

                return newBoundingBox;
            }
        }

        public Arrow(short ownerId, Vector2 position, Vector2 speed) : base(position)
        {
            this.OwnerId = ownerId;
            this.speed = speed;
            this.BoundingBox = new Rectangle(19, 1, 5, 5);
            this.baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Arrow), 1, 1);
            this.Timer = 0;
            Entity.Entities.Add(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            rotation = (float)Math.Acos(Vector2.Dot(speed, Vector2.UnitX) / (speed.Length()));

            if (speed.Y < 0)
                rotation = -rotation;

            spriteBatch.Draw(baseAnimation.SpriteSheet, camera.WorldToCameraCoordinates(position), null, Color.White, rotation, baseAnimation.TextureCenter, 1f, SpriteEffects.None, 0f);
        }
    }
}