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
        MapManager mapManager = MapManager.Instance;
        Camera camera = Camera.Instance;

        public GameplayState(short localPlayerId) : base()
        {
            eventManager.PlayerMovementStateUpdated += OnOtherClientPlayerMovementStateUpdated;
            eventManager.PlayerStateUpdated += OnOtherClientPlayerStateUpdated;
            eventManager.PlayerStateUpdatedWithArrow += OnOtherClientPlayerStateUpdatedWithArrow;
            eventManager.PlayerStateChangedWithArrow += OnLocalArrowShot;
            eventManager.ClientDisconnected += OnClientDisconnected;

            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();
            localPlayerManager.createLocalPlayer(localPlayerId);
            arrowManager = new ArrowManager(localPlayerId, localPlayerManager.LocalPlayer);
        }

        public GameplayState() : this(0)
        {            
        }

        public GameplayState(short localPlayerId, Dictionary<short, Client> clientsInfo) : this(localPlayerId)
        {
            foreach (short id in clientsInfo.Keys)
                this.playerManager.AddPlayer(id);
        }

        public override void Initialize(Game game, SpriteFont spriteFont)
        {
            base.Initialize(game, spriteFont);
        }

        public override void LoadContent(ContentManager content) 
        {
            mapManager.Content = content;
            mapManager.LoadMap(MapType.Level1);
            localPlayerManager.LocalPlayer.LoadContent();
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
            playerManager.Update(gameTime, mapManager.CollisionLayer);
            arrowManager.Update(gameTime, mapManager.CollisionLayer);
            camera.FollowLocalPlayer(localPlayerManager.LocalPlayer);
            
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

            DebugSystem.Instance.TimeRuler.BeginMark("Map Draw", Color.Red);
            mapManager.Draw(spriteBatch, camera.PositionToPoint, graphicsManager.ScreenSize);
            DebugSystem.Instance.TimeRuler.EndMark("Map Draw");

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
            spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(), new Rectangle(GraphicsManager.Instance.ScreenSize.X - 157, 7, 150, 40), Color.Black * 0.5f);
            spriteBatch.DrawString(spriteFont, Entity.Entities.Count + " Entities", new Vector2(GraphicsManager.Instance.ScreenSize.X - 147, 15), Color.White);            
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
                Console.WriteLine("Olha a merda > " + args.PlayerId);
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
                Console.WriteLine("Olha a merda > " + args.PlayerId);
            }
        }

        private void OnOtherClientPlayerStateUpdatedWithArrow(object sender, PlayerStateUpdatedWithArrowEventArgs args)
        {
            if (args.PlayerId != localPlayerManager.playerId)
            {
                OnOtherClientPlayerStateUpdated(sender, new PlayerStateUpdatedEventArgs(args.PlayerId, args.Position, args.PlayerState, args.StateType, args.MessageTime, args.LocalTime));

                arrowManager.CreateArrow(args.PlayerId, args.Position, args.ShotSpeed);
            }
            else
            {
                // TODO: VERIFICAR SAPORRA, refactoring previsto em network manager
                Console.WriteLine("Olha a merda > " + args.PlayerId);
            }
        }

        private void OnLocalArrowShot(object sender, PlayerStateChangedWithArrowEventArgs args)
        {
            arrowManager.CreateArrow(args.PlayerId, args.Position, args.ShotSpeed);
        }

        private void OnClientDisconnected(object sender, EventArgs eventArgs)
        {
            GameStatesManager.ResignState(this);
        }

        #endregion
    }
}
