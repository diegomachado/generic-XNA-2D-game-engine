using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.Network;
using ProjetoFinal.Entities;
using ProjetoFinal.EventHeaders;
using Microsoft.Xna.Framework;
using ProjetoFinal.Network.Messages.UpdatePlayerStateMessages;

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
        public short LocalPlayerId { get; private set; }

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
        
        // TODO: Fazer Inicialização comum aos dois metodos
        public void Host(int port, string nickname)
        {
            ServerInterface serverNetworkManager = new ServerInterface();
            serverNetworkManager.port = port;

            networkInterface = serverNetworkManager;

            // TODO: Vai adcionar outro cara no index 0 quando passar aqui denovo, ajeitar isso
            clients.Add(0, new Client("[SERVER] " + nickname));
 
            networkInterface.Connect();
            clientCounter = 1;
            LocalPlayerId = 0;

            eventManager.PlayerMovementStateChanged += OnPlayerMovementStateChanged;
            eventManager.PlayerStateChangedWithArrow += OnPlayerStateChangedWithArrow;
            eventManager.PlayerHit += OnPlayerHit;
            eventManager.PlayerSpawned += OnPlayerSpawned;
            eventManager.PlayerCreated += OnPlayerCreated;
            eventManager.NewClientPlayerCreated += OnNewClientPlayerCreated;
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

            eventManager.PlayerMovementStateChanged += OnPlayerMovementStateChanged;
            eventManager.PlayerStateChangedWithArrow += OnPlayerStateChangedWithArrow;
            eventManager.PlayerHit += OnPlayerHit;
            eventManager.PlayerSpawned += OnPlayerSpawned;
            eventManager.PlayerCreated += OnPlayerCreated;
        }

        public void Disconnect()
        {
            networkInterface.Disconnect();

            eventManager.PlayerMovementStateChanged -= OnPlayerMovementStateChanged;
            eventManager.PlayerStateChangedWithArrow -= OnPlayerStateChangedWithArrow;
            eventManager.PlayerHit -= OnPlayerHit;
            eventManager.PlayerSpawned -= OnPlayerSpawned;
            eventManager.NewClientPlayerCreated -= OnNewClientPlayerCreated;
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

                                im.SenderConnection.Approve(CreateHailMessage(im.SenderConnection));

                                break;

                            case NetConnectionStatus.Connected:

                                if (!IsServer)
                                {
                                    HailMessage message = new HailMessage(im.SenderConnection.RemoteHailMessage);

                                    if (clientCounter == 1)
                                        LocalPlayerId = message.PlayerId;

                                    clientCounter++;

                                    OnClientConnected(message);
                                    
                                    Console.WriteLine("Connected to {0}", im.SenderEndpoint);
                                }
                                else
                                {
                                    Console.WriteLine("{0} Connected", im.SenderEndpoint);
                                }

                                break;

                            case NetConnectionStatus.Disconnected:

                                if (!IsServer)
                                {
                                    OnDisconnected();

                                    Console.WriteLine("Disconnected from {0}", im.SenderEndpoint);
                                }
                                else
                                {
                                    short id = networkInterface.GetPlayerIdFromConnection(im.SenderConnection);

                                    OnClientDisconnected(id);

                                    SendClientDisconnectedMessage(id);

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
                            {
                                // TODO: DO SHIT HERE

                                break;
                            }
                            case GameMessageType.UpdatePlayerMovementState:
                            {
                                UpdatePlayerMovementStateMessage updatePlayerMovementStateMessage = new UpdatePlayerMovementStateMessage(im);
                                double localTime = im.SenderConnection.GetLocalTime(updatePlayerMovementStateMessage.MessageTime);

                                OnPlayerMovementStateUpdated(updatePlayerMovementStateMessage, localTime);

                                // If server, resend UpdatePlayerState to all clients
                                // TODO: Refactor this shit so that a client doesn't receive it's own message back
                                // This fucking shit ta causando um overhead chato na rede e acho que tem como consertar usando SendMessage ao inves de SendToAll no serverNetworkInterface porém como a porra do código é fechado temos que fazer testes manuais pra saber se a gente vai estar ganhando desempenho ou perdendo.
                                if (IsServer)
                                    networkInterface.SendMessage(updatePlayerMovementStateMessage);

                                break;
                            }

                            case GameMessageType.UpdatePlayerStateWithArrow:
                            {
                                UpdatePlayerStateWithArrowMessage updatePlayerStateWithArrowMessage = new UpdatePlayerStateWithArrowMessage(im);
                                double localTime = im.SenderConnection.GetLocalTime(updatePlayerStateWithArrowMessage.MessageTime);

                                OnPlayerStateUpdatedWithArrow(updatePlayerStateWithArrowMessage, localTime);

                                if (IsServer)
                                    networkInterface.SendMessage(updatePlayerStateWithArrowMessage);

                                break;
                            }

                            case GameMessageType.PlayerHit:
                            {
                                PlayerHitMessage playerHitMessage = new PlayerHitMessage(im);

                                OnPlayerHitUpdated(playerHitMessage);

                                if (IsServer)
                                    networkInterface.SendMessage(playerHitMessage);

                                break;
                            }

                            case GameMessageType.PlayerCreated:
                            {
                                PlayerCreatedMessage playerCreatedMessage = new PlayerCreatedMessage(im);

                                OnPlayerCreatedUpdated(playerCreatedMessage);

                                if (IsServer)
                                    networkInterface.SendMessage(playerCreatedMessage);

                                break;
                            }

                            case GameMessageType.PlayerSpawned:
                            {
                                PlayerSpawnedMessage playerSpawnedMessage = new PlayerSpawnedMessage(im);

                                OnPlayerSpawnedUpdated(playerSpawnedMessage);

                                if (IsServer)
                                    networkInterface.SendMessage(playerSpawnedMessage);

                                break;
                            }

                            case GameMessageType.NewClientPlayerCreated:
                            {
                                NewClientPlayerCreatedMessage newClientPlayerCreatedMessage = new NewClientPlayerCreatedMessage(im);

                                OnNewClientPlayerCreatedUpdated(newClientPlayerCreatedMessage);

                                break;
                            }

                            case GameMessageType.ClientDisconnected:
                            {
                                ClientDisconnectedMessage clientDisconnectedMessage = new ClientDisconnectedMessage(im);

                                OnClientDisconnected(clientDisconnectedMessage.PlayerId);

                                break;
                            }
                        }

                        break;
                }

                networkInterface.Recycle(im);
            }
        }

        // Eventos

        // Outgoing

        private void OnPlayerStateUpdated(UpdatePlayerMovementStateMessage message, double localTime)
        {
            eventManager.ThrowPlayerStateUpdated(this, new PlayerStateUpdatedEventArgs(message.PlayerId,
                                                                                       message.Position,
                                                                                       message.PlayerState,
                                                                                       message.StateType,
                                                                                       message.MessageTime,
                                                                                       localTime));
        }

        private void OnPlayerMovementStateUpdated(UpdatePlayerMovementStateMessage message, double localTime)
        {
            eventManager.ThrowPlayerMovementStateUpdated(this, new PlayerMovementStateUpdatedEventArgs(message.PlayerId,
                                                                                                       message.Position,
                                                                                                       message.Speed,
                                                                                                       message.PlayerState,
                                                                                                       message.StateType,
                                                                                                       message.MessageTime,
                                                                                                       localTime));
        }

        private void OnPlayerStateUpdatedWithArrow(UpdatePlayerStateWithArrowMessage message, double localTime)
        {
            eventManager.ThrowPlayerStateUpdatedWithArrow(this, new PlayerStateUpdatedWithArrowEventArgs(message.PlayerId,
                                                                                                         message.Position,
                                                                                                         message.ShotSpeed,
                                                                                                         message.PlayerState,
                                                                                                         message.StateType,
                                                                                                         message.MessageTime,
                                                                                                         localTime));
        }

        private void OnClientConnected(HailMessage msg)
        {
            eventManager.ThrowClientConnected(this, new ClientConnectedEventArgs(msg));
        }

        private void OnDisconnected()
        {
            eventManager.ThrowDisconnected(this, null);
        }

        private void OnClientDisconnected(short id)
        {
            eventManager.ThrowClientDisconnected(this, new PlayerIdEventArgs(id));
        }

        private void OnPlayerHitUpdated(PlayerHitMessage msg)
        {
            eventManager.ThrowPlayerHitUpdated(this, new PlayerHitEventArgs(msg));
        }

        private void OnPlayerSpawnedUpdated(PlayerSpawnedMessage msg)
        {
            eventManager.ThrowPlayerSpawnedUpdated(this, new PlayerSpawnedEventArgs(msg.PlayerId, msg.SpawnPosition));
        }

        private void OnPlayerCreatedUpdated(PlayerCreatedMessage msg)
        {
            eventManager.ThrowPlayerCreatedUpdated(this, new PlayerSpawnedEventArgs(msg.PlayerId, msg.SpawnPosition));
        }

        private void OnNewClientPlayerCreatedUpdated(NewClientPlayerCreatedMessage msg)
        {
            // TODO: Refactorar o EventArgs desse cara!
            eventManager.ThrowNewClientPlayerCreatedUpdated(this, new NewClientPlayerCreatedEventArgs(0, msg.PlayerPositions));
        }

        // Incoming

        private void OnPlayerStateChanged(object sender, PlayerStateChangedEventArgs args)
        {
            SendPlayerStateChangedMessage(args.PlayerId, args.Position, args.PlayerState, args.StateType);
        }

        private void OnPlayerMovementStateChanged(object sender, PlayerMovementStateChangedEventArgs args)
        {
            SendPlayerMovementStateChangedMessage(args.PlayerId, args.Position, args.Speed, args.PlayerState, args.StateType);
        }

        public void OnPlayerStateChangedWithArrow(object sender, PlayerStateChangedWithArrowEventArgs args)
        {
            SendPlayerStateChangedWithArrowMessage(args.PlayerId, args.Position, args.ShotSpeed, args.PlayerState, args.StateType);
        }

        private void OnPlayerHit(object sender, PlayerHitEventArgs args)
        {
            SendPlayerHitMessage(args.PlayerId, args.AttackerId);
        }

        private void OnPlayerSpawned(object sender, PlayerSpawnedEventArgs args)
        {
            SendPlayerSpawnedMessage(args.PlayerId, args.SpawnPoint);
        }

        private void OnPlayerCreated(object sender, PlayerSpawnedEventArgs args)
        {
            SendPlayerCreatedMessage(args.PlayerId, args.SpawnPoint);
        }

        private void OnNewClientPlayerCreated(object sender, NewClientPlayerCreatedEventArgs args)
        {
            foreach (KeyValuePair<short, Vector2> kv in args.PlayerPositions)
                Console.WriteLine("### " + kv.Key);

            SendNewClientPlayerCreatedMessage(args.PlayerId, args.PlayerPositions);
        }

        // Util

        private NetOutgoingMessage CreateHailMessage(NetConnection senderConnection)
        {
            networkInterface.RegisterClient(clientCounter, senderConnection);

            NetOutgoingMessage hailMessage = networkInterface.CreateMessage();
            new HailMessage(clientCounter++, clients).Encode(hailMessage);

            return hailMessage;
        }

        // Outgoing Messages

        private void SendPlayerStateChangedMessage(short id, Vector2 position, short playerState, UpdatePlayerStateType messageType)
        {
            networkInterface.SendMessage(new UpdatePlayerStateMessage(id, position, playerState, messageType));
        }

        private void SendPlayerStateChangedWithArrowMessage(short id, Vector2 position, Vector2 shotSpeed, short playerState, UpdatePlayerStateType messageType)
        {
            networkInterface.SendMessage(new UpdatePlayerStateWithArrowMessage(id, position, shotSpeed, playerState, messageType));
        }

        private void SendPlayerMovementStateChangedMessage(short id, Vector2 position, Vector2 speed, short playerState, UpdatePlayerStateType stateType)
        {
            networkInterface.SendMessage(new UpdatePlayerMovementStateMessage(id, position, speed, playerState, stateType));
        }

        private void SendPlayerHitMessage(short id, short attackerId)
        {
            networkInterface.SendMessage(new PlayerHitMessage(id, attackerId));
        }

        private void SendPlayerSpawnedMessage(short id, Vector2 spawnPosition)
        {
            networkInterface.SendMessage(new PlayerSpawnedMessage(id, spawnPosition));
        }

        private void SendPlayerCreatedMessage(short id, Vector2 spawnPosition)
        {
            networkInterface.SendMessage(new PlayerCreatedMessage(id, spawnPosition));
        }

        private void SendNewClientPlayerCreatedMessage(short playerId, Dictionary<short, Vector2> playerPositions)
        {
            networkInterface.SendMessageToClient(playerId, new NewClientPlayerCreatedMessage(playerPositions));
        }

        private void SendClientDisconnectedMessage(short playerId)
        {
            networkInterface.SendMessage(new ClientDisconnectedMessage(playerId));
        }
    }
}
