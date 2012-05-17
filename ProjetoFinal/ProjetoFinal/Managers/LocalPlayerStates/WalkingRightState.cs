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
        public override LocalPlayerState Jumped(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            return new JumpingRightState();
        }

        public override LocalPlayerState MovingLeft(Player localPlayer)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return new WalkingLeftState();
        }

        public override LocalPlayerState MovingRight(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return this;
        }

        protected override LocalPlayerState getWalkingState()
        {
            return new JumpingRightState();
        }

        public override string ToString()
        {
            return "WalkingRight";
        }
    }
}
