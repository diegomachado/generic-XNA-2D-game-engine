using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers;
using ProjetoFinal.PlayerStateMachine.MovementStates.VerticalMovementStates;
using ProjetoFinal.PlayerStateMachine.MovementStates.HorizontalMovementStates;
using ProjetoFinal.PlayerStateMachine.ActionStates;
using ProjetoFinal.Entities.Utils;
using ProjetoFinal.EventHeaders;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Entities
{
    class Player : DynamicEntity
    {
        // TODO: isso não ta bom
        EventManager eventManager = EventManager.Instance;
        NetworkManager networkManager = NetworkManager.Instance;

        public short id;
        public int health;

        public double lastUpdateTime;
        public bool FacingRight { get; set; }
        public VerticalMovementState VerticalState { get; set; }
        public HorizontalMovementState HorizontalState { get; set; }
        public ActionState ActionState { get; set; }

        public bool isInvencible;
        private const double respawnDuration = 5.0f;
        private double respawnCooldown = 0;
        private float alpha;
        public bool IsDead { get; private set; }

        public HorizontalStateType HorizontalStateType { get; set; }
        public VerticalStateType VerticalStateType { get; set; }
        public ActionStateType ActionStateType { get; set; }

        private Vector2 weaponPosition;
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

        public Player(short _id, Vector2 _position) : base(_position)
        {
            id = _id;
            health = 100;
            boundingBox = new Rectangle(10, 0, 14, 30);
            moveSpeed = 2f;
            gravity = 0.5f;
            friction = 0.8f;
            jumpSpeed = -10.5f;
            minSpeed = new Vector2(-15, -12);
            maxSpeed = new Vector2(15, 12);     
            WeaponPosition = new Vector2(29, 18);
            alpha = 1;
            lastUpdateTime = -99999;
            HorizontalState = PlayerStates.horizontalStates[HorizontalStateType.Idle];
            VerticalState = PlayerStates.verticalStates[VerticalStateType.Idle];
            ActionState = PlayerStates.actionStates[ActionStateType.Idle];
        }

        // TODO: Passar isso na hora que constrói o player, pra dar flexibilidade
        public override void LoadContent()
        {
            spriteMap = new SpriteMap(TextureManager.Instance.getTexture(TextureList.Bear), 34, 30);
            spriteMap.Add("idle", new int[] { 0, 1, 2, 3 }, 10);
            spriteMap.Add("moving", new int[] { 4, 5, 6, 7 }, 10).Play();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (respawnCooldown > 0)
            {
                respawnCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

                alpha += 0.2f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                isInvencible = false;
                alpha = 1;
            }

            if (health <= 0)
                IsDead = true;

            ChooseAnimation();
            
            spriteMap.Update(gameTime);
            base.Update(gameTime);
        }

        private void ChooseAnimation()
        {
            if (HorizontalStateType == HorizontalStateType.Idle)
            {
                spriteMap.Play("idle");
            }
            else if (HorizontalStateType == HorizontalStateType.WalkingLeft || HorizontalStateType == HorizontalStateType.WalkingRight)
            {
                if (VerticalStateType == VerticalStateType.Jumping)
                {
                    spriteMap.Play("idle");
                }
                else
                {
                    spriteMap.Play("moving");
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteMap.Draw(spriteBatch, Camera.Instance.WorldToCamera(position), !FacingRight, 1);
            //spriteMap.Draw(spriteBatch, Camera.Instance.WorldToCamera(position), !FacingRight, alpha);

            //if(!isInvencible)
            DrawHealthBar(spriteBatch);

            //Util.DrawRectangle(spriteBatch, this.CollisionBox, 1, Color.Red);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
           spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(),
                            new Rectangle((int)position.X + spriteMap.frameWidth/2 - (int)Camera.Instance.Position.X - health/8,
                            (int)position.Y - 7 - (int)Camera.Instance.Position.Y,
                            health/4, 3),
                            Color.Red);
        }

        public void DrawArrowPower(SpriteBatch spriteBatch, float shootingTimer, Camera camera)
        {
            if (shootingTimer != 0)
                if (FacingRight)
                {
                    spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(),
                                     new Rectangle((int)position.X - 5 - (int)camera.Position.X,
                                                   (int)position.Y + Height - (int)camera.Position.Y - (int)shootingTimer / 35,
                                                   5, (int)shootingTimer / 35),
                                     Color.Yellow);
                }
                else
                {
                    spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(),
                                     new Rectangle((int)position.X + 30 - (int)camera.Position.X,
                                         (int)position.Y + Height - (int)camera.Position.Y - (int)shootingTimer / 35,
                                         5, (int)shootingTimer / 35), 
                                     Color.Yellow);
                }
        }

        public void Respawn(Vector2 respawnPoint)
        {
            IsDead = false;
            health = 100;
            respawnCooldown = respawnDuration;
            isInvencible = true;
            alpha = 0;
            position = respawnPoint;
        }

        public void TakeHit()
        {
            health -= 20;
        }

        public override bool OnCollision(Entity entity)
        {
            if (entity is Arrow && id == networkManager.LocalPlayerId && !(respawnCooldown > 0))
            {
                if (((Arrow)entity).ownerId != id)
                {
                    eventManager.ThrowPlayerHit(this, new PlayerHitEventArgs(id, ((Arrow)entity).ownerId));
                    TakeHit();
                    return true;
                }               
            }             
            return false;  
        }

    }
}