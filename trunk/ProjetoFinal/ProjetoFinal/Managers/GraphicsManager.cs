using System;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.Managers
{
    class GraphicsManager
    {
        private static GraphicsManager instance;
        private Point screenSize = new Point(800, 608);

        public Vector2 Center { get { return new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2); } } // TODO: Guardar esse valor pra não calcular sempre
        public Point ScreenSize { get { return screenSize; } }

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
