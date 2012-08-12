using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class PauseState : GameState
    {
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            if (inputManager.Exit)
                gameStatesManager.ExitGame();

            if (inputManager.Pause)
                gameStatesManager.ResignState(this);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.DrawString(spriteFont,
                                   "GAME PAUSED",
                                   new Vector2(graphicsManager.ScreenSize.X / 2, graphicsManager.ScreenSize.Y / 2),
                                   Color.Red);
        }
    }
}
