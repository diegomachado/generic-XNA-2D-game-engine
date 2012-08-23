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
    class Arrow : Entity
    {
        float rotation;
        public short OwnerId { get; private set; }
        public bool Collided { get; set; }
        public float Timer { get; set; }
        public static float speedFactor = 1.0f,
                            gravityFactor = 0.5f;
                
        public override Rectangle BoundingBox
        {
            get
            {
                Rectangle newBoundingBox = base.BoundingBox;
                Vector2 textureCenterToBoundingBoxCenter = new Vector2(newBoundingBox.Center.X, newBoundingBox.Center.Y) - TextureCenter;

                textureCenterToBoundingBoxCenter = Vector2.Transform(textureCenterToBoundingBoxCenter, Matrix.CreateRotationZ(rotation));
                newBoundingBox.Location = Util.Vector2ToPoint(textureCenterToBoundingBoxCenter - 
                    new Vector2(newBoundingBox.Center.X - newBoundingBox.Left, newBoundingBox.Center.Y - newBoundingBox.Top));

                return newBoundingBox;
            }
        }

        public Arrow(short ownerId, Vector2 position, Vector2 speed) : base(position)
        {
            this.OwnerId = ownerId;
            this.Speed = speed;
            this.BoundingBox = new Rectangle(19, 1, 5, 5);
            this.baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Arrow), 1, 1);
            this.Timer = 0;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            rotation = (float)Math.Acos(Vector2.Dot(Speed, Vector2.UnitX) / (Speed.Length()));

            if (SpeedY < 0)
                rotation = -rotation;

            spriteBatch.Draw(baseAnimation.SpriteSheet, camera.WorldToCameraCoordinates(Position), null, Color.White, rotation, TextureCenter, 1f, SpriteEffects.None, 0f);
        }
    }
}