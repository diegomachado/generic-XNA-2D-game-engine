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
    class IdleState : PlayerState
    {
        public IdleState(bool isLocal) : base(isLocal) {}

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            return this;
        }

        public override PlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingStraight);
            return localPlayerStates[PlayerStateType.JumpingStraight];
        }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingLeft);
            return localPlayerStates[PlayerStateType.WalkingLeft];
        }
        
        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.WalkingRight);
            return localPlayerStates[PlayerStateType.WalkingRight];
        }

        public override string ToString()
        {
            return "Idle";
        }
    }
}
