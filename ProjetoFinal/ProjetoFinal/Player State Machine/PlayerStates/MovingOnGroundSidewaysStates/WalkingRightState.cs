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
    class WalkingRightState : MovingOnGroundSidewaysState
    {
        public WalkingRightState(bool isLocal) : base(isLocal) {}

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;

            return base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);
        }

        public override PlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingRight, PlayerStateMessage.Jumped);
            return localPlayerStates[PlayerStateType.JumpingRight];
        }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingRight, PlayerStateMessage.MovedLeft);
            return localPlayerStates[PlayerStateType.WalkingLeft];
        }

        public override PlayerState StoppedMovingRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingRight, PlayerStateMessage.StoppedMovingRight);
            return localPlayerStates[PlayerStateType.StoppingWalkingRight];
        }

        protected override PlayerStateType getJumpingState()
        {
            return PlayerStateType.JumpingRight;
        }

        public override string ToString()
        {
            return "WalkingRight";
        }
    }
}
