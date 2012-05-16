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
        Vector2 speed = Vector2.Zero;

        Point corner1, corner2, corner3, corner4;

        KeyboardState lastKeyboardState;
        
        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayerManager()
        {
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(240, 40), new Rectangle(6, 2, 24, 30));   
        }
        
        protected void OnPlayerStateChanged(PlayerState playerState)
        {
            localPlayer.State = playerState;

            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(playerId, localPlayer));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Layer collisionLayer)
        {
            if (localPlayer == null)
                return;

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Left))
                {
                    if(localPlayer.OnGround)
                        OnPlayerStateChanged(PlayerState.WalkingLeft);
                    else
                        OnPlayerStateChanged(PlayerState.JumpingLeft);                    
                }

                localPlayer.Flipped = true;
                speed -= localPlayer.walkForce;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (localPlayer.OnGround)
                        OnPlayerStateChanged(PlayerState.WalkingRight);
                    else
                        OnPlayerStateChanged(PlayerState.JumpingRight);
                }
                 

                localPlayer.Flipped = false;
                speed += localPlayer.walkForce;                    
            }

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (checkCollision(collisionBoxVerticalOffset, collisionLayer))
            {
                localPlayer.OnGround = true;
                speed.Y = 0;

                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space))
                {
                    if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                        OnPlayerStateChanged(PlayerState.Jumping);
                    else if (keyboardState.IsKeyDown(Keys.Left))
                        OnPlayerStateChanged(PlayerState.JumpingLeft);
                    else if (keyboardState.IsKeyDown(Keys.Right))
                        OnPlayerStateChanged(PlayerState.JumpingRight);
                    else
                        OnPlayerStateChanged(PlayerState.Jumping);

                    speed += localPlayer.JumpForce;
                    localPlayer.OnGround = false;
                }
            }
            else
            {
                speed += localPlayer.Gravity;
            }

            // So player doesn't slide forever
            if (Math.Abs(speed.X) < 1)
                speed.X = 0;
            
            if (localPlayer.OnGround && !keyboardState.IsKeyDown(Keys.Space) && !keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
                OnPlayerStateChanged(PlayerState.Idle);

            speed.X *= localPlayer.Friction;
            speed.Y = MathHelper.Clamp(speed.Y, localPlayer.JumpForce.Y, 10);
            handleHorizontalCollision(collisionLayer);
            handleVerticalCollision(collisionLayer);
               
            Camera.Instance.Position = localPlayer.Position 
                                        + new Vector2(localPlayer.Skin.Width / 2, localPlayer.Skin.Height / 2) 
                                        - new Vector2(Game.ScreenSize.X / 2, Game.ScreenSize.Y / 2);

            lastKeyboardState = keyboardState;          
        }

        private bool checkCollision(Rectangle collisionBox, Layer collisionLayer)
        {
            if (speed.Y < 0)
            {
                corner1 = new Point(collisionBox.Left, collisionBox.Top);
                corner2 = new Point(collisionBox.Right, collisionBox.Top);
            }
            else
            {
                corner1 = new Point(collisionBox.Left, collisionBox.Bottom);
                corner2 = new Point(collisionBox.Right, collisionBox.Bottom);
            }

            if (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2))
                return true;

            if (speed.X < 0)
            {
                corner3 = new Point(collisionBox.Left, collisionBox.Top);
                corner4 = new Point(collisionBox.Left, collisionBox.Bottom);
            }
            else
            {
                corner3 = new Point(collisionBox.Right, collisionBox.Top);
                corner4 = new Point(collisionBox.Right, collisionBox.Bottom);
            }

            if (collisionLayer.GetTileValueByPixelPosition(corner3) || collisionLayer.GetTileValueByPixelPosition(corner4))
                return true;            
            
            return false;
        }

        private void handleHorizontalCollision(Layer collisionLayer)
        {
            Rectangle collisionBoxOffset = localPlayer.CollisionBox;

            for (int i = 0; i < Math.Abs(speed.X); ++i)
            {
                collisionBoxOffset.Offset(Math.Sign(speed.X), 0);
                if (!checkCollision(collisionBoxOffset, collisionLayer))
                {
                    localPlayer.Position += new Vector2(Math.Sign(speed.X), 0);
                }
                else
                {
                    speed.X = 0;
                    break;
                }
            }
        }

        private void handleVerticalCollision(Layer collisionLayer)
        {
            Rectangle collisionBoxOffset = localPlayer.CollisionBox;            

            for (int i = 0; i < Math.Abs(speed.Y); ++i)
            {
                collisionBoxOffset.Offset(0, Math.Sign(speed.Y));
                
                if (!checkCollision(collisionBoxOffset, collisionLayer))
                {
                    localPlayer.Position += new Vector2(0, Math.Sign(speed.Y));
                }
                else
                {
                    speed.Y = 0;
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                //DrawBoundingBox(localPlayer.CollisionBox, 1, Color.Red, spriteBatch);
                //DrawBoundingBox(localPlayer.NextPosition, 1, Color.CornflowerBlue, spriteBatch);
                
                // On-Screen Debug
                spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 0, 170, 170), new Color(0, 0, 0, 0.2f));
                
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 20) - Camera.Instance.Position, Color.White);
                spriteBatch.DrawString(spriteFont, "OnGround: " + localPlayer.OnGround.ToString(), new Vector2(5f, 5f), Color.White);                
                spriteBatch.DrawString(spriteFont, "X: " + (int)localPlayer.Position.X, new Vector2(5f, 25f), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + (int)localPlayer.Position.Y, new Vector2(5f, 45f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)speed.X, new Vector2(5f, 65f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)speed.Y, new Vector2(5f, 85f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)Camera.Instance.Position.X, new Vector2(5f, 105f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)Camera.Instance.Position.Y, new Vector2(5f, 125f), Color.White);
                spriteBatch.DrawString(spriteFont, "State: " + localPlayer.State, new Vector2(5f, 145f), Color.White);
           
                DrawPoint(spriteBatch, corner1, 3, Color.Yellow);
                DrawPoint(spriteBatch, corner2, 3, Color.Yellow);
                DrawPoint(spriteBatch, corner3, 3, Color.Red);
                DrawPoint(spriteBatch, corner4, 3, Color.Red);                
            }
        }

        public void DrawPoint(SpriteBatch spriteBatch, Point position, int size, Color color)
        {
            // TODO: Transformar conta com Camera em uma funcao de Camera
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(position.X - (int)Camera.Instance.Position.X, position.Y - (int)Camera.Instance.Position.Y, size, size), Color.White);
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, Color color, SpriteBatch spriteBatch)
        {
            // TODO: Transformar conta com Camera em uma funcao de Camera
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, borderWidth, r.Height), Color.White); // Left
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Right - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, borderWidth, r.Height), Color.White); // Right
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, r.Width, borderWidth), Color.White); // Top
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Bottom - (int)Camera.Instance.Position.Y, r.Width, borderWidth), Color.White); // 
        }
    }
}