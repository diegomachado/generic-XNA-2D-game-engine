using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Lidgren.Network;

using ProjetoFinal.Network;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers;

using OgmoLibrary;

namespace ProjetoFinal
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        // Menu Input
        Dictionary<string, string> options;

        // Network
        INetworkManager networkManager;
        short clientCounter;
        Dictionary<short, Client> clients;

        // Graphics
        //GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        
        // Input
        //KeyboardState currentKeyboardState;
        //KeyboardState previousKeyboardState;
        //GamePadState currentGamePadState;
        //GamePadState previousGamePadState;

        //public Game()
        //{
        //    graphics = new GraphicsDeviceManager(this);
        //    this.Window.Title = "Projeto Final";
        //    Content.RootDirectory = "Content";
        //}

        protected override void Initialize()
        {
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SegoeFont = Content.Load<SpriteFont>(@"fonts/SegoeUI");

            MapWidthInPixels = mapManager.GetMapSize();
            ScreenSize = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            localPlayerManager.Update(gameTime, currentKeyboardState, currentGamePadState, mapManager.GetCollisionLayer());
            playerManager.Update(gameTime, currentKeyboardState, currentGamePadState, mapManager.GetCollisionLayer());

            ProcessNetworkMessages();
            
            base.Update(gameTime);
        }        

        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Matrix scaleMatrix = Matrix.CreateScale(0.5f);
            
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, scaleMatrix);
            spriteBatch.Begin();

            // Drawing Entities

            mapManager.DrawEfficiently(spriteBatch,
                                       camera.PositionToPoint(),
                                       PositionToTileCoord(camera.Position, mapManager.GetTileSize()),
                                       PositionToTileCoord(camera.Position + ViewportVector(mapManager.GetTileSize()), mapManager.GetTileSize()));
      
            //mapManager.Draw(spriteBatch, camera.PositionToPoint());

            localPlayerManager.Draw(spriteBatch, SegoeFont);
            playerManager.Draw(spriteBatch, SegoeFont);

            // In Game Debug
            float frameRate;
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.DrawString(SegoeFont, "FPS: " + Math.Round(frameRate), new Vector2(ScreenSize.X - 70, 5), Color.White);
            
            spriteBatch.DrawString(SegoeFont, "Players:", new Vector2(ScreenSize.X - 70, 25), Color.White);
            
            Vector2 playersDebugTextPosition = new Vector2(ScreenSize.X - 200, 25);
            foreach (KeyValuePair<short, Client> client in clients)
            {
                playersDebugTextPosition.Y += 20;
                spriteBatch.DrawString(SegoeFont, client.Value.nickname, playersDebugTextPosition, Color.White);               
            }           

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Point PositionToTileCoord(Vector2 position, Point tileSize)
        {
            return new Point((int)position.X / tileSize.X, (int)position.Y / tileSize.Y);
        }

        private Vector2 ViewportVector(Point tileSize)
        {
            return new Vector2( ScreenSize.X + tileSize.X, ScreenSize.Y + tileSize.Y);
        }

 
    }
}