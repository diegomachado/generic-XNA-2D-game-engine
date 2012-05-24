using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class StoppingJumpingLeftState : MovingOnAirSideways
    {
        public StoppingJumpingLeftState(bool isLocal) : base(isLocal) { }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.StoppingJumpingLeft, PlayerStateMessage.MovedLeft);
            return localPlayerStates[PlayerStateType.JumpingLeft];
        }

        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.StoppingJumpingLeft, PlayerStateMessage.MovedRight);
            return localPlayerStates[PlayerStateType.JumpingRight];
        }

        protected override PlayerStateType getWalkingState()
        {
            return PlayerStateType.StoppingWalkingLeft;
        }

        public override string ToString()
        {
            return "StoppingJumpingLeft";
        }
    }
}
