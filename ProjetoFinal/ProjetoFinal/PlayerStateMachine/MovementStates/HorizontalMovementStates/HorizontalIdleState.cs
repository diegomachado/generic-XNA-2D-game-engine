using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class HorizontalIdleState : HorizontalMovementState
    {
        public override HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            player.spriteMap.Play("idle");
            return this;
        }

        public override HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Horizontal, (short)HorizontalStateType.WalkingLeft);
            return playerStates[HorizontalStateType.WalkingLeft];
        }

        public override HorizontalMovementState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Horizontal, (short)HorizontalStateType.WalkingRight);
            return playerStates[HorizontalStateType.WalkingRight];
        }

        public override string ToString()
        {
            return "Horizontal Idle";
        }
    }
}
