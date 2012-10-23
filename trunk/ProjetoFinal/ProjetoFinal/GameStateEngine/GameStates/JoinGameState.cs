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
        Button joinServerButton,
               mainMenuButton;

        TextField portTextField,
                  nicknameTextField,
                  ipTextField;

        public JoinGameState() : base()
        {
            joinServerButton = new Button("Join Server", new Rectangle(100, 100, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnJoinServerButtonPressed);
            mainMenuButton = new Button("Main Menu", new Rectangle(100, 500, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnMainMenuButtonPressed);

            ipTextField = new TextField("127.0.0.1", new Rectangle(100, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));
            portTextField = new TextField("666", new Rectangle(100, 300, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));
            // TODO: Criar TextField que aceite letras em inputManager
            nicknameTextField = new TextField("ClientNick", new Rectangle(100, 400, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));

            guiManager.AddElement(joinServerButton);
            guiManager.AddElement(ipTextField);
            guiManager.AddElement(portTextField);
            guiManager.AddElement(nicknameTextField);
            guiManager.AddElement(mainMenuButton);           
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (inputManager.Exit)
                GameStatesManager.ResignState(this);
        }

        public void OnJoinServerButtonPressed()
        {
            networkManager.Connect(ipTextField.Text, int.Parse(portTextField.Text), nicknameTextField.Text);

            GameStatesManager.AddState(new ConnectingGameState());
        }

        public void OnMainMenuButtonPressed()
        {
            GameStatesManager.ResignState(this);
        }
    }
}
