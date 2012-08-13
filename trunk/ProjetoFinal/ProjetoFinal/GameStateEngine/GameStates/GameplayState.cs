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
    class GameplayState : NetworkGameState
    {
        // Managers
        PlayerManager playerManager;
        LocalPlayerManager localPlayerManager;        
        MapManager mapManager = MapManager.Instance;
        Camera camera = Camera.Instance;

        public GameplayState(short playerId) : base()
        {
            eventManager.PlayerStateUpdated += OnOtherClientPlayerStateUpdated;
            eventManager.ClientDisconnected += OnClientDisconnected;

            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();

            localPlayerManager.createLocalPlayer(playerId);

            camera.Speed = 4f;
        }

        public GameplayState() : this(0)
        {
            
        }
        
        public GameplayState(short playerId, Dictionary<short, Client> clientsInfo) : this(playerId)
        {
            foreach (short id in clientsInfo.Keys)
                this.playerManager.AddPlayer(id);
        }

        public override void LoadContent(ContentManager content) 
        {
            mapManager.Content = content;
            mapManager.LoadMap(MapType.Level1);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            base.Update(inputManager, gameTime);

            if (inputManager.Exit)
                gameStatesManager.ExitGame();

            if (inputManager.Pause)
                gameStatesManager.AddState(new PauseState());

            localPlayerManager.Update(gameTime, inputManager, mapManager.CollisionLayer);
            camera.FollowLocalPlayer(localPlayerManager.LocalPlayer);
            
            playerManager.Update(gameTime, mapManager.CollisionLayer);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            // Drawing Entities
            mapManager.Draw(spriteBatch, camera.PositionToPoint, graphicsManager.ScreenSize);
            localPlayerManager.Draw(spriteBatch, spriteFont);
            playerManager.Draw(spriteBatch, spriteFont);

            // TODO: THIS SHIT HAS TO GET THE HELL OTTA HERE
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
        }

        // TODO: REVER PORQUE FUNCIONOU SEM O TILESIZE_X e TILESIZE_Y em Map.cs depois do refactoring do Guifes
        // TODO: REFACTOR THIS XIT
        //private Vector2 ViewportVector(Point tileSize)
        //{
        //    return new Vector2(graphicsManager.ScreenSize.X + tileSize.X, graphicsManager.ScreenSize.Y + tileSize.Y);
        //}

        // Eventos de Network
        private void OnOtherClientPlayerStateUpdated(object sender, PlayerStateUpdatedEventArgs playerStateUpdatedEventArgs)
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
            }
            else
            {
                // TODO: VERIFICAR SAPORRA, acontece, refactoring previsto em network manager
                Console.WriteLine("Olha a merda > " + playerStateUpdatedEventArgs.playerId);
            }
        }

        private void OnClientDisconnected(object sender, EventArgs eventArgs)
        {
            gameStatesManager.ResignState(this);
        }
    }
}
