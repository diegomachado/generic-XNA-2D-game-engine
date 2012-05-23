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
    class JumpingRightState : JumpingSideways
    {
        public override LocalPlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingLeft);
            return localPlayerStates[PlayerState.JumpingLeft];
        }

        public override LocalPlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return this;
        }

        public override LocalPlayerState StoppedMovingRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerState.StoppingJumpingRight);
            return localPlayerStates[PlayerState.StoppingJumpingRight];
        }

        protected override PlayerState getWalkingState()
        {
            return PlayerState.WalkingRight;
        }

        public override string ToString()
        {
            return "JumpingRight";
        }
    }
}
