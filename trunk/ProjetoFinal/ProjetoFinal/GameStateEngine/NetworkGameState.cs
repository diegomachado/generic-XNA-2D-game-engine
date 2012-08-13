using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;
using System.Collections.Generic;
using ProjetoFinal.Entities;

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            base.Draw(gameTime, spriteBatch, spriteFont);

            if (networkManager.IsConnected)
            {
                spriteBatch.DrawString(spriteFont, "Players:", new Vector2(graphicsManager.ScreenSize.X - 70, 25), Color.White);

                Vector2 playersDebugTextPosition = new Vector2(graphicsManager.ScreenSize.X - 200, 25);
                foreach (KeyValuePair<short, Client> client in networkManager.clients)
                {
                    playersDebugTextPosition.Y += 20;
                    spriteBatch.DrawString(spriteFont, client.Value.nickname, playersDebugTextPosition, Color.White);
                }
            }
        }
    }
}