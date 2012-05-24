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
    class StoppingJumpingRightState : MovingOnAirSideways
    {
        public StoppingJumpingRightState(bool isLocal) : base(isLocal) { }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingLeft);
            return localPlayerStates[PlayerStateType.JumpingLeft];
        }

        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingRight);
            return localPlayerStates[PlayerStateType.JumpingRight];
        }

        protected override PlayerStateType getWalkingState()
        {
            return PlayerStateType.StoppingWalkingRight;
        }

        public override string ToString()
        {
            return "StoppingJumpingRight";
        }
    }
}
