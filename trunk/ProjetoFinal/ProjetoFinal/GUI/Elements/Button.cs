using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.GUI.Elements
{
    class Button : GUIElement
    {
        String label;

        public Button(String label, Rectangle frame/* DELEGATES AND SHIT!*/)
            : base(frame)
        {
            this.label = label;
        }

        // TODO: Delegates and shit
    }
}
