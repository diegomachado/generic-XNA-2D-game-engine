using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.EventArgs;
using ProjetoFinal.Entities;

namespace ProjetoFinal.Managers
{
    public enum PlayerState
    {
        Idle,
        WalkingLeft,
        WalkingRight,
        WalkingDead,
        Jumping
    }
    
    class LocalPlayerManager
    {
        public short playerId { get; set; }
        
        private Player localPlayer;
        PlayerState state = PlayerState.Idle;
        Vector2 direction = Vector2.Zero;

        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayerManager()
        {
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(0,240) );        
        }
        
        protected void OnPlayerStateChanged()
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(playerId, localPlayer));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Rectangle clientBounds)
        {
            if (localPlayer != null)
            {
                switch(state)
                {
                    case PlayerState.Idle:

                        if (!(keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right)))
                        {
                            if (keyboardState.IsKeyDown(Keys.Left))
                            {
                                state = PlayerState.WalkingLeft;
                                direction += new Vector2(-1, 0);
                            }

                            if (keyboardState.IsKeyDown(Keys.Right))
                            {
                                state = PlayerState.WalkingRight;
                                direction += new Vector2(1, 0);
                            }
                        }

                        break;

                    case PlayerState.WalkingLeft:

                        if (keyboardState.IsKeyDown(Keys.Right))
                        {
                            state = PlayerState.WalkingRight;
                            direction += new Vector2(2, 0);
                        }
                        else if (!keyboardState.IsKeyDown(Keys.Left))
                        {
                            direction += new Vector2(1, 0);
                        }

                        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                        {
                            direction += new Vector2(-1, 0);
                            state = PlayerState.Idle;
                        }

                        break;

                    case PlayerState.WalkingRight:

                        if (keyboardState.IsKeyDown(Keys.Left))
                        {
                            state = PlayerState.WalkingLeft;
                            direction += new Vector2(-2, 0);
                        }
                        else if (!keyboardState.IsKeyDown(Keys.Right))
                        {
                            direction += new Vector2(-1, 0);
                        }

                        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                        {
                            direction += new Vector2(1, 0);
                            state = PlayerState.Idle;
                        }

                        break;

                    case PlayerState.Jumping:
                        break;
                }
                
                if (direction == Vector2.Zero)
                    state = PlayerState.Idle;

                localPlayer.position += direction * localPlayer.speed;

                localPlayer.position = new Vector2(MathHelper.Clamp(localPlayer.position.X, 0, clientBounds.Width - localPlayer.Width),
                                                   MathHelper.Clamp(localPlayer.position.Y, 0, clientBounds.Height - localPlayer.Height));

                // TODO: Dar um jeito de mandar menos mensagens
                OnPlayerStateChanged();
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                //spriteBatch.DrawString(spriteFont, playerId.ToString(), localPlayer.position, Color.White);
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.position.X + 8, localPlayer.position.Y - 25), Color.White);
            }
        }
    }
}
