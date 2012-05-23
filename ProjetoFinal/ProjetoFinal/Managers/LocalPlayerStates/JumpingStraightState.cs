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
    class JumpingStraightState : JumpingState
    {
        public override LocalPlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.Speed = Vector2.Zero;

                OnPlayerStateChanged(playerId, localPlayer, PlayerState.Idle);
                return localPlayerStates[PlayerState.Idle];
            }
            
            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            if (handleVerticalCollision(localPlayer, collisionLayer) && localPlayer.Speed.Y > 0)
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerState.Idle);
                return localPlayerStates[PlayerState.Idle];
            }
            else
            {
                return this;
            }
        }

        public override LocalPlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingLeft);
            return localPlayerStates[PlayerState.JumpingLeft];
        }

        public override LocalPlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerState.JumpingRight);
            return localPlayerStates[PlayerState.JumpingRight];
        }

        public override string ToString()
        {
            return "JumpingStraight";
        }
    }
}
