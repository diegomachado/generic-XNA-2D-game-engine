using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class VerticalIdleState : VerticalMovementState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            if (!player.OnGround())
                return playerStates[VerticalStateType.Jumping];
            else
                return this;
        }

        public override VerticalMovementState Jumped(short playerId, Player player, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Vertical, (short)VerticalStateType.StartedJumping);

            return playerStates[VerticalStateType.StartedJumping];
        }

        public override string ToString()
        {
            return "Vertical Idle";
        }
    }
}
