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
        private EventManager eventManager;
        private NetworkInterface networkInterface;

        public bool IsServer 
        { 
            get
            {
                return networkInterface is ServerInterface;
            }
        }
        public short clientCounter; // TODO: Fazer property
        public Dictionary<short, Entities.Client> clients; // TODO: Fazer property
        
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkManager();
                    instance.eventManager = EventManager.Instance;
                }

                return instance;
            }
        }
        
        public void Host()
        {
            clients = new Dictionary<short, Client>();

            // Network
            // TODO: Definir IP e Porta dinamicamente
            ServerInterface serverNetworkManager = new ServerInterface();
            serverNetworkManager.port = 666;

            networkInterface = serverNetworkManager;

            clients.Add(0, new Client("[SERVER]"));
 
            networkInterface.Connect();
            clientCounter = 1;
        }

        // Método utilizado para realizar uma conexão de client com server ou para criar um servidor
        public void Connect()
        {
            clients = new Dictionary<short, Client>();

            // Network
            // TODO: Definir IP e Porta dinamicamente
            ClientInterface clientNetworkManager = new ClientInterface();
            clientNetworkManager.port = 666;
            clientNetworkManager.ip = "localhost";

            networkInterface = clientNetworkManager;

            clients.Add(0, new Client("[CLIENT]"));
        
            networkInterface.Connect();
            clientCounter = 1;  //TODO: Se eu for cliente aqui, meu clientCounter não é 1 (ver se não é setado depois)
        }

        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = networkInterface.ReadMessage()) != null)
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

                                if (!IsServer)
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

                                Console.WriteLine(IsServer ? "{0} Disconnected" : "Disconnected from {0}", im.SenderEndpoint);

                                break;

                        }

                        break;

                    case NetIncomingMessageType.Data:

                        var gameMessageType = (GameMessageType)im.ReadByte();

                        switch (gameMessageType)
                        {
                            case GameMessageType.UpdatePlayerState:

                                UpdatePlayerStateMessage updatePlayerStateMessage = new UpdatePlayerStateMessage(im);
                                double localTime = im.SenderConnection.GetLocalTime(updatePlayerStateMessage.messageTime);

                                OnPlayerStateUpdated(updatePlayerStateMessage, localTime);

                                break;
                        }

                        break;
                }

                networkInterface.Recycle(im);
            }
        }

        // Eventos

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnPlayerStateUpdated(UpdatePlayerStateMessage updatePlayerStateMessage, double localTime)
        {
            eventManager.ThrowPlayerStateUpdated(this, new PlayerStateUpdatedEventArgs(updatePlayerStateMessage, localTime));
        }

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnClientConnected(HailMessage hailMessage)
        {
            eventManager.throwClientConnected(this, new ClientConnectedEventArgs(hailMessage));
        }

        // Util
        private NetOutgoingMessage CreateHailMessage()
        {
            NetOutgoingMessage hailMessage = networkInterface.CreateMessage();
            new HailMessage(clientCounter++, clients).Encode(hailMessage);
            return hailMessage;
        }

        // Outgoing Messages
        public void SendPlayerStateChangedMessage(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            networkInterface.SendMessage(new UpdatePlayerStateMessage(id, player, messageType));
        }
    }
}
