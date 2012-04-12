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
        KeyboardState lastKeyboardState;

        //MovementState movementState = MovementState.Idle;

        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayerManager()
        {
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(0,240) );        
        }
        
        protected void OnPlayerStateChanged(PlayerState playerState)
        {
            localPlayer.State = playerState;

            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(playerId, localPlayer));
        }

        // TODO: Refactor Update Method on LocalPlayerManager
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Rectangle clientBounds, Layer collisionLayer)
        {
            if (localPlayer != null)
            {
                #region StatesAndShitCommented
                //switch(movementState)
                //{
                //    case MovementState.Idle:

                //        if (!(keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right)))
                //        {
                //            if (keyboardState.IsKeyDown(Keys.Left))
                //            {
                //                movementState = MovementState.WalkingLeft;
                //                direction += new Vector2(-1, 0);
                //            }

                //            if (keyboardState.IsKeyDown(Keys.Right))
                //            {
                //                movementState = MovementState.WalkingRight;
                //                direction += new Vector2(1, 0);
                //            }
                            
                //            if (keyboardState.IsKeyDown(Keys.Up))
                //            {
                //                movementState = MovementState.Jumping;
                //                direction += new Vector2(0, -1);
                //            }
                //        }

                //        break;

                //    case MovementState.WalkingLeft:

                //        if (keyboardState.IsKeyDown(Keys.Right))
                //        {
                //            movementState = MovementState.WalkingRight;
                //            direction += new Vector2(2, 0);
                //        }
                //        else if (!keyboardState.IsKeyDown(Keys.Left))
                //        {
                //            direction += new Vector2(1, 0);
                //        }

                //        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                //        {
                //            movementState = MovementState.Idle;
                //            direction += new Vector2(-1, 0);
                //        }

                //        if (keyboardState.IsKeyDown(Keys.Up))
                //        {
                //            movementState = MovementState.Jumping;
                //            direction += new Vector2(0, -1);
                //        }

                //        break;

                //    case MovementState.WalkingRight:

                //        if (keyboardState.IsKeyDown(Keys.Left))
                //        {
                //            movementState = MovementState.WalkingLeft;
                //            direction += new Vector2(-2, 0);
                //        }
                //        else if (!keyboardState.IsKeyDown(Keys.Right))
                //        {
                //            direction += new Vector2(-1, 0);
                //        }

                //        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                //        {
                //            movementState = MovementState.Idle;
                //            direction += new Vector2(1, 0);
                //        }

                //        if (keyboardState.IsKeyDown(Keys.Up))
                //        {
                //            movementState = MovementState.Jumping;
                //            direction += new Vector2(0, -1);
                //        }

                //        break;

                //    case MovementState.Jumping:
                //        break;

                //    case MovementState.Falling:
                //        break;
                //}
                
                //if (direction == Vector2.Zero)
                //    movementState = MovementState.Idle;

                //localPlayer.position += direction * localPlayer.speed;
                #endregion

                acceleration = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Left))
                        OnPlayerStateChanged(PlayerState.WalkingLeft);

                    acceleration += new Vector2(-0.5f, 0.0f);
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Right))
                        OnPlayerStateChanged(PlayerState.WalkingRight);

                    acceleration += new Vector2(0.5f, 0.0f);
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (localPlayer.Position.Y == (clientBounds.Height - localPlayer.Height))
                    {
                        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                            OnPlayerStateChanged(PlayerState.Jumping);
                        if (keyboardState.IsKeyDown(Keys.Left))
                            OnPlayerStateChanged(PlayerState.JumpingLeft);
                        else if (keyboardState.IsKeyDown(Keys.Right))
                            OnPlayerStateChanged(PlayerState.JumpingRight);
                        else
                            OnPlayerStateChanged(PlayerState.Jumping);

                        acceleration += new Vector2(0.0f, localPlayer.JumpForce);
                    }
                }

                if (!keyboardState.IsKeyDown(Keys.Space) &&
                    !keyboardState.IsKeyDown(Keys.Right) &&
                    !keyboardState.IsKeyDown(Keys.Left))
                {
                    OnPlayerStateChanged(PlayerState.Idle);
                }

                // TODO: Ajeitar colisão com o chão e testes se o jogador esta no chão ou não
                acceleration += new Vector2(0.0f, localPlayer.Gravity);

                localPlayer.speed += acceleration;
                localPlayer.speed.X *= localPlayer.Friction;





                Vector2 nextPosition = localPlayer.Position;
                nextPosition += localPlayer.speed;

                Point xy = new Point( (int)(nextPosition.X % clientBounds.Width) / 32, (int)(nextPosition.Y % clientBounds.Height) / 32);
                
                Tile actualTile = collisionLayer.GetTileId(xy);
                
                //if(actualTile != null)
                    Console.WriteLine(xy.X);
                
                localPlayer.Position += localPlayer.speed;
                localPlayer.Position = new Vector2(MathHelper.Clamp(localPlayer.Position.X, 0, clientBounds.Width - localPlayer.Width),
                                                   MathHelper.Clamp(localPlayer.Position.Y, 0, clientBounds.Height - localPlayer.Height));

                if (localPlayer.Position.Y == (clientBounds.Height - localPlayer.Height))
                    localPlayer.speed.Y = 0.0f;
















                lastKeyboardState = keyboardState;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                //spriteBatch.DrawString(spriteFont, playerId.ToString(), localPlayer.position, Color.White);
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 25), Color.White);
            }
        }
    }
}