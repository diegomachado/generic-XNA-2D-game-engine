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
        public Arrow(Texture2D playerSkin, Vector2 position, Rectangle boundingBox, Vector2 speed)
            : base(playerSkin, position, boundingBox)
        {
            this.Speed = speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Acos(Vector2.Dot(Speed, Vector2.UnitX));

            // TODO: Tirar a conta Camera.Instance.Position - Position daqui e jogar ela dentro de Camera tipo: Camera.Instance.ScreenToCameraCoordinates(Position)
            // TODO: testar se alguma vez SpeedX == 0
            if (SpeedX > 0)
            {
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}