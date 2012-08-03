using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.Network;
using ProjetoFinal.Entities;
using ProjetoFinal.EventHeaders;

namespace ProjetoFinal.Managers
{
    class NetworkManager
    {
        private static NetworkManager instance;

        // TODO: Renomear esse fdp
        INetworkManager someShit;

        public bool IsServer { get; set; }
        public short clientCounter; // TODO: Fazer property
        public Dictionary<short, Entities.Client> clients; // TODO: Fazer property

        // Events
        public event EventHandler<PlayerStateUpdatedEventArgs> PlayerStateUpdated;
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new NetworkManager();

                return instance;
            }
        }

        public bool IsHost { get { return someShit is ServerNetworkManager; } }
        
        // Método utilizado para realizar uma conexão de client com server ou para criar um servidor
        public void Connect()
        {
            clients = new Dictionary<short, Client>();

            // Network
            // TODO: Definir IP e Porta dinamicamente
            //if(IsServer)
            if(true)
            {
                ServerNetworkManager serverNetworkManager = new ServerNetworkManager();
                serverNetworkManager.port = 666;

                someShit = serverNetworkManager;

                clients.Add(0, new Client("[SERVER]"));
            }
            else
            {
                ClientNetworkManager clientNetworkManager = new ClientNetworkManager();
                clientNetworkManager.port = 666;
                clientNetworkManager.ip = "localhost";

                someShit = clientNetworkManager;

                clients.Add(0, new Client("[CLIENT]"));
            }

            someShit.Connect();
            clientCounter = 1;  //TODO: Se eu for cliente aqui, meu clientCounter não é 1 (ver se não é setado depois)
        }

        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            // TODO: Someshit
            while ((im = someShit.ReadMessage()) != null)
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

                                im.SenderConnection.Approve(CreateHailMessage());

                                break;

                            case NetConnectionStatus.Connected:

                                if (!IsHost)
                                {
                                    OnClientConnected(new HailMessage(im.SenderConnection.RemoteHailMessage));

                                    Console.WriteLine("Connected to {0}", im.SenderEndpoint);
                                }
                                else
                                {
                                    Console.WriteLine("{0} Connected", im.SenderEndpoint);
                                }

                                break;

                            case NetConnectionStatus.Disconnected:

                                Console.WriteLine(IsHost ? "{0} Disconnected" : "Disconnected from {0}", im.SenderEndpoint);

                                break;

                        }

                        break;

                    case NetIncomingMessageType.Data:

                        var gameMessageType = (GameMessageType)im.ReadByte();

                        switch (gameMessageType)
                        {
                            case GameMessageType.UpdatePlayerState:
                                // TODO: Lançar evento de Recebimento de UpdatePlayerStateMessage
                                OnPlayerStateUpdated(new UpdatePlayerStateMessage(im));

                                break;
                        }

                        break;
                }

                someShit.Recycle(im);
            }
        }

        // Eventos

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnPlayerStateUpdated(UpdatePlayerStateMessage updatePlayerStateMessage)
        {
            if (PlayerStateUpdated != null)
                PlayerStateUpdated(this, new PlayerStateUpdatedEventArgs(updatePlayerStateMessage));
        }

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnClientConnected(HailMessage hailMessage)
        {
            if (ClientConnected != null)
                ClientConnected(this, new ClientConnectedEventArgs(hailMessage));
        }

        // Util

        private NetOutgoingMessage CreateHailMessage()
        {
            NetOutgoingMessage hailMessage = someShit.CreateMessage();
            new HailMessage(clientCounter++, clients).Encode(hailMessage);
            return hailMessage;
        }

        public void SendPlayerStateChangedMessage(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            someShit.SendMessage(new UpdatePlayerStateMessage(id, player, messageType));
        }

        /*private void HandleUpdatePlayerStateMessage(object sender, EventArgs e)
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
        /*}*/

        /*private void HandleHailMessage(object sender, EventArgs e)
        {
            //private void HandleHailMessage(HailMessage message)

            //localPlayerManager.createLocalPlayer(message.clientId);

            //foreach (short id in message.clientsInfo.Keys)
            //    this.playerManager.AddPlayer(id);
        }*/
    }
}
