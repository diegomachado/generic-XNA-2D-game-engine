using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PerformanceUtility.GameDebugTools;

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
        LevelManager levelManager = LevelManager.Instance;
        NetworkManager networkManager = NetworkManager.Instance;
        Camera camera = Camera.Instance;

        public GameplayState()
        {
            eventManager.PlayerMovementStateUpdated += OnOtherClientPlayerMovementStateUpdated;
            eventManager.PlayerStateUpdated += OnOtherClientPlayerStateUpdated;
            eventManager.PlayerStateUpdatedWithArrow += OnOtherClientPlayerStateUpdatedWithArrow;
            eventManager.PlayerStateChangedWithArrow += OnLocalArrowShot;
            eventManager.Disconnected += OnDisconnected;
            eventManager.ClientDisconnected += OnClientDisconnected;
            eventManager.PlayerHitUpdated += OnPlayerHitUpdated;
            eventManager.PlayerSpawnedUpdated += OnPlayerSpawnedUpdated;
            eventManager.PlayerCreatedUpdated += OnPlayerCreatedUpdated;
            eventManager.NewClientPlayerCreatedUpdated += OnNewClientPlayerCreatedUpdated;

            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();
            arrowManager = new ArrowManager(localPlayerManager.localPlayer);
        }

        public GameplayState(Dictionary<short, Client> clientsInfo) : this()
        {
            // TODO: Criar o Player aqui, com as infos enviadas ao ele ter se conectado
            foreach (short id in clientsInfo.Keys)
                this.playerManager.AddPlayer(id, new Vector2(240, 240));
        }

        public override void Initialize(Game game, SpriteFont spriteFont)
        {
            base.Initialize(game, spriteFont);
        }

        public override void LoadContent(ContentManager content) 
        {
            levelManager.Content = content;
            levelManager.LoadLevel("battlefield");
            localPlayerManager.createLocalPlayer(networkManager.LocalPlayerId);
            localPlayerManager.localPlayer.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            DebugSystem.Instance.TimeRuler.StartFrame();

            if (inputManager.ToggleFPS)
                DebugSystem.Instance.FpsCounter.Visible = !DebugSystem.Instance.FpsCounter.Visible;

            if (inputManager.ToggleRuler)
                DebugSystem.Instance.TimeRuler.Visible = !DebugSystem.Instance.TimeRuler.Visible;

            if (inputManager.ToggleRulerLog)
            {
                DebugSystem.Instance.TimeRuler.Visible = true;
                DebugSystem.Instance.TimeRuler.ShowLog = !DebugSystem.Instance.TimeRuler.ShowLog;
            }

            if (inputManager.Exit)
                GameStatesManager.ExitGame();

            if (inputManager.Pause)
                GameStatesManager.AddState(new PauseState());

            localPlayerManager.Update(gameTime);
            playerManager.Update(gameTime, levelManager.Grid);
            arrowManager.Update(gameTime);
            camera.FollowLocalPlayer(localPlayerManager.localPlayer);
            
            foreach (EntityCollision entityCollision in EntityCollision.EntityCollisions)
            {
                Entity entityA = entityCollision.entityA;
                Entity entityB = entityCollision.entityB;

                if (entityA.OnCollision(entityB))
                    entityB.OnCollision(entityA);
            }
            EntityCollision.EntityCollisions.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            DebugSystem.Instance.TimeRuler.StartFrame();

            DebugSystem.Instance.TimeRuler.BeginMark("Level Draw", Color.Red);
            levelManager.Draw(spriteBatch, camera.PositionToPoint, graphicsManager.screen);
            DebugSystem.Instance.TimeRuler.EndMark("Level Draw");

            DebugSystem.Instance.TimeRuler.BeginMark("Player Draw", Color.Yellow);
            localPlayerManager.Draw(spriteBatch, spriteFont);
            DebugSystem.Instance.TimeRuler.EndMark("Player Draw");

            playerManager.Draw(spriteBatch, spriteFont);
            DebugSystem.Instance.TimeRuler.BeginMark("Arrows Draw", Color.Blue);
            arrowManager.Draw(spriteBatch, spriteFont);
            DebugSystem.Instance.TimeRuler.EndMark("Arrows Draw");

            DrawEntitiesCount(spriteBatch, spriteFont);
        }

        private void DrawEntitiesCount(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(), new Rectangle(GraphicsManager.Instance.screen.X - 157, 7, 150, 40), Color.Black * 0.5f);
            spriteBatch.DrawString(spriteFont, Entity.Entities.Count + " Entities", new Vector2(GraphicsManager.Instance.screen.X - 147, 15), Color.White);            
        }

        #region Network Events

        private void OnOtherClientPlayerStateUpdated(object sender, PlayerStateUpdatedEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
            {
                playerManager.UpdatePlayerState(args.PlayerId,
                                                args.Position,
                                                args.LocalTime,
                                                args.StateType,
                                                args.PlayerState);
            }
            else
            {
                // TODO: VERIFICAR SAPORRA, refactoring previsto em network manager
                //Console.WriteLine("Olha a merda > " + args.PlayerId);
            }
        }

        private void OnOtherClientPlayerMovementStateUpdated(object sender, PlayerMovementStateUpdatedEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
            {
                playerManager.UpdatePlayerMovementState(args.PlayerId,
                                                        args.Position,
                                                        args.Speed,
                                                        args.LocalTime,
                                                        args.StateType,
                                                        args.PlayerState);
            }
            else
            {
                // TODO: VERIFICAR SAPORRA, refactoring previsto em network manager
                //Console.WriteLine("Olha a merda > " + args.PlayerId);
            }
        }

        private void OnOtherClientPlayerStateUpdatedWithArrow(object sender, PlayerStateUpdatedWithArrowEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
            {
                OnOtherClientPlayerStateUpdated(sender, new PlayerStateUpdatedEventArgs(args.PlayerId, args.Position, args.PlayerState, args.StateType, args.MessageTime, args.LocalTime));

                playerManager.GetPlayer(args.PlayerId).FacingRight = (args.ShotSpeed.X > 0);

                arrowManager.CreateArrow(args.PlayerId, args.Position, args.ShotSpeed);
            }
            else
            {
                // TODO: VERIFICAR SAPORRA, refactoring previsto em network manager
                //Console.WriteLine("Olha a merda > " + args.PlayerId);
            }
        }

        private void OnLocalArrowShot(object sender, PlayerStateChangedWithArrowEventArgs args)
        {
            arrowManager.CreateArrow(args.PlayerId, args.Position, args.ShotSpeed);
        }

        private void OnDisconnected(object sender, EventArgs eventArgs)
        {
            GameStatesManager.ResignState(this);
        }

        private void OnClientDisconnected(object sender, PlayerIdEventArgs args)
        {
            playerManager.RemovePlayer(args.PlayerId);
        }

        private void OnPlayerHitUpdated(object sender, PlayerHitEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
                playerManager.GetPlayer(args.PlayerId).TakeHit();
        }

        private void OnPlayerSpawnedUpdated(object sender, PlayerSpawnedEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
                playerManager.GetPlayer(args.PlayerId).Respawn(args.SpawnPoint);
        }

        private void OnPlayerCreatedUpdated(object sender, PlayerSpawnedEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
            {
                playerManager.GetPlayer(args.PlayerId).Respawn(args.SpawnPoint);

                Dictionary<short, Vector2> playerPositions = new Dictionary<short, Vector2>();

                playerPositions.Add(localPlayerManager.playerId, localPlayerManager.localPlayer.position);

                foreach (KeyValuePair<short, Player> player in playerManager.players)
                    if(player.Key != args.PlayerId)
                        playerPositions.Add(player.Key, player.Value.position);

                eventManager.ThrowNewClientPlayerCreated(this, new NewClientPlayerCreatedEventArgs(args.PlayerId, playerPositions));
            }
        }

        private void OnNewClientPlayerCreatedUpdated(object sender, NewClientPlayerCreatedEventArgs args)
        {
            foreach (KeyValuePair<short, Vector2> playerPositions in args.PlayerPositions)
            {
                //Console.WriteLine(localPlayerManager.playerId + " >>> " + playerPositions.Key);
                playerManager.GetPlayer(playerPositions.Key).Respawn(playerPositions.Value);
            }
        }

        #endregion
    }
}
