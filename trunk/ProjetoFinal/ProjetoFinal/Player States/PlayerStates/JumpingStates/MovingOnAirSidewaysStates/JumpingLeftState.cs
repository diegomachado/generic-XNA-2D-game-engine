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
    class JumpingLeftState : StoppingJumpingLeftState
    {
        public JumpingLeftState(bool isLocal) : base(isLocal) { }

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;

            return base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);
        }

        public override PlayerState StoppedMovingLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.StoppingJumpingLeft);
            return localPlayerStates[PlayerStateType.StoppingJumpingLeft];
        }

        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingRight);
            return localPlayerStates[PlayerStateType.JumpingRight];
        }

        protected override PlayerStateType getWalkingState()
        {
            return PlayerStateType.WalkingLeft;
        }

        public override string ToString()
        {
            return "JumpingLeft";
        }
    }
}
