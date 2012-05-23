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
    class WalkingRightState : WalkingSidewaysState
    {
        public override LocalPlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingRight);
            return localPlayerStates[PlayerState.JumpingRight];
        }

        public override LocalPlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.WalkingLeft);
            return localPlayerStates[PlayerState.WalkingLeft];
        }

        public override LocalPlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return this;
        }

        public override LocalPlayerState StoppedMovingRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            //OnPlayerStateChanged(playerId, localPlayer, PlayerState.WalkingLeft);
            return localPlayerStates[PlayerState.StoppingWalkingRight];
        }

        protected override PlayerState getWalkingState()
        {
            return PlayerState.JumpingRight;
        }

        public override string ToString()
        {
            return "WalkingRight";
        }
    }
}
