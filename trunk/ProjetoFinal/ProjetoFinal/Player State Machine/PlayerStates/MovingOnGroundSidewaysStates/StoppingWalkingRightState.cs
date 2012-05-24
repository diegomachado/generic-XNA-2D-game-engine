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
    class StoppingWalkingRightState : MovingOnGroundSidewaysState
    {
        public StoppingWalkingRightState(bool isLocal) : base(isLocal) { }

        public override PlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.StoppingJumpingRight);
            return localPlayerStates[PlayerStateType.StoppingJumpingRight];
        }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingLeft);
            return localPlayerStates[PlayerStateType.WalkingLeft];
        }

        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingRight);
            return localPlayerStates[PlayerStateType.WalkingRight];
        }

        protected override PlayerStateType getJumpingState()
        {
            return PlayerStateType.StoppingJumpingRight;
        }

        public override string ToString()
        {
            return "StoppingWalkingRight";
        }
    }
}
