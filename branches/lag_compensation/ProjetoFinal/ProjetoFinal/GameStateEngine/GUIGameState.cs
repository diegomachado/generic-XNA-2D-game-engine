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

        public override void Update(GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            base.Update(gameTime);
            guiManager.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            base.Draw(gameTime, spriteBatch, spriteFont);

            guiManager.Draw(spriteBatch, spriteFont);
        }
    }
}