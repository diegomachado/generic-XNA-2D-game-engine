using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine;

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

    abstract class HorizontalMovementState : PlayerState
    {
        public abstract HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates);

        public virtual HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }
        public virtual HorizontalMovementState StoppedMovingLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }
        public virtual HorizontalMovementState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }
        public virtual HorizontalMovementState StoppedMovingRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }
    }
}
