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
    class JumpingState : LocalPlayerState
    {
        public override LocalPlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.Gravity;

            return this;
        }

        public override LocalPlayerState MovingLeft(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return this;
        }

        public override LocalPlayerState MovingRight(short playerId, Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return this;
        }
    }
}
