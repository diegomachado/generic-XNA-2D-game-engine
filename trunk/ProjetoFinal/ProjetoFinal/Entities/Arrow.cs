using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjetoFinal.Managers;

namespace ProjetoFinal.Entities
{
    class Arrow : Entity
    {
        public short OwnerId { get; private set; }
        public Arrow(short ownerId, Vector2 position, Vector2 speed)
            : base(position)
        {
            this.OwnerId = ownerId;
            this.Speed = speed;
            this.BoundingBox = new Rectangle(17, 1, 8, 5);
            this.skin = TextureManager.Instance.getTexture(TextureList.Arrow);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            float rotation = (float)Math.Acos(Vector2.Dot(Speed, Vector2.UnitX) / (Speed.Length()));

            if (SpeedY < 0)
                rotation = -rotation;

            spriteBatch.Draw(skin, camera.WorldToCameraCoordinates(Position), null, Color.White, rotation, TextureCenter, 1f, SpriteEffects.None, 0f);
        }
    }
}