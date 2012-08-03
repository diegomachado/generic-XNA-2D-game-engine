using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.EventHeaders;
using Lidgren.Network;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class GameplayState : GameState
    {
        // Managers
        PlayerManager playerManager;
        LocalPlayerManager localPlayerManager;        
        MapManager mapManager = MapManager.Instance;
        Camera camera = Camera.Instance;

        public GameplayState() : base()
        {
            eventManager.PlayerStateUpdated += HandleUpdatePlayerStateMessage;
            //eventManager.HailMessageReceived += HandleHailMessage;

            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();

            camera.Speed = 4f;

            //if (networkManager.IsHost)
                localPlayerManager.createLocalPlayer(0);

        }

        public override void LoadContent(ContentManager content) 
        {
            mapManager.Content = content;
            mapManager.LoadMap(MapType.Level1);
        }
        
        public override void Update(GameStatesManager gameStateManager, InputManager inputManager, GameTime gameTime)
        {
            if (inputManager.Exit)
                gameStateManager.ExitGame();

            localPlayerManager.Update(gameTime, inputManager, mapManager.GetCollisionLayer());
            camera.FollowLocalPlayer(localPlayerManager.LocalPlayer);
            
            playerManager.Update(gameTime, mapManager.GetCollisionLayer());
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Begin();

            // Drawing Entities
            mapManager.Draw(spriteBatch,
                                       camera.PositionToPoint(),
                                       PositionToTileCoord(camera.Position, mapManager.GetTileSize()),
                                       PositionToTileCoord(camera.Position + ViewportVector(mapManager.GetTileSize()), mapManager.GetTileSize()));

            localPlayerManager.Draw(spriteBatch, spriteFont);
            playerManager.Draw(spriteBatch, spriteFont);

            // In Game Debug
            float frameRate;
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.DrawString(spriteFont, "FPS: " + Math.Round(frameRate), new Vector2(graphicsManager.ScreenSize.X - 70, 5), Color.White);

            spriteBatch.DrawString(spriteFont, "Players:", new Vector2(graphicsManager.ScreenSize.X - 70, 25), Color.White);

            Vector2 playersDebugTextPosition = new Vector2(graphicsManager.ScreenSize.X - 200, 25);
            foreach (KeyValuePair<short, Client> client in networkManager.clients)
            {
                playersDebugTextPosition.Y += 20;
                spriteBatch.DrawString(spriteFont, client.Value.nickname, playersDebugTextPosition, Color.White);
            }

            spriteBatch.End();
        }

        // TODO: REFACTOR THIS XIT
        public Point PositionToTileCoord(Vector2 position, Point tileSize)
        {
            return new Point((int)position.X / tileSize.X, (int)position.Y / tileSize.Y);
        }

        private Vector2 ViewportVector(Point tileSize)
        {
            return new Vector2(graphicsManager.ScreenSize.X + tileSize.X, graphicsManager.ScreenSize.Y + tileSize.Y);
        }

        public override void UnloadContent() { }

        // Eventos de Network
        private void HandleUpdatePlayerStateMessage(object sender, PlayerStateUpdatedEventArgs playerStateUpdatedEventArgs)
        {
            if (playerStateUpdatedEventArgs.playerId != localPlayerManager.playerId)
            {
                Player player = playerManager.GetPlayer(playerStateUpdatedEventArgs.playerId);

                // TODO: Tentar implementar algo de Lag Prediction
                //var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.messageTime));

                if (player.LastUpdateTime < playerStateUpdatedEventArgs.messageTime)
                {
                    var timeDelay = (float)(NetTime.Now - playerStateUpdatedEventArgs.localTime);

                    playerManager.UpdatePlayer(playerStateUpdatedEventArgs.playerId,
                                               playerStateUpdatedEventArgs.position, 
                                               playerStateUpdatedEventArgs.speed, 
                                               timeDelay,
                                               playerStateUpdatedEventArgs.movementType,
                                               playerStateUpdatedEventArgs.playerState);
                    // TODO: Pensar sobre isso: player.position = message.position += (message.speed * timeDelay);
                }

                eventManager.ThrowOtherClientPlayerStateChanged(playerStateUpdatedEventArgs.playerId, player, playerStateUpdatedEventArgs.movementType);
            }
        }

        private void HandleHailMessage(object sender, EventArgs e)
        {
            //private void HandleHailMessage(HailMessage message)

            //localPlayerManager.createLocalPlayer(message.clientId);

            //foreach (short id in message.clientsInfo.Keys)
            //    this.playerManager.AddPlayer(id);
        }
    }
}
