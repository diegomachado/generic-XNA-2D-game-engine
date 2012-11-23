using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.PlayerStateMachine.ActionStates
{
    class DefendingState : ActionState
    {
        public override ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        public override string ToString()
        {
            return "Action Defending";
        }
    }
}
