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
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (!checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = false;

                return getWalkingState(localPlayerStates);
            }

            localPlayer.SpeedX *= localPlayer.Friction;

            if (clampHorizontalSpeed(localPlayer))
                return localPlayerStates[PlayerState.Idle];            

            if (handleHorizontalCollision(localPlayer, collisionLayer))
                return localPlayerStates[PlayerState.Idle];
            else
                return this;
        }

        abstract protected LocalPlayerState getWalkingState(Dictionary<PlayerState, LocalPlayerState> localPlayerStates);
    }
}
