using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.EventArgs;
using ProjetoFinal.Entities;

using OgmoLibrary;

namespace ProjetoFinal.Managers
{
    #region StatesAndShitCommented
    public enum MovementState
    {
        Idle,

        WalkingLeft,
        WalkingRight,
        WalkingDead,

        Jumping,
        JumpingLeft,
        JumpingRight,

        Falling,
        FallingLeft,
        FallingRight
    }

    public enum ActionState
    {
        Idle,
        Striking,
        Shooting        
    }
    #endregion

    class LocalPlayerManager
    {
        public short playerId { get; set; }
        
        Player localPlayer;
        Vector2 acceleration = Vector2.Zero;
        Vector2 moveAmount = Vector2.Zero;

        KeyboardState lastKeyboardState;
        
        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayerManager()
        {
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(96, 240), new Rectangle(6, 2, 24, 30));
            //localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(96, 240), new Rectangle(6, 2, 24, 30), new Vector2(0f, 0.1f));        
        }
        
        protected void OnPlayerStateChanged(PlayerState playerState)
        {
            localPlayer.State = playerState;

            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(playerId, localPlayer));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Layer collisionLayer)
        {
            if (localPlayer != null)
            {
                localPlayer.CollisionBox.X = (int)localPlayer.Position.X + localPlayer.BoundingBox.X;
                localPlayer.CollisionBox.Y = (int)localPlayer.Position.Y + localPlayer.BoundingBox.Y;

                localPlayer.NextPosition = localPlayer.CollisionBox;
                localPlayer.NextPosition.Offset(20, 20);
                
                acceleration = Vector2.Zero;

                localPlayer.OnGround = (moveAmount.Y == 0) ? true : false;

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Left))
                        OnPlayerStateChanged(PlayerState.WalkingLeft);

                    acceleration -= localPlayer.Speed;
                }

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Right))
                        OnPlayerStateChanged(PlayerState.WalkingRight);

                    acceleration += localPlayer.Speed;
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (localPlayer.OnGround)
                    {
                        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                            OnPlayerStateChanged(PlayerState.Jumping);

                        if (keyboardState.IsKeyDown(Keys.Left))
                            OnPlayerStateChanged(PlayerState.JumpingLeft);
                        else if (keyboardState.IsKeyDown(Keys.Right))
                            OnPlayerStateChanged(PlayerState.JumpingRight);
                        else
                            OnPlayerStateChanged(PlayerState.Jumping);

                        acceleration += localPlayer.JumpForce;
                    }
                }

                if (!keyboardState.IsKeyDown(Keys.Space) && !keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
                    OnPlayerStateChanged(PlayerState.Idle);
            
                acceleration += localPlayer.Gravity;  
                moveAmount += acceleration;

                moveAmount.X *= localPlayer.Friction; 
                moveAmount.Y = limitFallSpeed(10, moveAmount);

                moveAmount = horizontalCollisionTest(moveAmount, collisionLayer);
                moveAmount = verticalCollisionTest(moveAmount, collisionLayer);

                localPlayer.Position += moveAmount;
                lastKeyboardState = keyboardState;

                Console.WriteLine(localPlayer.Position.ToString());
            }
        }

        private float limitFallSpeed(float speedLimit, Vector2 moveAmount)
        {
            if (moveAmount.Y >= speedLimit)
                moveAmount.Y = speedLimit;

            return moveAmount.Y;
        }

        private Vector2 horizontalCollisionTest(Vector2 moveAmount, Layer collisionLayer)
        {
            Point corner1, corner2;
            Rectangle nextPosition = localPlayer.CollisionBox;

            if (moveAmount.X == 0)
                return moveAmount;

            nextPosition.Offset((int)moveAmount.X, 0);

            if (moveAmount.X < 0)
            {
                corner1 = new Point(nextPosition.Left, nextPosition.Top + 1);
                corner2 = new Point(nextPosition.Left, nextPosition.Bottom - 1);
            }
            else
            {
                corner1 = new Point(nextPosition.Right, nextPosition.Top + 1);
                corner2 = new Point(nextPosition.Right, nextPosition.Bottom - 1);
            }

            if (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2))
                moveAmount.X = 0;

            return moveAmount;
        }

        private Vector2 verticalCollisionTest(Vector2 moveAmount, Layer collisionLayer)
        {
            Point corner1, corner2;
            Rectangle nextPosition = localPlayer.CollisionBox;

            nextPosition.Offset(0, (int)moveAmount.Y);

            if (moveAmount.Y < 0)
            {
                corner1 = new Point(nextPosition.Left + 1, nextPosition.Top);
                corner2 = new Point(nextPosition.Right - 1, nextPosition.Top);
            }
            else
            {
                corner1 = new Point(nextPosition.Left + 1, nextPosition.Bottom);
                corner2 = new Point(nextPosition.Right - 1, nextPosition.Bottom);
            }

            if (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2))
            {
                if (moveAmount.Y > 0)
                    localPlayer.OnGround = true;

                moveAmount.Y = 0;
            }

            return moveAmount;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                DrawBoundingBox(localPlayer.CollisionBox, 1, spriteBatch, TextureManager.Instance.getPixelTextureByColor(Color.Red));
                DrawBoundingBox(localPlayer.NextPosition, 1, spriteBatch, TextureManager.Instance.getPixelTextureByColor(Color.CornflowerBlue));
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 25), Color.White);                
            }
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, SpriteBatch spriteBatch, Texture2D borderTexture)
        {
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White); // Left
            spriteBatch.Draw(borderTexture, new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); // Right
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White); // Top
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White); // Bottom
        }
    }
}