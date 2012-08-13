using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.GUI.Elements;
using Microsoft.Xna.Framework;
using ProjetoFinal.Managers;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class JoinGameState : NetworkGUIGameState
    {
        TextField portTextField;
        TextField ipTextField;

        public JoinGameState()
            : base()
        {
            guiManager.AddElement(new Button("Join Server", new Rectangle(100, 100, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnJoinServerButtonPressed));

            ipTextField = new TextField("127.0.0.1", new Rectangle(100, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));
            guiManager.AddElement(ipTextField);

            portTextField = new TextField("666", new Rectangle(100, 300, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));
            guiManager.AddElement(portTextField);

            guiManager.AddElement(new Button("Main Menu", new Rectangle(100, 400, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnMainMenuButtonPressed));
        }

        public override void Update(Managers.InputManager inputManager, GameTime gameTime)
        {
            base.Update(inputManager, gameTime);

            if (inputManager.Exit)
                gameStatesManager.ResignState(this);
        }

        // Callbacks

        public void OnJoinServerButtonPressed()
        {
            networkManager.Connect(ipTextField.Text, int.Parse(portTextField.Text));

            gameStatesManager.AddState(new ConnectingGameState());
        }

        public void OnMainMenuButtonPressed()
        {
            gameStatesManager.ResignState(this);
        }
    }
}
