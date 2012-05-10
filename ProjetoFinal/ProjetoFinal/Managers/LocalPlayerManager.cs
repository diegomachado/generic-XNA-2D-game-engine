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
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(240, 40), new Rectangle(6, 2, 24, 30));
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

                if (moveAmount.Y != 0)
                    localPlayer.OnGround = false;

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

                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space))
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

                moveAmount = horizontalCollisionTest(moveAmount, collisionLayer);
                moveAmount = verticalCollisionTest(moveAmount, collisionLayer);

                moveAmount.X *= localPlayer.Friction;
                moveAmount.Y = limitFallSpeed(10, moveAmount);

                localPlayer.Position += moveAmount;

                Camera.Instance.Position = localPlayer.Position - new Vector2(Game.ScreenSize.X / 2, Game.ScreenSize.Y / 2);
                //Camera.Instance.Position = new Vector2(MathHelper.Lerp(Camera.Instance.Position.X, localPlayer.Position.X, 0.1f), MathHelper.Lerp(Camera.Instance.Position.Y, localPlayer.Position.Y, 0.1f));

                
                lastKeyboardState = keyboardState;
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
                localPlayer.debugCorner1 = corner1;
                localPlayer.debugCorner2 = corner2;
            }
            else
            {
                corner1 = new Point(nextPosition.Right, nextPosition.Top + 1);
                corner2 = new Point(nextPosition.Right, nextPosition.Bottom - 1);
                localPlayer.debugCorner1 = corner1;
                localPlayer.debugCorner2 = corner2;
            }

            if (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2))
                moveAmount.X = 0;

            return moveAmount;
        }

        private Vector2 verticalCollisionTest(Vector2 moveAmount, Layer collisionLayer)
        {
            Point corner1, corner2;
            Rectangle nextPosition = localPlayer.CollisionBox;

            nextPosition.Offset((int)moveAmount.X, (int)moveAmount.Y);

            if (moveAmount.Y < 0)
            {
                corner1 = new Point(nextPosition.Left + 1, nextPosition.Top);
                corner2 = new Point(nextPosition.Right - 1, nextPosition.Top);
                localPlayer.debugCorner3 = corner1;
                localPlayer.debugCorner4 = corner2;
            }
            else
            {
                corner1 = new Point(nextPosition.Left + 1, nextPosition.Bottom);
                corner2 = new Point(nextPosition.Right - 1, nextPosition.Bottom);
                localPlayer.debugCorner3 = corner1;
                localPlayer.debugCorner4 = corner2;
            }

            if (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2))
            {
                if (moveAmount.Y > 0)
                    localPlayer.OnGround = true;
                else if(moveAmount.Y < 0)
                    localPlayer.OnGround = false;                    

                moveAmount.Y = 0;
            }

            return moveAmount;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 20) - Camera.Instance.Position, Color.White);

                // Debug na Tela
                //DrawBoundingBox(localPlayer.CollisionBox, 1, Color.Red, spriteBatch);
                //DrawBoundingBox(localPlayer.NextPosition, 1, Color.CornflowerBlue, spriteBatch);

                spriteBatch.DrawString(spriteFont, "On Ground: " + localPlayer.OnGround.ToString(), new Vector2(5f, 5f), Color.White);
                spriteBatch.DrawString(spriteFont, "X: " + (int)localPlayer.Position.X, new Vector2(5f, 25f), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + (int)localPlayer.Position.Y, new Vector2(5f, 45f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera X: " + (int)Camera.Instance.Position.X, new Vector2(5f, 65f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera Y: " + (int)Camera.Instance.Position.Y, new Vector2(5f, 85f), Color.White);
                
                DrawPoint(localPlayer.debugCorner1, 3, Color.Yellow, spriteBatch);
                DrawPoint(localPlayer.debugCorner2, 3, Color.Yellow, spriteBatch);
                DrawPoint(localPlayer.debugCorner3, 3, Color.Red, spriteBatch);
                DrawPoint(localPlayer.debugCorner4, 3, Color.Red, spriteBatch);
                
            }
        }

        public void DrawPoint(Point position, int size, Color color, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(position.X - (int)Camera.Instance.Position.X, position.Y - (int)Camera.Instance.Position.Y, size, size), Color.White);
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, Color color, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White); // Left
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); // Right
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White); // Top
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White); // Bottom
        }
    }
}