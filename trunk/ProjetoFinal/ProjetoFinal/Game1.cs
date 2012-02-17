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

//Debug
using System.Diagnostics;

namespace ProjetoFinal
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        Dictionary<long,Player> otherPlayers;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        NetClient client;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            NetPeerConfiguration config = new NetPeerConfiguration("projetofinal");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();
        }

        protected override void Initialize()
        {
            otherPlayers = new Dictionary<long,Player>();

            client.DiscoverLocalPeers(666);

            base.Initialize();
        }

        protected override void LoadContent()
       { 
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y
                                                    + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            Console.WriteLine("Entre com seu nick:");
            string nick = Console.ReadLine();
            player = new Player(nick, Content.Load<Texture2D>("sprites/bear"), playerPosition, 8.0f );
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            UpdatePlayer(gameTime);

            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            Vector2 playerPositionTemp = player.position;

            float prevXPosition = player.position.X;
            float prevYPosition = player.position.Y;

            player.position.X += currentGamePadState.ThumbSticks.Left.X * player.speed;
            player.position.Y -= currentGamePadState.ThumbSticks.Left.Y * player.speed;

            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)
                player.position.Y -= player.speed;
            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)
                player.position.Y += player.speed;
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)
                player.position.X -= player.speed;
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)
                player.position.X += player.speed;

            player.position.X = MathHelper.Clamp(player.position.X,0, GraphicsDevice.Viewport.Width - player.Width);
            player.position.Y = MathHelper.Clamp(player.position.Y,0, GraphicsDevice.Viewport.Height - player.Height);

            // Cliente envia atualizações pro Servidor
            if (prevXPosition != player.position.X || prevYPosition != player.position.Y)
            {
                NetOutgoingMessage om = client.CreateMessage();

                om.Write(player.position.X);
                om.Write(player.position.Y);
                om.Write(player.nickname);

                client.SendMessage(om, NetDeliveryMethod.Unreliable);
            }

            // Cliente recebe atualizações do Servidor          
            NetIncomingMessage im;
            while ((im = client.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        client.Connect(im.SenderEndpoint);
                        break;

                    case NetIncomingMessageType.Data:
                        long id = im.ReadInt64();
                        //string nick = im.ReadString();
                        float x = im.ReadFloat();
                        float y = im.ReadFloat();

                        Console.WriteLine(id + " online; X = " + x + "; Y = " + y);
                        //Console.WriteLine(nick + " online; X = " + x + "; Y = " + y );

                        //if (!otherPlayers.ContainsKey(id))
                        //    otherPlayers[id] = new Player(Content.Load<Texture2D>("sprites/bear"), new Vector2(x, y), 8.0f);
                        //else
                        //    otherPlayers[id].position = new Vector2(x, y);

                        break;
                }
            }
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            player.Draw(spriteBatch);

            foreach (var p in otherPlayers)
                p.Value.Draw(spriteBatch);

            spriteBatch.End();            

            base.Draw(gameTime);
        }

        // Desconecta o cliente do servidor ao fechar o jogo
        protected override void OnExiting(object sender, EventArgs args)
        {
            client.Shutdown("bye");

            base.OnExiting(sender, args);
        }
    }
}

