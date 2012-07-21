using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.Network;
using ProjetoFinal.Entities;

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

        public event EventHandler UpdatePlayerStateMessageReceived;
        public event EventHandler HailMessageReceived;
        
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
            if(IsServer)
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
                                    // TODO: Lançar evento de Recebimento de HailStateMessage
                                    OnHailMessageReceived(new HailMessage(im.SenderConnection.RemoteHailMessage));

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
                                OnUpdatePlayerStateMessageReceived(im);

                                break;
                        }

                        break;
                }

                someShit.Recycle(im);
            }
        }

        // Eventos

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnUpdatePlayerStateMessageReceived(NetIncomingMessage im)
        {
            if (UpdatePlayerStateMessageReceived != null)
                UpdatePlayerStateMessageReceived(this, null);
        }

        // TODO: Desconstruir mensagem aqui dentro e passar as informações dela pelos args
        private void OnHailMessageReceived(HailMessage hailMessage)
        {
            if (UpdatePlayerStateMessageReceived != null)
                UpdatePlayerStateMessageReceived(this, null);
        }

        // Util

        public void SendMessage(Network.Messages.UpdatePlayerStateMessage updatePlayerStateMessage)
        {
            someShit.SendMessage(updatePlayerStateMessage);
        }

        private NetOutgoingMessage CreateHailMessage()
        {
            NetOutgoingMessage hailMessage = someShit.CreateMessage();
            new HailMessage(clientCounter++, clients).Encode(hailMessage);
            return hailMessage;
        }
    }
}
