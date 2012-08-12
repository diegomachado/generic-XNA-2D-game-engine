using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;
using ProjetoFinal.GUI;

namespace ProjetoFinal.GameStateEngine
{
    abstract class GUIGameState : GameState
    {
        // Managers
        protected GUIManager guiManager;

        public GUIGameState()
        {
            guiManager = new GUIManager();
        }

        public override void Update(InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            base.Update(inputManager, gameTime);

            guiManager.Update(inputManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            base.Draw(gameTime, spriteBatch, spriteFont);

            guiManager.Draw(spriteBatch, spriteFont);
        }
    }
}