using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;

namespace ProjetoFinal.GameStateEngine
{
    abstract class NetworkGameState : GameState
    {
        protected NetworkManager networkManager = NetworkManager.Instance;

        public override void Update(InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            base.Update(inputManager, gameTime);

            if(networkManager.IsConnected)
                networkManager.ProcessNetworkMessages();
        }
    }
}