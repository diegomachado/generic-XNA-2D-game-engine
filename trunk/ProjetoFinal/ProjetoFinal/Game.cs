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
            IsMouseVisible = true;

            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = graphicsManager.ScreenSize.X;
            graphics.PreferredBackBufferHeight = graphicsManager.ScreenSize.Y;
            graphics.ApplyChanges();

            gameStatesManager = new GameStatesManager(this);

            Components.Add(gameStatesManager);
        }

        protected override void Initialize()
        {
            base.Initialize();

            TextureManager.Instance.setContent(Content, GraphicsDevice);
            //gameStatesManager.AddState(new GameplayState());
            gameStatesManager.AddState(new MainMenuState());
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
