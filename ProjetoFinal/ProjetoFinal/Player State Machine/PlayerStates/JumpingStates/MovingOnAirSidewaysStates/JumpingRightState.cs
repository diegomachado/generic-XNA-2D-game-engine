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
    class JumpingRightState : MovingOnAirSideways
    {
        public JumpingRightState(bool isLocal) : base(isLocal) { }

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;

            return base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);
        }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingRight, PlayerStateMessage.MovedLeft);
            return localPlayerStates[PlayerStateType.JumpingLeft];
        }

        public override PlayerState StoppedMovingRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingRight, PlayerStateMessage.StoppedMovingRight);
            return localPlayerStates[PlayerStateType.StoppingJumpingRight];
        }

        protected override PlayerStateType getWalkingState()
        {
            return PlayerStateType.WalkingRight;
        }

        public override string ToString()
        {
            return "JumpingRight";
        }
    }
}
