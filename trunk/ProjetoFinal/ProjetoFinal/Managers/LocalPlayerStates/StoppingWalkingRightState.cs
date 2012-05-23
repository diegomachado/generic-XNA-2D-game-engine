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
    class StoppingWalkingRightState : WalkingRightState
    {
        public override string ToString()
        {
            return "StoppingWalkingRight";
        }
    }
}
