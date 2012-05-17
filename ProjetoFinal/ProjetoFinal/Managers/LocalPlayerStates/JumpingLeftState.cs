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
        public override LocalPlayerState MovingLeft(Player localPlayer)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return this;
        }

        public override LocalPlayerState MovingRight(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return new JumpingRightState();
        }

        protected override LocalPlayerState getWalkingState()
        {
            return new WalkingLeftState();
        }

        public override string ToString()
        {
            return "JumpingLeft";
        }
    }
}
