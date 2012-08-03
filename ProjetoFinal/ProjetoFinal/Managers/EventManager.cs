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
            networkManager.SendPlayerStateChangedMessage(id, player, messageType);
        }

        public void ThrowOtherClientPlayerStateChanged(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            if (networkManager.IsServer)
                ThrowPlayerStateChanged(id, player, messageType);
        }

        // Eventos Indiretos

        public void ThrowPlayerStateUpdated(NetworkManager networkManager, PlayerStateUpdatedEventArgs playerStateUpdatedEventArgs)
        {
            // TODO: Questionar se tenho que mandar o sender e quem será?
            if (PlayerStateUpdated != null)
                PlayerStateUpdated(networkManager, playerStateUpdatedEventArgs);
        }

        internal void throwClientConnected(NetworkManager networkManager, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            if (ClientConnected != null)
                ClientConnected(networkManager, clientConnectedEventArgs);
        }
    }
}
