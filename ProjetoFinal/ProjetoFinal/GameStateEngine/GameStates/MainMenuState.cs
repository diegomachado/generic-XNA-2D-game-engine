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

            guiManager.AddElement(new Button("Exit", new Rectangle(100, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnExitGameButtonPressed));
        }

        public override void Update(InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            base.Update(inputManager, gameTime);

            if (inputManager.Exit)
                gameStatesManager.ExitGame();
        }

        // Callbacks

        public void OnHostGameButtonPressed(object sender)
        {
            gameStatesManager.AddState(new HostGameState());
        }

        public void OnJoinGameButtonPressed(object sender)
        {
            Console.WriteLine("Join Game");
        }

        public void OnExitGameButtonPressed(object sender)
        {
            gameStatesManager.ExitGame();
        }
    }
}
