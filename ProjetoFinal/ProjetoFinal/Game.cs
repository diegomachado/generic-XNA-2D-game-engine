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

namespace ProjetoFinal
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        // Network
        INetworkManager networkManager;

        // Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Input
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // Managers
        LocalPlayerManager localPlayerManager;
        PlayerManager playerManager;
        TextureManager textureManager;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Network
            // TODO: Definir IP e Porta dinamicamente
            networkManager = SelectMenu("localhost", 666);
            networkManager.Connect();

            // Resources
            textureManager = TextureManager.Instance;
            textureManager.setContent(Content);

            // Game Objects
            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();
            
            // Registering Events
            this.localPlayerManager.PlayerStateChanged += (sender, e) => this.networkManager.SendMessage(new UpdatePlayerStateMessage(e.player));

            // Window Management
            graphics.PreferredBackBufferWidth = 200;
            graphics.PreferredBackBufferHeight = 200;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            localPlayerManager.Update(gameTime, currentKeyboardState, currentGamePadState, this.Window.ClientBounds);

            ProcessNetworkMessages();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            localPlayerManager.Draw(spriteBatch);
            playerManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool IsHost
        {
            get
            {
                return this.networkManager is ServerNetworkManager;
            }
        }

        // TODO: Achar um nome mais expressivo pra essa função
        private INetworkManager SelectMenu(String ip, int port)
        {
            int type;           

            Console.WriteLine("==========================");
            Console.WriteLine("       What are you?      ");
            Console.WriteLine("==========================");
            Console.WriteLine("1. I'm a Server");
            Console.WriteLine("2. I'm a Client");
            type = int.Parse(Console.ReadLine());

            //Console.WriteLine("Port?");
            //port = int.Parse(Console.ReadLine());

            switch (type)
            {
                case 1:
                    ServerNetworkManager serverNetworkManager = new ServerNetworkManager();
                    serverNetworkManager.port = port;

                    networkManager = serverNetworkManager;

                    break;
                case 2:
                    //Console.WriteLine("IP?");
                    //ip = Console.ReadLine();

                    ClientNetworkManager clientNetworkManager = new ClientNetworkManager();
                    clientNetworkManager.port = port;
                    clientNetworkManager.ip = ip;

                    networkManager = clientNetworkManager;

                    break;
            }

            return networkManager;
        }

        private void HandleUpdatePlayerStateMessage(NetIncomingMessage im)
        {
            UpdatePlayerStateMessage message = new UpdatePlayerStateMessage(im);

            //var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MessageTime));

            Player player = this.playerManager.GetPlayer(message.id) ??
                            this.playerManager.AddPlayer(message.id, textureManager.getTexture(TextureList.Ranger), message.position);

            //player.EnableSmoothing = true;

            if (player.LastUpdateTime < message.messageTime)
            {
                //player.SimulationState.Position = message.Position += (message.Velocity * timeDelay);
                //player.SimulationState.Velocity = message.Velocity;
                //player.SimulationState.Rotation = message.Rotation;

                player.position = message.position;

                player.LastUpdateTime = message.messageTime;
            }
        }

        private void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.networkManager.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(im.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                NetOutgoingMessage hailMessage = this.networkManager.CreateMessage();
                                new UpdatePlayerStateMessage(playerManager.AddPlayer(textureManager.getTexture(TextureList.Bear))).Encode(hailMessage);
                                im.SenderConnection.Approve(hailMessage);

                                break;

                            case NetConnectionStatus.Connected:
                                if (!this.IsHost)
                                {
                                    UpdatePlayerStateMessage message = new UpdatePlayerStateMessage(im.SenderConnection.RemoteHailMessage);
                                    this.playerManager.AddPlayer(message.id, textureManager.getTexture(TextureList.Bear), message.position);
                                    Console.WriteLine("Connected to {0}", im.SenderEndpoint);
                                }
                                else
                                {
                                    Console.WriteLine("{0} Connected", im.SenderEndpoint);
                                }
                                break;

                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine(this.IsHost ? "{0} Disconnected" : "Disconnected from {0}", im.SenderEndpoint);
                                break;
                            
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        var gameMessageType = (GameMessageTypes)im.ReadByte();
                        switch (gameMessageType)
                        {
                            case GameMessageTypes.UpdatePlayerState:
                                this.HandleUpdatePlayerStateMessage(im);
                                break;
                        }
                        break;

                }

                this.networkManager.Recycle(im);
            }
        }
    }
}