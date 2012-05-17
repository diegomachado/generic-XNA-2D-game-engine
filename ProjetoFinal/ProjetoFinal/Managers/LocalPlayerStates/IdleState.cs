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
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return this;
        }

        public override LocalPlayerState Jumped(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            return localPlayerStates[PlayerState.JumpingStraight];
        }

        public override LocalPlayerState MovingLeft(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return localPlayerStates[PlayerState.WalkingLeft];
        }

        public override LocalPlayerState MovingRight(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return localPlayerStates[PlayerState.WalkingRight];
        }

        public override string ToString()
        {
            return "Idle";
        }
    }
}
