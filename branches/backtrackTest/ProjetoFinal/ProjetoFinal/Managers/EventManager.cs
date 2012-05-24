using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoFinal.Managers
{
    class EventManager
    {
        private static EventManager instance;

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventManager();

                return instance;
            }
        }
    }
}
