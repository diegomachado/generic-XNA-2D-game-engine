using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class MainMenuState : GameState
    {
        public virtual void Update(GameStatesManager gameStateManager, InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            if (inputManager.Exit)
                gameStateManager.ExitGame();
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont) { }
    }
}
