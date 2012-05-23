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
    abstract class JumpingSideways : JumpingState
    {
        public override LocalPlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            localPlayer.SpeedX *= localPlayer.Friction;

            if (clampHorizontalSpeed(localPlayer))
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingStraight);
                return localPlayerStates[PlayerState.JumpingStraight];
            }

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.SpeedY = 0;

                OnPlayerStateChanged(playerId, localPlayer, getWalkingState());
                return localPlayerStates[getWalkingState()];
            }
            
            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            bool collidedHorizontally = handleHorizontalCollision(localPlayer, collisionLayer);
            bool collidedVertically = handleVerticalCollision(localPlayer, collisionLayer);

            if (collidedHorizontally && collidedVertically && localPlayer.Speed.Y > 0)
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerState.Idle);
                return localPlayerStates[PlayerState.Idle];
            }
            else if (collidedHorizontally)
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingStraight);
                return localPlayerStates[PlayerState.JumpingStraight];
            }
            else if (collidedVertically && localPlayer.Speed.Y > 0)
            {
                OnPlayerStateChanged(playerId, localPlayer, getWalkingState());
                return localPlayerStates[getWalkingState()];
            }
            else
            {
                return this;
            }
        }

        abstract protected PlayerState getWalkingState();
    }
}
