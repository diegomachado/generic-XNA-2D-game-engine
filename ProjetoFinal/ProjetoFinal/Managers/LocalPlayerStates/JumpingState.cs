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
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            localPlayer.Speed += localPlayer.Gravity;

            return this;
        }

        public override LocalPlayerState MovingLeft(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return this;
        }

        public override LocalPlayerState MovingRight(Player localPlayer, Dictionary<PlayerState, LocalPlayerState> localPlayerStates)
        {
            return this;
        }
    }
}
