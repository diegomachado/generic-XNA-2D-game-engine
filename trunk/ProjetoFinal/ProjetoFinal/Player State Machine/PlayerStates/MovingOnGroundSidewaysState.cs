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
    abstract class MovingOnGroundSidewaysState : PlayerState
    {
        public MovingOnGroundSidewaysState(bool isLocal) : base(isLocal) { }

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (!checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = false;

                return localPlayerStates[getJumpingState()];
            }

            localPlayer.SpeedX *= localPlayer.Friction;

            if (clampHorizontalSpeed(localPlayer))
                return localPlayerStates[PlayerStateType.Idle];

            if (handleHorizontalCollision(localPlayer, collisionLayer))
                return localPlayerStates[PlayerStateType.Idle];
            else
                return this;
        }

        abstract protected PlayerStateType getJumpingState();
    }
}
