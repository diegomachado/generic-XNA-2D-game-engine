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
        public JumpingStraightState(bool isLocal) : base(isLocal) { }

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.Speed = Vector2.Zero;

                OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.Idle);
                return localPlayerStates[PlayerStateType.Idle];
            }
            
            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            if (handleVerticalCollision(localPlayer, collisionLayer) && localPlayer.Speed.Y > 0)
            {
                OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.Idle);
                return localPlayerStates[PlayerStateType.Idle];
            }
            else
            {
                return this;
            }
        }

        public override PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingLeft);
            return localPlayerStates[PlayerStateType.JumpingLeft];
        }

        public override PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingRight);
            return localPlayerStates[PlayerStateType.JumpingRight];
        }

        public override string ToString()
        {
            return "JumpingStraight";
        }
    }
}
