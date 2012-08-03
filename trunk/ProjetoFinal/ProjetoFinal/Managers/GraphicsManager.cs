using System;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.Managers
{
    class GraphicsManager
    {
        private static GraphicsManager instance;
        private Point screenSize = new Point(800, 608);
        
        public Point ScreenSize 
        {
            get { return screenSize; }
        }

        public static GraphicsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GraphicsManager();
                }

                return instance;
            }
        }
    }
}
