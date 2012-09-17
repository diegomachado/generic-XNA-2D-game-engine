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
    class Player : DynamicEntity
    {
        public VerticalStateType VerticalState { get; set; }
        public HorizontalStateType HorizontalState { get; set; }
        public ActionStateType ActionState { get; set; }
        public bool FacingRight { get; set; }

        public double LastUpdateTime;

        Vector2 weaponPosition;
        public Vector2 WeaponPosition
        {
            get
            {
                // TODO: Refatorar saporra, pra não ficar criando Vector2 toda hora atoa
                if (FacingRight)
                    return weaponPosition + position;
                else
                    return new Vector2(Width - weaponPosition.X, Height - weaponPosition.Y) + position;
            }
            private set
            {
                weaponPosition = value;
            }
        }

        public Player(Vector2 _position) : base(_position)
        {
            boundingBox = new Rectangle(8, 2, 10, 30);
            moveSpeed = 2f;
            gravity = 0.5f;
            friction = 0.8f;
            jumpSpeed = -10.5f;
            minSpeed = new Vector2(-15, -12);
            maxSpeed = new Vector2(15, 12);     
            WeaponPosition = new Vector2(29, 18);
            VerticalState = VerticalStateType.Idle;
            HorizontalState = HorizontalStateType.Idle;
            ActionState = ActionStateType.Idle;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Ajeitar Bug ao flippar próximo das paredes
            if (FacingRight)
                boundingBox.X = 8;
            else
                boundingBox.X = 14;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FacingRight)
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCamera(position), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else                                                                                                                          
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCamera(position), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);

            Util.DrawRectangle(spriteBatch, this.CollisionBox, 1, Color.Red);
        }

        public override bool OnCollision(Entity entity)
        {
            if (entity is Arrow)
                if (entity.active)
                    Console.WriteLine("Tomei uma flechada frenética!");
            
            return true;
        }
    }
}
