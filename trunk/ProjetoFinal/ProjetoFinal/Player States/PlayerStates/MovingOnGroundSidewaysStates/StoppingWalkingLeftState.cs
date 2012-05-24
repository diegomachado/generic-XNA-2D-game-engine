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
    class StoppingWalkingLeftState : MovingOnGroundSidewaysState
    {
        public StoppingWalkingLeftState(bool isLocal) : base(isLocal) { }

        public override PlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.StoppingJumpingLeft);
            return localPlayerStates[PlayerStateType.StoppingJumpingLeft];
        }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingLeft);
            return localPlayerStates[PlayerStateType.WalkingLeft];
        }

        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingRight);
            return localPlayerStates[PlayerStateType.WalkingRight];
        }

        protected override PlayerStateType getJumpingState()
        {
            return PlayerStateType.StoppingJumpingLeft;
        }

        public override string ToString()
        {
            return "StoppingWalkingLeft";
        }
    }
}
