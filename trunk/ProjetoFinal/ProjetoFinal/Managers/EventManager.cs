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
        private NetworkManager networkManager = NetworkManager.Instance;

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                    //instance.networkManager = NetworkManager.Instance;
                }

                return instance;
            }
        }

        // Eventos Diretos

        public void ThrowPlayerStateChanged(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            networkManager.SendPlayerStateChangedMessage(id, player, messageType);
        }

        // Eventos Indiretos
    }
}
