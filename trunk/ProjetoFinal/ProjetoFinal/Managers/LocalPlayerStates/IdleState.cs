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
    class IdleState : LocalPlayerState
    {
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer)
        {
            return this;
        }

        public override LocalPlayerState Jumped(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            return new JumpingStraightState();
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

            return new WalkingRightState();
        }

        public override string ToString()
        {
            return "Idle";
        }
    }
}
