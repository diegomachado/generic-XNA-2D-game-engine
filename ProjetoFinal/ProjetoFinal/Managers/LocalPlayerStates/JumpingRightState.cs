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
        public override LocalPlayerState MovingLeft(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return localPlayerStates[PlayerState.JumpingLeft];
        }

        public override LocalPlayerState MovingRight(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return this;
        }

        protected override LocalPlayerState getWalkingState(Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return localPlayerStates[PlayerState.WalkingRight];
        }

        public override string ToString()
        {
            return "JumpingRight";
        }
    }
}
