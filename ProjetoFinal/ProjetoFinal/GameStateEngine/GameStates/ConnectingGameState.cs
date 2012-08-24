using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.GUI.Elements;
using Microsoft.Xna.Framework;
using ProjetoFinal.Managers;
using ProjetoFinal.EventHeaders;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class ConnectingGameState : NetworkGUIGameState
    {
        int dots = 0;
        int timeBuffer = 0;

        public ConnectingGameState()
            : base()
        {
            IsPopup = true;

            eventManager.ClientConnected += OnClientConnected;
            eventManager.ClientDisconnected += OnClientDisconnected;

            guiManager.AddElement(new Button("Cancel", new Rectangle(300, 200, 305, 51), textureManager.getTexture(TextureList.ButtonFrame), OnCancelButtonClicked));
        }

        public override void Update(Managers.InputManager inputManager, GameTime gameTime)
        {
            base.Update(inputManager, gameTime);

            if (inputManager.Exit)
                OnCancelButtonClicked();
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont)
        {
            base.Draw(gameTime, spriteBatch, spriteFont);

            timeBuffer += gameTime.ElapsedGameTime.Milliseconds;
            if (timeBuffer > 500)
            {
                timeBuffer = 0;
                dots = (dots + 1) % 4;
            }

            String text = "Connecting";

            for (int i = 0; i < dots; i++)
            {
                text += ".";
            }

            spriteBatch.DrawString(spriteFont, text, new Vector2(300, 170), Color.White);
        }

        // Callbacks

        public void OnCancelButtonClicked()
        {
            networkManager.Disconnect();
            GameStatesManager.ResignState(this);
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            GameStatesManager.ResignState(this);
            GameStatesManager.AddState(new GameplayState(clientConnectedEventArgs.ClientId, clientConnectedEventArgs.ClientsInfo));
        }

        private void OnClientDisconnected(object sender, EventArgs eventArgs)
        {
            // TODO: Talvez precise dar disconnect aqui, talvez não, tem que ver!
            GameStatesManager.ResignState(this);
        }
    }
}
