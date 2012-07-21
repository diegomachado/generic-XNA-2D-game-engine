using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace ProjetoFinal.GameStateEngine
{
    class GameStatesManager : DrawableGameComponent
    {
        List<GameState> states = new List<GameState>();
        //List<GameState> statesToUpdate = new List<GameState>();

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        InputManager inputManager = InputManager.Instance;
        NetworkManager networkManager = NetworkManager.Instance;

        public bool TraceEnabled { get; set; }

        bool isInitialized;
        
        public GameStatesManager(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>(@"fonts/SegoeUI");

            foreach (GameState state in states)
            {
                state.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (GameState state in states)
            {
                state.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            inputManager.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            //screensToUpdate.Clear();

            //foreach (GameState state in states)
            //    statesToUpdate.Add(state);

            //bool otherScreenHasFocus = !Game.IsActive;
            //bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (states.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameState state = states[states.Count - 1];

                //states.RemoveAt(states.Count - 1);

                // Update the screen.
                state.Update(this, inputManager, gameTime/*, otherScreenHasFocus, coveredByOtherScreen*/);

                /*if (state.ScreenState == ScreenState.TransitionOn || state.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        //screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    //if (!screen.IsPopup)
                    //    coveredByOtherScreen = true;
                }*/
            }

            networkManager.ProcessNetworkMessages();

            // Print debug trace?
            if (TraceEnabled)
                TraceScreens();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameState state in states)
            {
                // TODO: Mudar os estados dos estados (Ainda não fazemos isso!)
                if (state.State == GameStateState.Hidden)
                    continue;

                state.Draw(gameTime, spriteBatch, spriteFont);
            }
        }

        public void AddState(GameState state/*, PlayerIndex? controllingPlayer*/)
        {
            //screen.ControllingPlayer = controllingPlayer;
            //screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                state.LoadContent();
            }

            states.Add(state);
        }

        public void RemoveState(GameState state)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (isInitialized)
            {
                state.UnloadContent();
            }

            states.Remove(state);
            //screensToUpdate.Remove(screen);
        }

        void TraceScreens()
        {
            List<string> statesNames = new List<string>();

            foreach (GameState state in states)
                statesNames.Add(state.GetType().Name);

            Debug.WriteLine(string.Join(", ", statesNames.ToArray()));
        }

        public void ExitGame()
        {
            Game.Exit();
        }
    }
}
