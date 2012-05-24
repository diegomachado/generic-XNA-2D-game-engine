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
    abstract class WalkingSidewaysState : LocalPlayerState
    {
        public override LocalPlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (!checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = false;

                OnPlayerStateChanged(playerId, localPlayer, getWalkingState());
                return localPlayerStates[getWalkingState()];
            }

            localPlayer.SpeedX *= localPlayer.Friction;

            if (clampHorizontalSpeed(localPlayer))
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerState.Idle);
                return localPlayerStates[PlayerState.Idle];
            }

            if (handleHorizontalCollision(localPlayer, collisionLayer))
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerState.Idle);
                return localPlayerStates[PlayerState.Idle];
            }
            else
            {
                return this;
            }
        }

        abstract protected PlayerState getWalkingState();
    }
}
