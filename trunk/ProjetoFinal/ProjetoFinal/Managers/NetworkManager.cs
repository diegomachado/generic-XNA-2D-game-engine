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
        public short clientCounter; // TODO: Fazer property
        public Dictionary<short, Entities.Client> clients; // TODO: Fazer property

        public bool IsServer { get { return networkInterface is ServerInterface; } }
        public bool IsConnected { get { return networkInterface != null; } }

        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkManager();
                    instance.eventManager = EventManager.Instance;
                    instance.clients = new Dictionary<short, Client>();
                }

                return instance;
            }
        }
        
        public void Host(int port, string nickname)
        {
            ServerInterface serverNetworkManager = new ServerInterface();
            serverNetworkManager.port = port;

            networkInterface = serverNetworkManager;

            // TODO: Vai adcionar outro cara no index 0 quando passar aqui denovo, ajeitar isso
            clients.Add(0, new Client("[SERVER] " + nickname));
 
            networkInterface.Connect();
            clientCounter = 1;

            eventManager.PlayerStateChanged += OnPlayerStateChanged;
        }

        public void Connect(String ip, int port, string nickname)
        {
            ClientInterface clientNetworkManager = new ClientInterface();
            clientNetworkManager.port = port;
            clientNetworkManager.ip = ip;

            networkInterface = clientNetworkManager;

            // TODO: Vai adcionar outro cara no index 0 quando passar aqui denovo, ajeitar isso
            clients.Add(0, new Client("[CLIENT] " + nickname));
        
            networkInterface.Connect();
            clientCounter = 1;  //TODO: Se eu for cliente aqui, meu clientCounter não é 1 (ver se não é setado depois)

            eventManager.PlayerStateChanged += OnPlayerStateChanged;
        }

        public void Disconnect()
        {
            networkInterface.Disconnect();

            eventManager.PlayerStateChanged -= OnPlayerStateChanged;
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
                                    // TODO: Server poderia nesse momento, disparar uma mensagem a todos dizendo que um novo jogador se conectou para que cada client crie o boneco no seu jogo
                                    // TODO: O proprio server tambem tem que criar o boneco no seu jogo
                                    Console.WriteLine("{0} Connected", im.SenderEndpoint);
                                }

                                break;

                            case NetConnectionStatus.Disconnected:

                                if (!IsServer)
                                {
                                    OnClientDisconnected();

                                    Console.WriteLine("Disconnected from {0}", im.SenderEndpoint);
                                }
                                else
                                {
                                    // TODO: Server poderia nesse momento, disparar uma mensagem a todos dizendo que um jogador se desconectou para que cada client destrua o boneco no seu jogo
                                    // TODO: O proprio server tambem tem que destruir o boneco no seu jogo
                                    Console.WriteLine("{0} Disconnected", im.SenderEndpoint);
                                }

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
                                
                                // If server, resend UpdatePlayerState to all clients
                                // TODO: Refactor this shit so that a client doesn't receive it's own message back
                                // This fucking shit ta causando um overhead chato na rede e acho que tem como consertar usando SendMessage ao inves de SendToAll no serverNetworkInterface porém como a porra do código é fechado temos que fazer testes manuais pra saber se a gente vai estar ganhando desempenho ou perdendo.
                                if(IsServer)
                                    networkInterface.SendMessage(updatePlayerStateMessage);

                                break;
                        }

                        break;
                }

                networkInterface.Recycle(im);
            }
        }

        // Eventos

        // Outgoing

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnPlayerStateUpdated(UpdatePlayerStateMessage updatePlayerStateMessage, double localTime)
        {
            eventManager.ThrowPlayerStateUpdated(this, new PlayerStateUpdatedEventArgs(updatePlayerStateMessage, localTime));
        }

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnClientConnected(HailMessage hailMessage)
        {
            eventManager.ThrowClientConnected(this, new ClientConnectedEventArgs(hailMessage));
        }

        private void OnClientDisconnected()
        {
            eventManager.ThrowClientDisconnected(this, null);
        }

        // Incoming

        private void OnPlayerStateChanged(object sender, PlayerStateChangedEventArgs playerStateChangedEventArgs)
        {
            SendPlayerStateChangedMessage(playerStateChangedEventArgs.playerId, playerStateChangedEventArgs.player, playerStateChangedEventArgs.movementType);
        }

        // Util

        private NetOutgoingMessage CreateHailMessage()
        {
            NetOutgoingMessage hailMessage = networkInterface.CreateMessage();
            new HailMessage(clientCounter++, clients).Encode(hailMessage);
            return hailMessage;
        }

        // Outgoing Messages

        private void SendPlayerStateChangedMessage(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            networkInterface.SendMessage(new UpdatePlayerStateMessage(id, player, messageType));
        }

        private void SendCreateArrowMessage(short id, Player player)
        {
            networkInterface.SendMessage(new CreateArrowMessage(id, player));
        }
    }
}
