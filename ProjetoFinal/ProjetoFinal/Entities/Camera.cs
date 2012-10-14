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
        private LevelManager levelManager = LevelManager.Instance;
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
                if (levelManager.IsCurrentLevelLoaded)
                {
                    position.X = MathHelper.Clamp(value.X, 0, levelManager.LevelWidth - graphicsManager.screen.Width);
                    position.Y = MathHelper.Clamp(value.Y, 0, levelManager.LevelHeight - graphicsManager.screen.Height);
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
