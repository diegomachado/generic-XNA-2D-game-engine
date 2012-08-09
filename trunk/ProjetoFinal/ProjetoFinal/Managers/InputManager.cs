using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ProjetoFinal.Managers
{
    class InputManager
    {
        private static InputManager instance;

        private KeyboardState previousKeyboardState, keyboardState;

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
        }

        public bool Exit { get { return keyboardState.IsKeyDown(Keys.Escape); } }
        public bool Jump { get { return keyboardState.IsKeyDown(Keys.Space); } }
        public bool Left { get { return keyboardState.IsKeyDown(Keys.Left); } }
        public bool PreviouslyLeft { get { return previousKeyboardState.IsKeyDown(Keys.Left); } }
        public bool Right { get { return keyboardState.IsKeyDown(Keys.Right); } }
        public bool PreviouslyRight { get { return previousKeyboardState.IsKeyDown(Keys.Right); } }
        public bool Pause { get { return keyboardState.IsKeyDown(Keys.P) && !previousKeyboardState.IsKeyDown(Keys.P); } }
    }
}