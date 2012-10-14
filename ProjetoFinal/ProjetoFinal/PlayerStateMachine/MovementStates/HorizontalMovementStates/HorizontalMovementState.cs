﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;
using ProjetoFinal.PlayerStateMachine;

using OgmoEditorLibrary;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum HorizontalStateType : short
    {
        Idle,
        WalkingLeft,
        WalkingRight,
        StoppingWalkingLeft,
        StoppingWalkingRight
    }

    abstract class HorizontalMovementState : MovementPlayerState
    {
        public abstract HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates);

        #region Public Messages

        public virtual HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            if(player.VerticalState != VerticalStateType.Jumping) player.spriteMap.Play("moving");
            return this;
        }
        public virtual HorizontalMovementState StoppedMovingLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            player.spriteMap.Play("idle");
            return this;
        }
        public virtual HorizontalMovementState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            if (player.VerticalState != VerticalStateType.Jumping) player.spriteMap.Play("moving");
            return this;
        }
        public virtual HorizontalMovementState StoppedMovingRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            player.spriteMap.Play("idle");
            return this;
        }

        #endregion
    }
}
