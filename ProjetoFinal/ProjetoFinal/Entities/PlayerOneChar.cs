using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjetoFinal.Entities
{
    class PlayerOneChar : Char
    {
        public PlayerOneChar(Texture2D playerSkin, Vector2 playerPosition, float playerSpeed)
            : base(playerSkin, playerPosition, playerSpeed)
        {
            this.skin = playerSkin;
            this.position = playerPosition;
            this.speed = playerSpeed;
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
            position.X = MathHelper.Clamp(position.X, 0, clientBounds.Width - Width);
            position.Y = MathHelper.Clamp(position.Y, 0, clientBounds.Height - Height);

            base.Update();
        }
    }
}
