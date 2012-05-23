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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont SegoeFont;
        
        // Input
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // Managers
        PlayerManager playerManager;
        LocalPlayerManager localPlayerManager;
        TextureManager textureManager;
        EventManager eventManager;
        MapManager mapManager;

        // Camera Globals
        Camera camera;       
        public static Point ScreenSize { get; set; }
        public static Point MapWidthInPixels { get; set; }

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.Title = "Projeto Final";
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            options = SelectMenu();
            clients = new Dictionary<short, Client>();
            
            // Network
            // TODO: Definir IP e Porta dinamicamente
            switch (int.Parse(options["type"]))
            {
                case 1:
                    ServerNetworkManager serverNetworkManager = new ServerNetworkManager();
                    serverNetworkManager.port = 666;

                    networkManager = serverNetworkManager;

                    clients.Add(0, new Client("[SERVER]" + options["nickname"]));

                    break;
                case 2:
                    //Console.WriteLine("IP?");
                    //ip = Console.ReadLine();

                    ClientNetworkManager clientNetworkManager = new ClientNetworkManager();
                    clientNetworkManager.port = 666;
                    clientNetworkManager.ip = "localhost";

                    networkManager = clientNetworkManager;
                    
                    clients.Add(0, new Client("[CLIENT]" + options["nickname"]));

                    break;
            }

            networkManager.Connect();
            clientCounter = 1;  //TODO: Se eu for cliente aqui, meu clientCounter n�o � 1 (ver se n�o � setado depois)

            // Resources
            textureManager = TextureManager.Instance;
            textureManager.setContent(Content, GraphicsDevice);

            // Events
            eventManager = EventManager.Instance;

            // Managers
            playerManager = new PlayerManager();
            localPlayerManager = new LocalPlayerManager();
            mapManager = new MapManager(Content);

            if (IsHost)
                localPlayerManager.createLocalPlayer(0);

            // Registering Events
            eventManager.PlayerStateChanged += (sender, e) => this.networkManager.SendMessage(new UpdatePlayerStateMessage(e.id, e.player));

            // Window Management
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 608;
            graphics.ApplyChanges();    
        
            // Camera
            camera = Camera.Instance;
            camera.Speed = 4f;
                                    
            base.Initialize();
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
            playerManager.Update();

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

        private bool IsHost
        {
            get { return this.networkManager is ServerNetworkManager; }
        }

        // TODO: Achar um nome mais expressivo pra essa fun��o
        private Dictionary<string, string> SelectMenu()
        {
            Dictionary<string, string> returnValues = new Dictionary<string, string>();

            Console.WriteLine("==========================");
            Console.WriteLine("       What are you?      ");
            Console.WriteLine("==========================");
            Console.WriteLine("1. I'm a Server");
            Console.WriteLine("2. I'm a Client");
            returnValues.Add("type", Console.ReadLine());
            //Console.WriteLine("Type your nickname:");
            //returnValues.Add("nickname", Console.ReadLine());
            returnValues.Add("nickname", "BomberMacFaggot");
            
            //Console.WriteLine("Port?");
            //port = int.Parse(Console.ReadLine());

            return returnValues;
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
                                new HailMessage(clientCounter++, clients).Encode(hailMessage);
                                im.SenderConnection.Approve(hailMessage);

                                break;

                            case NetConnectionStatus.Connected:
                                if (!this.IsHost)
                                {
                                    this.HandleHailMessage(new HailMessage(im.SenderConnection.RemoteHailMessage));
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
                        var gameMessageType = (GameMessageType)im.ReadByte();
                        switch (gameMessageType)
                        {
                            case GameMessageType.UpdatePlayerState:
                                this.HandleUpdatePlayerStateMessage(im);

                                break;
                        }
                        break;

                }

                this.networkManager.Recycle(im);
            }
        }

        private void HandleUpdatePlayerStateMessage(NetIncomingMessage im)
        {
            UpdatePlayerStateMessage message = new UpdatePlayerStateMessage(im);

            if (message.playerId != localPlayerManager.playerId)
            {
                Player player = this.playerManager.GetPlayer(message.playerId);

                // TODO: Tentar implementar algo de Lag Prediction
                //var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.messageTime));

                if (player.LastUpdateTime < message.messageTime)
                {
                    //player.position = message.position += (message.speed * timeDelay);
                    player.Position = message.position;
                    player.State = message.playerState;

                    player.LastUpdateTime = message.messageTime;
                }

                if (IsHost)
                    networkManager.SendMessage(new UpdatePlayerStateMessage(message.playerId, player));
            }
        }

        private void HandleHailMessage(HailMessage message)
        {
            localPlayerManager.createLocalPlayer(message.clientId);

            foreach (short id in message.clientsInfo.Keys)
                this.playerManager.AddPlayer(id);
        }
    }
}