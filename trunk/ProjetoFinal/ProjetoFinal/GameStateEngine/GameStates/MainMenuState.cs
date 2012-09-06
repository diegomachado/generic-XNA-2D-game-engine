using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;
using ProjetoFinal.GUI;
using ProjetoFinal.GUI.Elements;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class MainMenuState : GUIGameState
    {
        public MainMenuState() : base()
        {
            guiManager.AddElement(new Button("Host Game", new Rectangle(100, 100, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnHostGameButtonPressed));
            guiManager.AddElement(new Button("Join Game", new Rectangle(100, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnJoinGameButtonPressed));
            guiManager.AddElement(new Button("Exit Game", new Rectangle(100, 300, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnExitGameButtonPressed));
        }

        public override void Update(GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            base.Update(gameTime);

            if (inputManager.Exit)
                GameStatesManager.ExitGame();
        }

        // Callbacks

        public void OnHostGameButtonPressed()
        {
            GameStatesManager.AddState(new HostGameState());
        }

        public void OnJoinGameButtonPressed()
        {
            GameStatesManager.AddState(new JoinGameState());
        }

        public void OnExitGameButtonPressed()
        {
            GameStatesManager.ExitGame();
        }
    }
}
