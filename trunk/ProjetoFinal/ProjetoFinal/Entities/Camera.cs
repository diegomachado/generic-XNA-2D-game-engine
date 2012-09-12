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
        private MapManager mapManager = MapManager.Instance;
        private GraphicsManager graphicsManager = GraphicsManager.Instance;

        private static Camera instance;
        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();

                return instance;
            }
        }

        private float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = MathHelper.Clamp(value, 0.5f, 50f); }
        }

        private Vector2 position;
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
                    position.X = MathHelper.Clamp(value.X, 0, mapManager.MapSize.X - graphicsManager.ScreenSize.X);
                    position.Y = MathHelper.Clamp(value.Y, 0, mapManager.MapSize.Y - graphicsManager.ScreenSize.Y);
                }
            }
        }
        public Point PositionToPoint { get { return new Point((int)position.X, (int)position.Y); } }

        public Camera()
        {
            Position = new Vector2();
            speed = 4f;
        }

        public void FollowLocalPlayer(Player player)
        {
            this.Position = player.Center - graphicsManager.Center;
        }

        public void FollowLocalPlayer(DynamicEntity entity)
        {
            this.Position = entity.position - graphicsManager.Center;
        }

        public Vector2 WorldToCamera(Vector2 position)
        {
            return position - this.Position;
        }

        public Vector2 CameraToWorld(Vector2 position)
        {
            return position + this.Position;
        }
    }
}
