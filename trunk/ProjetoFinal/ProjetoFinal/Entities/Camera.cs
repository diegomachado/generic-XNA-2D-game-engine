using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjetoFinal.Managers;

namespace ProjetoFinal.Entities
{
    class Camera
    {
        private static Camera instance;
        private MapManager mapManager = MapManager.Instance;
        private GraphicsManager graphicsManager = GraphicsManager.Instance;

        private Vector2 position;
        private float speed;

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();

                return instance;
            }
        }

        public Camera()
        {
            Position = new Vector2();
            speed = 4f;
        }

        public float Speed
        {
            get { return speed; }
            set { speed = MathHelper.Clamp(value, 0.5f, 50f); }
        }

        public Vector2 Position
        {
            get 
            { 
                return position; 
            }
            set
            {
                if (mapManager.IsCurrentMapLoaded)
                {
                    position.X = MathHelper.Clamp(value.X, 0, mapManager.GetMapSize().X - graphicsManager.ScreenSize.X);
                    position.Y = MathHelper.Clamp(value.Y, 0, mapManager.GetMapSize().Y - graphicsManager.ScreenSize.Y);    
                }                
            }
        }

        public Point PositionToPoint()
        {
            return new Point((int)position.X, (int)position.Y);
        }

        public void FollowLocalPlayer(Player player)
        {
            this.Position = player.Position + new Vector2(player.Skin.Width / 2, player.Skin.Height / 2)
                                          - new Vector2(graphicsManager.ScreenSize.X / 2, graphicsManager.ScreenSize.Y / 2);
        }
    }
}
