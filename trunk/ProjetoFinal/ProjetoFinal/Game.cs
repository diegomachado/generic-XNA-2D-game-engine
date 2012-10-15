using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PerformanceUtility.GameDebugTools;

using ProjetoFinal.GameStateEngine;
using ProjetoFinal.Managers;
using ProjetoFinal.GameStateEngine.GameStates;

namespace ProjetoFinal
{
    public class ProjetoFinal : Game
    {
        GraphicsDeviceManager graphics;
        GameStatesManager gameStatesManager;
        GraphicsManager graphicsManager = GraphicsManager.Instance;
        
        //DebugSystem debugSystem;
        //Vector2 debugPos = new Vector2(100, 100);

        public ProjetoFinal()
        {
            IsMouseVisible = true;

            Content.RootDirectory = "Content";

            //IsFixedTimeStep = false;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = graphicsManager.screen.Width;
            graphics.PreferredBackBufferHeight = graphicsManager.screen.Height;
            graphics.SynchronizeWithVerticalRetrace = false;

            graphics.ApplyChanges();

            gameStatesManager = new GameStatesManager(this);
            Components.Add(gameStatesManager);
        }

        protected override void Initialize()
        {
            TextureManager.Instance.setContent(Content, GraphicsDevice);
            gameStatesManager.AddState(new MainMenuState());
            gameStatesManager.Initialize();
            
            DebugSystem.Initialize(this, "DebugFont");
            DebugSystem.Instance.FpsCounter.Visible = true;
            DebugSystem.Instance.TimeRuler.Visible = true;
            DebugSystem.Instance.TimeRuler.ShowLog = true;

            // register a new command that lets us move a sprite on the screen
            //debugSystem.DebugCommandUI.RegisterCommand(
            //    "pos",              // Name of command
            //    "set position",     // Description of command
            //    PosCommand          // Command execution delegate
            //    );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //gameStatesManager.LoadContent();
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        { 
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        /// <summary>
        /// This method is called from DebugCommandHost when the user types the 'pos'
        /// command into the command prompt. This is registered with the command prompt
        /// through the DebugCommandUI.RegisterCommand method we called in Initialize.
        /// </summary>
        //void PosCommand(IDebugCommandHost host, string command, IList<string> arguments)
        //{
        //    // if we got two arguments from the command
        //    if (arguments.Count == 2)
        //    {
        //        // process text "pos xPos yPos" by parsing our two arguments
        //        debugPos.X = Single.Parse(arguments[0], CultureInfo.InvariantCulture);
        //        debugPos.Y = Single.Parse(arguments[1], CultureInfo.InvariantCulture);
        //    }
        //    else
        //    {
        //        // if we didn't get two arguments, we echo the current position of the cat
        //        host.Echo(String.Format("Pos={0},{1}", debugPos.X, debugPos.Y));
        //    }
        //}

    }

#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (ProjetoFinal game = new ProjetoFinal())
            {
                game.Run();
            }
        }
    }
#endif
}