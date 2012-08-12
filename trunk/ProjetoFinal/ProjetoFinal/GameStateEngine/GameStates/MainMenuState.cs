using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;
using ProjetoFinal.GUI;
using ProjetoFinal.GUI.Elements;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class MainMenuState : GameState
    {
        // Managers
        GUIManager guiManager;

        public MainMenuState()
        {
            guiManager = new GUIManager();

            // TODO: Inicializar toda a GUI
            //guiManager.AddElement(new Button("Host Game", new Rectangle(100, 100, 200, 100)));
            guiManager.AddElement(new Button("Host Game", new Rectangle(100, 100, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnHostGameButtonPressed));
        }

        public override void Update(GameStatesManager gameStateManager, InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            if (inputManager.Exit)
                gameStateManager.ExitGame();

            guiManager.Update(inputManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            guiManager.Draw(spriteBatch, spriteFont);
        }

        // Callbacks

        public void OnHostGameButtonPressed(object sender)
        {
            // TODO: Empilhar estado de HostGame
            // Nesse caso tenho que refatorar pq eh necessario acessar o GameStateManager, o qual so tenho acesso em Update
        }

        public void OnJoinGameButtonPressed(object sender)
        {
            // TODO: Empilhar estado de JoinGame
            // Nesse caso tenho que refatorar pq eh necessario acessar o GameStateManager, o qual so tenho acesso em Update
        }
    }
}
