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
    class HostGameState : GUIGameState
    {
        public HostGameState() : base()
        {
            guiManager.AddElement(new Button("Start Server", new Rectangle(100, 100, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnStartServerButtonPressed));

            guiManager.AddElement(new Button("Main Menu", new Rectangle(100, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnMainMenuButtonPressed));

            guiManager.AddElement(new TextField("127.0.0.1", new Rectangle(100, 300, 305, 51), textureManager.getTexture(TextureList.ButtonFrame)));
        }

        public override void Update(Managers.InputManager inputManager, GameTime gameTime)
        {
            base.Update(inputManager, gameTime);

            if (inputManager.Exit)
                gameStatesManager.ResignState(this);
        }

        // Callbacks

        public void OnStartServerButtonPressed(object sender)
        {
            gameStatesManager.AddState(new GameplayState());
        }

        public void OnMainMenuButtonPressed(object sender)
        {
            gameStatesManager.ResignState(this);
        }
    }
}
