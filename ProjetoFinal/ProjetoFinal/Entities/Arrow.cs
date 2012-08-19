using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.Entities
{
    class Arrow : Entity
    {
        public short OwnerId { get; private set; }
        public Arrow(short ownerId, Texture2D playerSkin, Vector2 position, Rectangle boundingBox, Vector2 speed)
            : base(playerSkin, position, boundingBox)
        {
            this.Speed = speed;
            this.OwnerId = ownerId;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Acos(Vector2.Dot(Speed, Vector2.UnitX) / (Speed.Length()));

            if (SpeedY < 0)
                rotation = -rotation;

            // TODO: Tirar a conta Camera.Instance.Position - Position daqui e jogar ela dentro de Camera tipo: Camera.Instance.ScreenToCameraCoordinates(Position)
            // Refatorar esse new Vector2 ae
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, rotation, TextureCenter, 1f, SpriteEffects.None, 0f);
        }
    }
}