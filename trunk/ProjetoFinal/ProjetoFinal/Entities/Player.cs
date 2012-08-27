using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjetoFinal.Managers.LocalPlayerStates;
using ProjetoFinal.Managers;
using ProjetoFinal.Entities.Utils;

namespace ProjetoFinal.Entities
{
    class Player : Entity
    {
        public Vector2 JumpForce { get; private set; }
        public Vector2 WalkForce { get; private set; }
        public float Friction { get; private set; }

        public VerticalStateType VerticalState { get; set; }
        public HorizontalStateType HorizontalState { get; set; }
        public ActionStateType ActionState { get; set; }
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

        public Player(Vector2 playerPosition) : base(playerPosition)
        {
            this.baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Bear), 1, 1);
            this.MinSpeed = new Vector2(30, -500);
            this.MaxSpeed = new Vector2(500, 500);
            this.WalkForce = new Vector2(60, 0);
            this.JumpForce = new Vector2(0, -480f);
            this.WeaponPosition = new Vector2(29, 18);
            this.Friction = 0.85f;
            this.BoundingBox = new Rectangle(5, 1, 24, 30);
            this.VerticalState = VerticalStateType.Idle;
            this.HorizontalState = HorizontalStateType.Idle;
            this.ActionState = ActionStateType.Idle;

            Entity.Entities.Add(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FacingLeft)
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCameraCoordinates(Position), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCameraCoordinates(Position), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Util.DrawRectangle(spriteBatch, this.CollisionBox, 1, Color.Red);
            Util.DrawRectangle(spriteBatch, new Rectangle(this.CollisionBox.X, this.CollisionBox.Y, 24, 30), 1, Color.Yellow);
        }
    }
}
