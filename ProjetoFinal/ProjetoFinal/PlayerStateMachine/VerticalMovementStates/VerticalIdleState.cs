using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class VerticalIdleState : VerticalMovementState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            if (player.OnGround())
                return this;
            else
                return playerStates[VerticalStateType.Jumping];
        }

        public override VerticalMovementState Jumped(short playerId, Player player, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateMessageType.Vertical, (short)VerticalStateType.StartedJumping);
            return playerStates[VerticalStateType.StartedJumping];
        }

        public override string ToString()
        {
            return "Vertical Idle";
        }
    }
}
