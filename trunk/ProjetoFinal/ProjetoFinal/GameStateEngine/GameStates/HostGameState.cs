using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class HostGameState : GameState
    {
        public override void Update(Managers.InputManager inputManager, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (inputManager.Exit)
                GameStateManager.ExitGame();
        }
    }
}
