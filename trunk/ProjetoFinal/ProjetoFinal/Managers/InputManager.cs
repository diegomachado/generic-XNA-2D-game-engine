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
        
        private Keys[] alphaKeys = { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, 
                                     Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, 
                                     Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, 
                                     Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z };

        private Keys[] numericKeys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4,
                                       Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
                                       Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, 
                                       Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, 
                                       Keys.OemPeriod };

        private Keys[] symbolKeys = { Keys.OemBackslash, Keys.OemOpenBrackets, Keys.OemCloseBrackets,
                                      Keys.OemMinus, Keys.OemPeriod, Keys.OemPipe, Keys.OemPlus, Keys.Space, 
                                      Keys.OemQuestion, Keys.OemQuotes, Keys.OemSemicolon, Keys.OemTilde };

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

        #region Game Keyboard Input

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

        public String GetInput(bool numericOnly = false)
        {
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                if(alphaKeys.Contains(key) && !numericOnly)
                    return GetAlphaInput(key);
                if(symbolKeys.Contains(key) && !numericOnly)
                    return GetSymbolInput(key);
                if (numericKeys.Contains(key))
                    return GetNumericInput(key);
            }
            return "";
        }
        
        private String GetAlphaInput(Keys key)
        {
            if (previousKeyboardState.IsKeyUp(key))
                if (keyboardState.GetPressedKeys().Contains(Keys.LeftShift) || keyboardState.GetPressedKeys().Contains(Keys.RightShift))
                    return key.ToString();
                else
                    return key.ToString().ToLower();
            return "";
        }

        private String GetNumericInput(Keys key)
        {
            if (previousKeyboardState.IsKeyUp(key))
                if (key == Keys.OemPeriod)
                    return ".";
                else
                    return key.ToString().Substring(key.ToString().Length - 1, 1);
            return "";
        }
        
        private String GetSymbolInput(Keys key)
        {
            if(previousKeyboardState.IsKeyUp(key))
            {
                if (key == Keys.Space) return " ";
                if (key == Keys.OemBackslash) return "/";
                if (key == Keys.OemOpenBrackets) return "[";
                if (key == Keys.OemCloseBrackets) return "]";
                if (key == Keys.OemMinus) return "-";
                if (key == Keys.OemPlus) return "+";
                if (key == Keys.OemPeriod) return ".";
                if (key == Keys.OemPipe) return "|";
                if (key == Keys.OemQuestion) return "?";
                if (key == Keys.OemQuotes) return "\"";
                if (key == Keys.OemSemicolon) return ";";
                if (key == Keys.OemTilde) return "~";
            }
            return "";
        }

        #region Debugger

        public bool ToggleFPS { get { return keyboardState.IsKeyDown(Keys.D1) && previousKeyboardState.IsKeyUp(Keys.D1); } }
        public bool ToggleRuler { get { return keyboardState.IsKeyDown(Keys.D2) && previousKeyboardState.IsKeyUp(Keys.D2); } }
        public bool ToggleRulerLog { get { return keyboardState.IsKeyDown(Keys.D3) && previousKeyboardState.IsKeyUp(Keys.D3); } }

        #endregion
        
        #region Mouse Input

        public Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public bool MouseLeftButton { get { return ((mouseState.LeftButton == ButtonState.Pressed) && (previousMouseState.LeftButton == ButtonState.Released)); } }
        public bool PreparingShot { get { return ((mouseState.LeftButton == ButtonState.Pressed) && (previousMouseState.LeftButton == ButtonState.Pressed)); } }

        #endregion
    }
}