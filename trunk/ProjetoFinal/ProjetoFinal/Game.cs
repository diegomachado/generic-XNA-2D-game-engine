using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public ProjetoFinal()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = graphicsManager.ScreenSize.X;
            graphics.PreferredBackBufferHeight = graphicsManager.ScreenSize.Y;
            graphics.ApplyChanges();

            gameStatesManager = new GameStatesManager(this);

            Components.Add(gameStatesManager);

            // TODO: Colocar os primeiros estados na lista
            //gameStatesManager.AddState(new BackgroundState(), null);
        }

        protected override void Initialize()
        {
            TextureManager.Instance.setContent(Content, GraphicsDevice);
            gameStatesManager.AddState(new GameplayState());

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
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
