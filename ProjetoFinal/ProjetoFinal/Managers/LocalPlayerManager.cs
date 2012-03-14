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
    class LocalPlayerManager
    {
        private short playerId;
        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;
        private Player localPlayer;

        public LocalPlayerManager()
        {
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), Vector2.Zero);        
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
                Vector2 inputDirection = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    inputDirection.X -= 1;
                if (keyboardState.IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (keyboardState.IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;

                localPlayer.position += inputDirection * localPlayer.speed;
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
                spriteBatch.DrawString(spriteFont, playerId.ToString(), localPlayer.position, Color.White);
            }
        }
    }
}
