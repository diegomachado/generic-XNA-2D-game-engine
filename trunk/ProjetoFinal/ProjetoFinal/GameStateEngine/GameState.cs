using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers;

namespace ProjetoFinal.GameStateEngine
{
    public enum GameStateState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    abstract class GameState
    {
        public GameStateState State
        {
            get { return state; }
            protected set { state = value; }
        }

        public bool IsPopup
        {
            get { return isPopup; }
        }
        bool isPopup;

        GameStateState state = GameStateState.TransitionOn;

        public virtual void Update(GameStatesManager gameStateManager, InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/)
        {
            
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont) { }
    }
}
