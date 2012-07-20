using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class GameplayState : GameState
    {
        // Managers
        PlayerManager playerManager;
        LocalPlayerManager localPlayerManager;
        TextureManager textureManager = TextureManager.Instance;
        EventManager eventManager = EventManager.Instance;
        MapManager mapManager = MapManager.Instance;
        NetworkManager networkManager = NetworkManager.Instance;

        // Camera Globals
        Camera camera;
        public static Point ScreenSize { get; set; }
        public static Point MapWidthInPixels { get; set; }

        public GameplayState() : base()
        {
            // TODO: configurar networkManager
            // Fazer com que esse estado assine os metodos que ele precisa de network manager nesse ponto
            networkManager.UpdatePlayerStateMessageReceived += HandleUpdatePlayerStateMessage;
            networkManager.HailMessageReceived += HandleHailMessage;

            // Managers
            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();

            if (networkManager.IsHost)
                localPlayerManager.createLocalPlayer(0);

            // Registering Events
            eventManager.PlayerStateChanged += (sender, e) => this.networkManager.SendMessage(new UpdatePlayerStateMessage(e.id, e.player, e.messageType));

            // Camera
            camera = Camera.Instance;
            camera.Speed = 4f;
            //MapWidthInPixels = mapManager.GetMapSize();
            //ScreenSize = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent() { }
        
        public override void Update(GameStatesManager gameStateManager, InputManager inputManager, GameTime gameTime)
        {
            if (inputManager.Exit)
                gameStateManager.ExitGame();

            localPlayerManager.Update(gameTime, inputManager, mapManager.GetCollisionLayer());
            playerManager.Update(gameTime, inputManager, mapManager.GetCollisionLayer());
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Matrix scaleMatrix = Matrix.CreateScale(0.5f);

            //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, scaleMatrix);
            spriteBatch.Begin();

            // Drawing Entities

            /*mapManager.DrawEfficiently(spriteBatch,
                                       camera.PositionToPoint(),
                                       PositionToTileCoord(camera.Position, mapManager.GetTileSize()),
                                       PositionToTileCoord(camera.Position + ViewportVector(mapManager.GetTileSize()), mapManager.GetTileSize()));*/

            // TODO: Fazer saporra eficiente e foda-se
            mapManager.Draw(spriteBatch, camera.PositionToPoint());

            localPlayerManager.Draw(spriteBatch, spriteFont);
            playerManager.Draw(spriteBatch, spriteFont);

            // In Game Debug
            float frameRate;
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.DrawString(spriteFont, "FPS: " + Math.Round(frameRate), new Vector2(ScreenSize.X - 70, 5), Color.White);

            spriteBatch.DrawString(spriteFont, "Players:", new Vector2(ScreenSize.X - 70, 25), Color.White);

            Vector2 playersDebugTextPosition = new Vector2(ScreenSize.X - 200, 25);
            foreach (KeyValuePair<short, Client> client in networkManager.clients)
            {
                playersDebugTextPosition.Y += 20;
                spriteBatch.DrawString(spriteFont, client.Value.nickname, playersDebugTextPosition, Color.White);
            }

            spriteBatch.End();
        }

        public override void UnloadContent() { }

        private void HandleUpdatePlayerStateMessage(object sender, EventArgs e)
        {
            //private void HandleUpdatePlayerStateMessage(NetIncomingMessage im)

            /*UpdatePlayerStateMessage message = new UpdatePlayerStateMessage(im);

            if (message.playerId != localPlayerManager.playerId)
            {
                Player player = playerManager.GetPlayer(message.playerId);

                // TODO: Tentar implementar algo de Lag Prediction
                //var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.messageTime));

                if (player.LastUpdateTime < message.messageTime)
                {
                    var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.messageTime));

                    playerManager.UpdatePlayer(message.playerId, message.position, message.speed, timeDelay, message.messageType, message.playerState);
                    // TODO: Pensar sobre isso: player.position = message.position += (message.speed * timeDelay);
                }

                if (IsHost)
                    networkManager.SendMessage(new UpdatePlayerStateMessage(message.playerId, player, message.messageType));
            }*/
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
