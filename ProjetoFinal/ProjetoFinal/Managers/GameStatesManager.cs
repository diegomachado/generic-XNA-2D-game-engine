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
            isInitialized = true;

            //TODO: Tirar saporra daqui
            Dictionary<string, string> options = SelectMenu();

            switch (int.Parse(options["type"]))
            {
                case 1:
                    networkManager.Host();
                    break;
                case 2:
                    networkManager.Connect();
                    break;
            }
            

            base.Initialize();
        }

        private Dictionary<string, string> SelectMenu()
        {
            Dictionary<string, string> returnValues = new Dictionary<string, string>();

            Console.WriteLine("==========================");
            Console.WriteLine("       What are you?      ");
            Console.WriteLine("==========================");
            Console.WriteLine("1. I'm a Server");
            Console.WriteLine("2. I'm a Client");
            returnValues.Add("type", Console.ReadLine());
            //Console.WriteLine("Type your nickname:");
            //returnValues.Add("nickname", Console.ReadLine());
            returnValues.Add("nickname", "BomberMacFaggot");

            //Console.WriteLine("Port?");
            //port = int.Parse(Console.ReadLine());

            return returnValues;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>(@"fonts/SegoeUI");

            foreach (GameState state in states)
            {
                state.LoadContent(content);
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
            //while (states.Count > 0)
            //{
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
            //}

            networkManager.ProcessNetworkMessages();

            // Print debug trace?
            if (TraceEnabled)
                TraceScreens();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (GameState state in states)
            {
                // TODO: Mudar os estados dos estados (Ainda não fazemos isso!)
                if (state.State == GameStateState.Hidden)
                    continue;

                state.Draw(gameTime, spriteBatch, spriteFont);
            }

            spriteBatch.End();
        }

        public void AddState(GameState state/*, PlayerIndex? controllingPlayer*/)
        {
            //screen.ControllingPlayer = controllingPlayer;
            //screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
                state.LoadContent(Game.Content);
            
            states.Add(state);
        }

        public void RemoveState(GameState state)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (isInitialized)
                state.UnloadContent();

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

        public void ResignState(GameState item)
        {
            states.Remove(item);
        }
    }
}
