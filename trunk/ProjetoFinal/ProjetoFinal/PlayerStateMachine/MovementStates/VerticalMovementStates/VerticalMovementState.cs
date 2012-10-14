using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum VerticalStateType : short
    {
        Idle,
        Jumping,
        StartedJumping
    }

    abstract class VerticalMovementState : MovementPlayerState
    {
        public abstract VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<VerticalStateType, VerticalMovementState> playerStates);

        #region Public Messages

        public virtual VerticalMovementState Jumped(short playerId, Player player, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            return this;
        }

        #endregion

    }
}
