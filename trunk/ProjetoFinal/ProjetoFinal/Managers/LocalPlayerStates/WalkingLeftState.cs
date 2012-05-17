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
        public override LocalPlayerState Jumped(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            return localPlayerStates[PlayerState.JumpingLeft];
        }

        public override LocalPlayerState MovingLeft(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return this;
        }

        public override LocalPlayerState MovingRight(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return localPlayerStates[PlayerState.WalkingRight];
        }

        protected override LocalPlayerState getWalkingState(Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return localPlayerStates[PlayerState.JumpingLeft];
        }

        public override string ToString()
        {
            return "WalkingLeft";
        }
    }
}
