using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class MainMenuState : GameState
    {
        public override void Update(GameStatesManager gameStateManager, InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            if (inputManager.Exit)
                gameStateManager.ExitGame();

            // TODO: Criar menu de seleção inicial
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.DrawString(spriteFont,
                                   "TEST",
                                   new Vector2(graphicsManager.ScreenSize.X / 2, graphicsManager.ScreenSize.Y / 2),
                                   Color.Cyan);
        }
    }
}
