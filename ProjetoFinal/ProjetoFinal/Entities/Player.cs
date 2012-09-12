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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FacingRight)
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCamera(position + origin), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(baseAnimation.SpriteSheet, Camera.Instance.WorldToCamera(position + origin), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

            // TODO: Flippar a boudingBox junto
            //Util.DrawRectangle(spriteBatch, this.CollisionBox, 1, Color.Red);
        }

        // Passar os states pra dentro de player???
        /*public void DrawDebug(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(), new Rectangle(0, 0, 230, 170), new Color(0, 0, 0, 0.2f));
            spriteBatch.DrawString(spriteFont, "" + localPlayerHorizontalState, new Vector2(localPlayer.position.X + 8, localPlayer.position.Y - 20) - camera.Position, Color.White);
            spriteBatch.DrawString(spriteFont, "" + localPlayerVerticalState, new Vector2(localPlayer.position.X + 8, localPlayer.position.Y - 40) - camera.Position, Color.White);
            spriteBatch.DrawString(spriteFont, "X: " + (int)localPlayer.position.X, new Vector2(5f, 05f), Color.White);
            spriteBatch.DrawString(spriteFont, "Y: " + (int)localPlayer.position.Y, new Vector2(5f, 25f), Color.White);
            spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)localPlayer.speed.X, new Vector2(5f, 45f), Color.White);
            spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)localPlayer.speed.Y, new Vector2(5f, 65f), Color.White);
            spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)camera.Position.X, new Vector2(5f, 85f), Color.White);
            spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)camera.Position.Y, new Vector2(5f, 105f), Color.White);
            spriteBatch.DrawString(spriteFont, "Horizontal State: " + localPlayerHorizontalState, new Vector2(5f, 125f), Color.White);
            spriteBatch.DrawString(spriteFont, "Vertical State: " + localPlayerVerticalState, new Vector2(5f, 145f), Color.White);
        }*/
    }
}
