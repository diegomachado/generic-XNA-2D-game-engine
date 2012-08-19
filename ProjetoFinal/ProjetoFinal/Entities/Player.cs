using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers.LocalPlayerStates;
using ProjetoFinal.Managers;

namespace ProjetoFinal.Entities
{
    class Player : Entity
    {
        public Vector2 JumpForce { get; private set; }
        public Vector2 walkForce { get; private set; }
        public float Friction { get; private set; }

        public VerticalStateType LastVerticalState { get; set; }
        public HorizontalStateType LastHorizontalState { get; set; }
        public ActionStateType LastActionState { get; set; }
        public bool FacingLeft { get; set; }

        Vector2 weaponPosition;
        public Vector2 WeaponPosition
        {
            get
            {
                if (FacingLeft)
                {
                    // TODO: Refatorar saporra, pra não ficar criando Vector2 toda hora atoa
                    return new Vector2(Width - weaponPosition.X, Height - weaponPosition.Y) + Position;
                }
                else
                {
                    return weaponPosition + Position;
                }
            }
            private set
            {
                weaponPosition = value;
            }
        }

        public Player(Vector2 playerPosition)
            : base(playerPosition)
        {
            this.skin = TextureManager.Instance.getTexture(TextureList.Bear);
            this.walkForce = new Vector2(60, 0);
            this.JumpForce = new Vector2(0, -480f);
            this.WeaponPosition = new Vector2(29, 18);
            this.Friction = 0.85f;
            this.BoundingBox = new Rectangle(5, 1, 24, 30);
            this.LastVerticalState = VerticalStateType.Idle;
            this.LastHorizontalState = HorizontalStateType.Idle;
            this.LastActionState = ActionStateType.Idle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Tirar a conta Camera.Instance.Position - Position daqui e jogar ela dentro de Camera tipo: Camera.Instance.ScreenToCameraCoordinates(Position)
            if (FacingLeft)
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
