using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.Managers
{
    class InputManager
    {
        private static InputManager instance;

        private KeyboardState previousKeyboardState, keyboardState;
        private MouseState previousMouseState, mouseState;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();

                return instance;
            }
        }

        public void Update()
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        #region Keyboard Input

        public bool Jump
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Space) ||
                       keyboardState.IsKeyDown(Keys.Up) ||
                       keyboardState.IsKeyDown(Keys.W);
            }
        }
        public bool Left 
        { 
            get 
            { 
                return keyboardState.IsKeyDown(Keys.Left) ||
                       keyboardState.IsKeyDown(Keys.A); 
            } 
        }
        public bool PreviouslyLeft 
        { 
            get 
            { 
                return previousKeyboardState.IsKeyDown(Keys.Left) ||
                       previousKeyboardState.IsKeyDown(Keys.A);
            } 
        }
        public bool Right 
        { 
            get 
            {
                return keyboardState.IsKeyDown(Keys.Right) ||
                       keyboardState.IsKeyDown(Keys.D); 
            } 
        }
        public bool PreviouslyRight 
        { 
            get 
            { 
                return previousKeyboardState.IsKeyDown(Keys.Right) ||
                       previousKeyboardState.IsKeyDown(Keys.D);
            } 
        }

        public bool Exit { get { return keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape); } }
        public bool Pause { get { return keyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P); } }
        public bool BackSpace { get { return keyboardState.IsKeyDown(Keys.Back) && previousKeyboardState.IsKeyUp(Keys.Back); } }

        #endregion

        public String TextInput
        {
            get
            {
                /*
                String output = "";

                foreach (Keys key in keyboardState.GetPressedKeys())
                    output += key.ToString() + ", ";

                Console.WriteLine(output);
                */

                String buffer = "";
                
                foreach (Keys key in keyboardState.GetPressedKeys())
                {
                    if (key == Keys.D0 ||
                        key == Keys.D1 ||
                        key == Keys.D2 ||
                        key == Keys.D3 ||
                        key == Keys.D4 ||
                        key == Keys.D5 ||
                        key == Keys.D6 ||
                        key == Keys.D7 ||
                        key == Keys.D8 ||
                        key == Keys.D9 ||
                        key == Keys.OemPeriod
                       )
                    {
                        if (previousKeyboardState.IsKeyUp(key))
                        {
                            if (key == Keys.OemPeriod)
                                buffer += ".";
                            else
                                buffer += key.ToString().Substring(1, 1);
                        }
                    }
                }

                return buffer;
            }
        }

        #region Debugger

        public bool ToggleFPS { get { return keyboardState.IsKeyDown(Keys.D1) && previousKeyboardState.IsKeyUp(Keys.D1); } }
        public bool ToggleRuler { get { return keyboardState.IsKeyDown(Keys.D2) && previousKeyboardState.IsKeyUp(Keys.D2); } }
        public bool ToggleRulerLog { get { return keyboardState.IsKeyDown(Keys.D3) && previousKeyboardState.IsKeyUp(Keys.D3); } }

        #endregion
        
        #region Mouse Input

        public Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public bool MouseLeftButton { get { return ((mouseState.LeftButton == ButtonState.Pressed) && (previousMouseState.LeftButton == ButtonState.Released)); } }
        public bool PreparingShot { get { return (mouseState.LeftButton == ButtonState.Pressed); } }

        #endregion
    }
}