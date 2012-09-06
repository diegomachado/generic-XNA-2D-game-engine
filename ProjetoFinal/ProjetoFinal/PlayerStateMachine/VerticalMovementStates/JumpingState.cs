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
    class JumpingState : VerticalMovementState
    {
        Rectangle collisionBoxVerticalOffset;

        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            if (player.MapCollideY(-1))
                player.speed.Y = 0;

            if ((player.flags & Entity.Flags.Gravity) == Entity.Flags.Gravity) player.speed.Y += player.gravity;
            
            collisionBoxVerticalOffset = player.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            player.speed.Y = MathHelper.Clamp(player.speed.Y, player.minSpeed.Y, player.maxSpeed.Y);

            player.MoveYBy(player.speed.Y);

            if (player.OnGround())
            {
                player.speed.Y = 0;
                return playerStates[VerticalStateType.Idle];
            }
            
            return this;
        }

        public override string ToString()
        {
            return "Jumping";
        }
    }
}
