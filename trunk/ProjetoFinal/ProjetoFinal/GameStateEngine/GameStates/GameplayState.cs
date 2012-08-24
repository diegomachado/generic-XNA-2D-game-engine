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
        PlayerManager playerManager;
        LocalPlayerManager localPlayerManager;
        ArrowManager arrowManager;
        MapManager mapManager = MapManager.Instance;
        Camera camera = Camera.Instance;

        public GameplayState(short localPlayerId) : base()
        {
            eventManager.PlayerStateUpdated += OnOtherClientPlayerStateUpdated;
            eventManager.ClientDisconnected += OnClientDisconnected;

            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();
            localPlayerManager.createLocalPlayer(localPlayerId);
            arrowManager = new ArrowManager(localPlayerId, localPlayerManager.LocalPlayer);

            camera.Speed = 4f;
        }

        public GameplayState() : this(0)
        {            
        }

        public GameplayState(short localPlayerId, Dictionary<short, Client> clientsInfo) : this(localPlayerId)
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
                GameStatesManager.ExitGame();

            if (inputManager.Pause)
                GameStatesManager.AddState(new PauseState());

            localPlayerManager.Update(gameTime, inputManager, mapManager.CollisionLayer);
            camera.FollowLocalPlayer(localPlayerManager.LocalPlayer);
            
            playerManager.Update(gameTime, mapManager.CollisionLayer);
            arrowManager.Update(gameTime, mapManager.CollisionLayer);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            mapManager.Draw(spriteBatch, camera.PositionToPoint, graphicsManager.ScreenSize);
            localPlayerManager.Draw(spriteBatch, spriteFont);
            playerManager.Draw(spriteBatch, spriteFont);
            arrowManager.Draw(spriteBatch, spriteFont);
        }

        // TODO: REVER PORQUE FUNCIONOU SEM O TILESIZE_X e TILESIZE_Y em Map.cs depois do refactoring do Guifes
        //private Vector2 ViewportVector(Point tileSize)
        //{
        //    return new Vector2(graphicsManager.ScreenSize.X + tileSize.X, graphicsManager.ScreenSize.Y + tileSize.Y);
        //}

        // Eventos de Network

        private void OnOtherClientPlayerStateUpdated(object sender, PlayerStateUpdatedEventArgs playerStateUpdatedEventArgs)
        {
            if (playerStateUpdatedEventArgs.PlayerId != localPlayerManager.playerId)
            {
                playerManager.UpdatePlayer(playerStateUpdatedEventArgs.PlayerId,
                                               playerStateUpdatedEventArgs.Position,
                                               playerStateUpdatedEventArgs.Speed,
                                               playerStateUpdatedEventArgs.LocalTime,
                                               playerStateUpdatedEventArgs.MovementType,
                                               playerStateUpdatedEventArgs.PlayerState);
            }
            else
            {
                // TODO: VERIFICAR SAPORRA, refactoring previsto em network manager
                Console.WriteLine("Olha a merda > " + playerStateUpdatedEventArgs.PlayerId);
            }
        }

        private void OnClientDisconnected(object sender, EventArgs eventArgs)
        {
            GameStatesManager.ResignState(this);
        }
    }
}
