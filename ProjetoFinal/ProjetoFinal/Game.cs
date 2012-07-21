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

        public ProjetoFinal()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 608;
            graphics.ApplyChanges();

            gameStatesManager = new GameStatesManager(this);

            Components.Add(gameStatesManager);

            // TODO: Colocar os primeiros estados na lista
            //gameStatesManager.AddState(new BackgroundState(), null);
            gameStatesManager.AddState(new MainMenuState());
        }

        protected override void Initialize()
        {
            TextureManager.Instance.setContent(Content, GraphicsDevice);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }

    static class Program
    {
        static void Main()
        {
            using (ProjetoFinal game = new ProjetoFinal())
            {
                game.Run();
            }
        }
    }
}
