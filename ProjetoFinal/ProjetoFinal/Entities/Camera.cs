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

        public void Update(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.I))
                ScrollUp();
            if (keyboardState.IsKeyDown(Keys.K))
                ScrollDown();
            if (keyboardState.IsKeyDown(Keys.J))
                ScrollLeft();
            if (keyboardState.IsKeyDown(Keys.L))
                ScrollRight();
        }

        public void ScrollUp()
        {
            Position = Vector2.Add(Position, new Vector2(0, Speed));
        }

        public void ScrollDown()
        {
            Position = Vector2.Add(Position, new Vector2(0, Speed));
        }

        public void ScrollLeft()
        {
            Position = Vector2.Add(Position, new Vector2(-Speed, 0));
        }

        public void ScrollRight()
        {
            Position = Vector2.Add(Position, new Vector2(Speed, 0));
        }
    }
}
