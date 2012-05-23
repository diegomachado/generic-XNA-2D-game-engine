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
    class JumpingLeftState : JumpingSideways
    {
        public override LocalPlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return this;
        }

        public override LocalPlayerState StoppedMovingLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            //OnPlayerStateChanged(playerId, localPlayer, PlayerState.WalkingLeft);
            return localPlayerStates[PlayerState.StoppingJumpingLeft];
        }

        public override LocalPlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingRight);
            return localPlayerStates[PlayerState.JumpingRight];
        }

        protected override PlayerState getWalkingState()
        {
            return PlayerState.WalkingLeft;
        }

        public override string ToString()
        {
            return "JumpingLeft";
        }
    }
}
