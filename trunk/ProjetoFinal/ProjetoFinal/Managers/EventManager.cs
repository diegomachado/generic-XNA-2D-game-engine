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
        public event EventHandler<PlayerStateChangedEventArgs> PlayerStateChanged; // When a player state has locally changed
        public event EventHandler<PlayerStateUpdatedEventArgs> PlayerStateUpdated; // When a message with updated information about a player arrives
        public event EventHandler<ClientConnectedEventArgs> ClientConnected; // When a message saying a new client connected arrives
        public event EventHandler<EventArgs> ClientDisconnected; // When a message saying a client disconnected arrives
        public event EventHandler<ArrowShotEventArgs> ArrowShot; // When an arrow is locally created
        // TODO: // When a message saying a new arrow was created arrives

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

        public void ThrowPlayerStateChanged(object sender, PlayerStateChangedEventArgs playerStateChangedEventArgs)
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(sender, playerStateChangedEventArgs);
        }

        public void ThrowPlayerStateUpdated(object sender, PlayerStateUpdatedEventArgs playerStateUpdatedEventArgs)
        {
            if (PlayerStateUpdated != null)
                PlayerStateUpdated(sender, playerStateUpdatedEventArgs);
        }

        public void ThrowClientConnected(object sender, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            if (ClientConnected != null)
                ClientConnected(sender, clientConnectedEventArgs);
        }

        public void ThrowClientDisconnected(object sender, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(sender, clientConnectedEventArgs);
        }

        public void ThrowArrowShot(object sender, ArrowShotEventArgs arrowShotEventArgs)
        {
            if (ArrowShot != null)
            {
                ArrowShot(sender, arrowShotEventArgs);
            }
        }
    }
}