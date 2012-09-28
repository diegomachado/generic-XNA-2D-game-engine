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
        public int health;
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
            health = 100;
            boundingBox = new Rectangle(10, 0, 14, 30);
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
            if (health <= 0) Console.WriteLine("Morri :(");
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FacingRight)
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCamera(position), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else                                                                                                                          
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCamera(position), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);

            DrawHealthBar(spriteBatch);
            //Util.DrawRectangle(spriteBatch, this.CollisionBox, 1, Color.Red);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
           spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(),
                            new Rectangle((int)position.X + Width/2 - (int)Camera.Instance.Position.X - health/8,
                            (int)position.Y - 7 - (int)Camera.Instance.Position.Y,
                            health/4, 3),
                            Color.Red);
        }

        public void DrawArrowPower(SpriteBatch spriteBatch, float shootingTimer, Camera camera)
        {
            if (shootingTimer != 0)
                if (FacingRight)
                    spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(),
                                     new Rectangle((int)position.X - 5 - (int)camera.Position.X,
                                                   (int)position.Y + Height - (int)camera.Position.Y - (int)shootingTimer / 35,
                                                   5, (int)shootingTimer / 35),
                                     Color.Yellow);
                else
                    spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(),
                                     new Rectangle((int)position.X + 30 - (int)camera.Position.X,
                                         (int)position.Y + Height - (int)camera.Position.Y - (int)shootingTimer / 35,
                                         5, (int)shootingTimer / 35), 
                                     Color.Yellow);
        }

        public override bool OnCollision(Entity entity)
        {
            if (entity is Arrow)
            { 
                health -= 1;
                Console.WriteLine(health);
                return true;
            }

            return false;            
        }        
    }
}
