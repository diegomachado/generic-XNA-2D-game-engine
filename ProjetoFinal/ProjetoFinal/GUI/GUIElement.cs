using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.GUI
{
    abstract class GUIElement
    {
        Rectangle frame;

        public GUIElement(Rectangle frame)
        {
            this.frame = frame;
        }
    }
}
