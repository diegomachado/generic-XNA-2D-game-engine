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
        public event EventHandler<PlayerHitEventArgs> PlayerHit; // When a player has locally been hit
        public event EventHandler<PlayerStateChangedEventArgs> PlayerStateChanged; // When a player state has locally changed
        public event EventHandler<PlayerMovementStateChangedEventArgs> PlayerMovementStateChanged; // Wheh a player movement state locally changed
        public event EventHandler<PlayerStateChangedWithArrowEventArgs> PlayerStateChangedWithArrow; // When a player state has locally changed and an arrow was shot
        public event EventHandler<PlayerStateUpdatedEventArgs> PlayerStateUpdated; // When a message with updated information about a player's state arrives
        public event EventHandler<PlayerMovementStateUpdatedEventArgs> PlayerMovementStateUpdated; // When a message with updated information about a player's movement state arrives
        public event EventHandler<PlayerStateUpdatedWithArrowEventArgs> PlayerStateUpdatedWithArrow; // When a message with updated information about a player's state arrives with an arrow shot
        public event EventHandler<ClientConnectedEventArgs> ClientConnected; // When a message saying a new client connected arrives
        public event EventHandler<EventArgs> ClientDisconnected; // When a message saying a client disconnected arrives
        public event EventHandler<PlayerHitEventArgs> PlayerHitUpdated; // When a message saying a player was hit arrives

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

        public void ThrowPlayerStateChanged(object sender, PlayerStateChangedEventArgs args)
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(sender, args);
        }

        public void ThrowPlayerMovementStateChanged(object sender, PlayerMovementStateChangedEventArgs args)
        {
            if (PlayerMovementStateChanged != null)
                PlayerMovementStateChanged(sender, args);
        }

        public void ThrowPlayerStateChangedWithArrow(object sender, PlayerStateChangedWithArrowEventArgs args)
        {
            if (PlayerStateChangedWithArrow != null)
                PlayerStateChangedWithArrow(sender, args);
        }

        public void ThrowPlayerStateUpdated(object sender, PlayerStateUpdatedEventArgs args)
        {
            if (PlayerStateUpdated != null)
                PlayerStateUpdated(sender, args);
        }

        public void ThrowPlayerMovementStateUpdated(object sender, PlayerMovementStateUpdatedEventArgs args)
        {
            if (PlayerMovementStateUpdated != null)
                PlayerMovementStateUpdated(sender, args);
        }

        public void ThrowPlayerHitUpdated(object sender, PlayerHitEventArgs args)
        {
            if (PlayerHitUpdated != null)
                PlayerHitUpdated(sender, args);
        }

        public void ThrowPlayerStateUpdatedWithArrow(object sender, PlayerStateUpdatedWithArrowEventArgs args)
        {
            if (PlayerStateUpdatedWithArrow != null)
                PlayerStateUpdatedWithArrow(sender, args);
        }

        public void ThrowClientConnected(object sender, ClientConnectedEventArgs args)
        {
            if (ClientConnected != null)
                ClientConnected(sender, args);
        }

        public void ThrowClientDisconnected(object sender, ClientConnectedEventArgs args)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(sender, args);
        }

        public void ThrowPlayerHit(object sender, PlayerHitEventArgs args)
        {
            if (PlayerHit != null)
                PlayerHit(sender, args);
        }
    }
}