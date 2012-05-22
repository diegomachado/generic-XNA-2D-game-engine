using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjetoFinal.Entities
{
    class Camera
    {
        private static Camera instance;

        Vector2 position;
        float speed;

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
                position.X = MathHelper.Clamp(value.X, 0, Game.MapWidthInPixels.X - Game.ScreenSize.X);
                position.Y = MathHelper.Clamp(value.Y, 0, Game.MapWidthInPixels.Y - Game.ScreenSize.Y);
            }
        }

        public Point PositionToPoint()
        {
            return new Point((int)position.X, (int)position.Y);
        }
    }
}
