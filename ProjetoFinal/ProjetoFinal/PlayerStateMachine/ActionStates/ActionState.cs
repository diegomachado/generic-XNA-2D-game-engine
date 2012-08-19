using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine;
using ProjetoFinal.EventHeaders;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum ActionStateType : short
    {
        Idle,
        Shooting,
        PreparingShot,
        Defending,
        Attacking
    }

    abstract class ActionState : PlayerState
    {
        public abstract ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionStateType, ActionState> playerStates);

        #region Public Messages

        public virtual ActionState PreparingShot(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        public virtual ActionState ShotReleased(short playerId, Player player, float shootingTimer, Vector2 aim, Dictionary<ActionStateType, ActionState> playerStates)
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

        protected void OnArrowShot(short playerId, Vector2 position, Vector2 speed)
        {
            EventManager.Instance.ThrowArrowShot(this, new ArrowShotEventArgs(playerId, position, speed));
        }

        #endregion
    }
}
