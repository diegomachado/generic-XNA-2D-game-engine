using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;
using ProjetoFinal.GUI;

namespace ProjetoFinal.GameStateEngine
{
    // TODO: Obviamente refatorar essa PORRA, mas ainda não sei como, sei que vou ter que usar interface mas não faço idéia de como fazer isso ainda. Bater cabeça com bombado pra figure it out.
    abstract class NetworkGUIGameState : NetworkGameState
    {
        // Managers
        protected GUIManager guiManager;

        public NetworkGUIGameState()
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