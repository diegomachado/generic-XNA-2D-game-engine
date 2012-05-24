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
    class WalkingLeftState : WalkingSidewaysState
    {
        public override LocalPlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingLeft);
            return localPlayerStates[PlayerState.JumpingLeft];
        }

        public override LocalPlayerState MovingLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return this;
        }

        public override LocalPlayerState MovingRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.WalkingRight);
            return localPlayerStates[PlayerState.WalkingRight];
        }

        protected override PlayerState getWalkingState()
        {
            return PlayerState.JumpingLeft;
        }

        public override string ToString()
        {
            return "WalkingLeft";
        }
    }
}
