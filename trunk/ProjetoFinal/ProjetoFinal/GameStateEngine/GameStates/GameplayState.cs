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
            //playerManager.Update(gameTime, mapManager.CollisionLayer);
            arrowManager.Update(gameTime, mapManager.CollisionLayer);
            camera.FollowLocalPlayer(localPlayerManager.LocalPlayer);

            /*
            foreach (EntityCollision entityCollision in EntityCollision.EntityCollisions)
            {
                Entity entityA = entityCollision.entityA;
                Entity entityB = entityCollision.entityB;

                if (entityA.OnCollision(entityB))
                    entityB.OnCollision(entityA);
            }
            EntityCollision.EntityCollisions.Clear();
             */
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

            //playerManager.Draw(spriteBatch, spriteFont);
            DebugSystem.Instance.TimeRuler.BeginMark("Arrows Draw", Color.Blue);
            arrowManager.Draw(spriteBatch, spriteFont);
            DebugSystem.Instance.TimeRuler.EndMark("Arrows Draw");
            //FPSCounter(spriteBatch, spriteFont, gameTime);
        }

        #region Network Events

        private void OnOtherClientPlayerMovementStateUpdated(object sender, PlayerMovementStateUpdatedEventArgs playerStateUpdatedEventArgs)
        {
            if (playerStateUpdatedEventArgs.PlayerId != localPlayerManager.playerId)
            {
                playerManager.UpdatePlayer(playerStateUpdatedEventArgs.PlayerId,
                                               playerStateUpdatedEventArgs.Position,
                                               playerStateUpdatedEventArgs.Speed,
                                               playerStateUpdatedEventArgs.LocalTime,
                                               playerStateUpdatedEventArgs.StateType,
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

        #endregion

        /*
        float fps;
        Texture2D debugBackground;
        Color debugColor;
        Rectangle debugRectangleBase;
        Vector2 fpsTextPos;
        private void FPSCounter(SpriteBatch spriteBatch, SpriteFont spriteFont, GameTime gameTime)
        {
            debugBackground = TextureManager.Instance.GetPixelTextureByColor(Color.Black);
            debugRectangleBase = new Rectangle(graphicsManager.ScreenSize.X - 80, 0, 100, 40);
            debugColor = new Color(0, 0, 0, 0.5f);
            fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            fpsTextPos = new Vector2(graphicsManager.ScreenSize.X - 65, 10);

            spriteBatch.Draw(debugBackground, debugRectangleBase, debugColor);
            spriteBatch.DrawString(spriteFont, "FPS: " + Math.Round(fps), fpsTextPos, Color.White);            
        }
         */
    }
}
