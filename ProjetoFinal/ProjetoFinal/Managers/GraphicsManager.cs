using System;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.Managers
{
    class GraphicsManager
    {
        private static GraphicsManager instance;
        public Rectangle screen = new Rectangle(0, 0, 800, 608);

        public static GraphicsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GraphicsManager();

                return instance;
            }
        }

        public Vector2 Center { get { return new Vector2(screen.Center.X, screen.Center.Y); } }
    }
}
