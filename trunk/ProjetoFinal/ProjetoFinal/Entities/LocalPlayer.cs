using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.EventArgs;

namespace ProjetoFinal.Entities
{
    class LocalPlayer : Player
    {
        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayer(short playerId, Texture2D playerSkin, Vector2 playerPosition)
            : base(playerId, playerSkin, playerPosition)
        {
      
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState,  Rectangle clientBounds)
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

            position += inputDirection * speed;
            position = new Vector2(MathHelper.Clamp(this.position.X, 0, clientBounds.Width - Width), 
                                   MathHelper.Clamp(this.position.Y, 0, clientBounds.Height - Height));

            // TODO: Dar um jeito de mandar menos mensagens
            OnPlayerStateChanged();

            base.Update();
        }

        protected void OnPlayerStateChanged()
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(this));
        }
    }
}
