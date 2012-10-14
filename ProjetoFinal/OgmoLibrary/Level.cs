using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.Collections.Generic;

namespace OgmoEditorLibrary
{
    public class Level
    {
        public string name;
        public int width, height;
        public Grid grid;
        public TileMap[] tileMaps;
        public List<LevelEntity> levelEntities;

        public Level(string name, int width, int height, Grid grid, TileMap[] tileMaps)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.tileMaps = tileMaps;
            this.grid = grid;
        }
        
        public void LoadContent(XmlDocument levelFile)
        {           
        }

        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screenSize)
        {
            for (int i = 0; i < tileMaps.Length; i++)
                tileMaps[i].Draw(spriteBatch, cameraPos, screenSize);
        }

        public void entitiesByType(string type)
        {

        }

    }
}
