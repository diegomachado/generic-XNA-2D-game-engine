using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProjetoFinal.EventHeaders;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers
{
    class EventManager
    {
        private static EventManager instance;
        private NetworkManager networkManager;

        // Events
        public event EventHandler<PlayerStateUpdatedEventArgs> PlayerStateUpdated;
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<EventArgs> ClientDisconnected;

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                    instance.networkManager = NetworkManager.Instance;
                }

                return instance;
            }
        }

        // Eventos Diretos

        public void ThrowPlayerStateChanged(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            Console.WriteLine("SHIT");
            networkManager.SendPlayerStateChangedMessage(id, player, messageType);
        }

        // Eventos Indiretos

        public void ThrowPlayerStateUpdated(NetworkManager networkManager, PlayerStateUpdatedEventArgs playerStateUpdatedEventArgs)
        {
            if (PlayerStateUpdated != null)
                PlayerStateUpdated(networkManager, playerStateUpdatedEventArgs);
        }

        public void ThrowClientConnected(NetworkManager networkManager, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            if (ClientConnected != null)
                ClientConnected(networkManager, clientConnectedEventArgs);
        }

        public void ThrowClientDisconnected(NetworkManager networkManager, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(networkManager, clientConnectedEventArgs);
        }
    }
}