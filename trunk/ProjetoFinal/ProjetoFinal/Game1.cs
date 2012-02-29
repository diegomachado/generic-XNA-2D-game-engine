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
using ProjetoFinal.Entities;

namespace ProjetoFinal
{

    public class Game1 : Microsoft.Xna.Framework.Game
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

        // Game Object
        PlayerChar playerChar;

        public Game1()
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

            Texture2D texture = this.Content.Load<Texture2D>(@"sprites/bear");

            // Game Objects
            playerChar = new PlayerChar(texture, Vector2.Zero, 8);

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

            // Atualiza player
            playerChar.Update(gameTime, currentKeyboardState, currentGamePadState, this.Window.ClientBounds);

            // Processa mensagens
            ProcessNetworkMessages();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            playerChar.Draw(spriteBatch);

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
                            case NetConnectionStatus.Connected:
                                if (!this.IsHost)
                                {
                                    //var message = new UpdatePlayerStateMessage(im.SenderConnection.RemoteHailMessage);
                                    //this.playerManager.AddPlayer(message.Id, message.Position, message.Velocity, message.Rotation, true);
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
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                NetOutgoingMessage hailMessage = this.networkManager.CreateMessage();
                                //new UpdatePlayerStateMessage(playerManager.AddPlayer(false)).Encode(hailMessage);
                                im.SenderConnection.Approve(hailMessage);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        /*var gameMessageType = (GameMessageTypes)im.ReadByte();
                        switch (gameMessageType)
                        {
                            case GameMessageTypes.UpdateAsteroidState:
                                this.HandleUpdateAsteroidStateMessage(im);
                                break;
                            case GameMessageTypes.UpdatePlayerState:
                                this.HandleUpdatePlayerStateMessage(im);
                                break;
                        }*/
                        break;
                }

                this.networkManager.Recycle(im);
            }
        }
    }
}