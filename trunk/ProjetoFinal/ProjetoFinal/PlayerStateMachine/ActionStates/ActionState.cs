using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum ActionStateType : short
    {
        Idle,
        Shooting,
        Defending,
        Attacking
    }

    abstract class ActionState : PlayerState
    {
        public abstract ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionStateType, ActionState> playerStates);

        #region Public Messages

        public virtual ActionState Shot(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        public virtual ActionState Defended(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        public virtual ActionState Attacked(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        #endregion

        #region Protected Methods

        #endregion
    }
}
