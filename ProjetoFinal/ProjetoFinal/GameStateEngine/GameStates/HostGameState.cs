using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ProjetoFinal.GUI;
using ProjetoFinal.GUI.Elements;
using ProjetoFinal.Managers;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class HostGameState : NetworkGUIGameState
    {
        Button startServerButton, 
               mainMenuButton;
        
        TextField portTextField, 
                  nicknameTextField;

        public HostGameState() : base()
        {
            startServerButton = new Button("Start Server", new Rectangle(100, 100, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnStartServerButtonPressed);
            mainMenuButton = new Button("Main Menu", new Rectangle(100, 400, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnMainMenuButtonPressed);

            portTextField = new TextField("666", new Rectangle(100, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));
            nicknameTextField = new TextField("AdminNick", new Rectangle(100, 300, 305, 51), textureManager.getTexture(TextureList.ButtonFrame));

            guiManager.AddElement(startServerButton);            
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

        // Callbacks
        public void OnStartServerButtonPressed()
        {
            networkManager.Host(int.Parse(portTextField.Text), nicknameTextField.Text);

            GameStatesManager.AddState(new GameplayState());
        }

        public void OnMainMenuButtonPressed()
        {
            GameStatesManager.ResignState(this);
        }
    }
}
