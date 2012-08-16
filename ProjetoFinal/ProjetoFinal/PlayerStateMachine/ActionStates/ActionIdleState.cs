using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class ActionIdleState : ActionState
    {
        public override ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        public override ActionState Shot(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return playerStates[ActionStateType.Shooting];
        }

        public override ActionState Defended(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return playerStates[ActionStateType.Defending];
        }

        public override ActionState Attacked(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return playerStates[ActionStateType.Attacking];
        }

        public override string ToString()
        {
            return "Action Idle";
        }
    }
}
