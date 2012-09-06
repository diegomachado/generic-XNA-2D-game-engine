using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using OgmoLibrary;
using ProjetoFinal.Entities;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum VerticalStateType : short
    {
        Idle,
        Jumping,
        StartedJumping
    }

    abstract class VerticalMovementState : PlayerState
    {
        public abstract VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<VerticalStateType, VerticalMovementState> playerStates);

        public virtual VerticalMovementState Jumped(short playerId, Player player, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            return this;
        }
    }
}
