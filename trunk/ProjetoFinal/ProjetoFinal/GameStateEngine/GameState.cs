using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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
        GameStateState state = GameStateState.TransitionOn;
        
        protected GraphicsManager graphicsManager = GraphicsManager.Instance;
        protected TextureManager textureManager = TextureManager.Instance;
        protected EventManager eventManager = EventManager.Instance;
        protected NetworkManager networkManager = NetworkManager.Instance;

        // TODO: Refactor this mother fucker name bitch!
        public GameStatesManager gameStatesManager { protected get; set; }
        public GameStateState State
        {
            get { return state; }
            set { state = value; }
        }
        
        //public bool IsPopup
        //{
        //    get { return isPopup; }
        //}
        //bool isPopup;

        public virtual void LoadContent(ContentManager content) { }
        public virtual void Update(InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont) { }
        public virtual void UnloadContent() { }
    }
}