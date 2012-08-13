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

        // TODO: Refactor this mother fucker name bitch! É property, deveria começar maiusculo so que nao da!
        public GameStatesManager gameStatesManager { protected get; set; }
        public GameStateState State
        {
            get { return state; }
            set { state = value; }
        }

        public bool IsPopup { get; protected set; }

        public virtual void LoadContent(ContentManager content) { }

        public virtual void Update(InputManager inputManager, GameTime gameTime/*, bool otherScreenHasFocus, bool coveredByOtherScreen*/) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            float frameRate;
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.DrawString(spriteFont, "FPS: " + Math.Round(frameRate), new Vector2(graphicsManager.ScreenSize.X - 70, 5), Color.White);
        }

        public virtual void UnloadContent() { }
    }
}