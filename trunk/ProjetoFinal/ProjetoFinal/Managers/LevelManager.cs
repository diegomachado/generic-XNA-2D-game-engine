using System;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using OgmoEditorLibrary;

namespace ProjetoFinal.Managers
{
    class LevelManager
    {
        XmlDocument levelFile;
        Level currentLevel;

        private static LevelManager instance;
        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LevelManager();

                return instance;
            }
        }

        ContentManager content;
        public ContentManager Content
        {
            set { content = value; }
        }

        public Grid Grid { get { return currentLevel.grid; } }

        public int LevelWidth { get { return currentLevel.width; } }

        public int LevelHeight { get { return currentLevel.height; } }

        public bool IsCurrentLevelLoaded { get { return (currentLevel != null); } }

        // Tirar esse Load Level daqui e colocar em Level.
        public void LoadLevel(string levelName)
        {
            levelFile = content.Load<XmlDocument>(@"levels/" + levelName);

            XmlNodeList tileMapNodes;
            Grid grid;
            TileMap[] tileMaps;
            Texture2D tileset;
            int x, y, id, levelWidth, levelHeight;
            string bitstring;

            XmlNode root = levelFile.SelectSingleNode("/level");

            levelWidth = Convert.ToInt32(levelFile.SelectSingleNode("/level").Attributes["width"].Value);
            levelHeight = Convert.ToInt32(levelFile.SelectSingleNode("/level").Attributes["height"].Value);

            bitstring = levelFile.SelectSingleNode("/level/*[@exportMode='Bitstring']").InnerText;
            grid = new Grid(levelWidth, levelHeight, 32, 32);
            grid.LoadFromString(bitstring);

            tileMapNodes = levelFile.SelectNodes("/level/*[@exportMode='XML']");
            tileMaps = new TileMap[tileMapNodes.Count];
            for (int i = 0; i < tileMapNodes.Count; i++)
            {
                tileset = content.Load<Texture2D>(@"levels/" + tileMapNodes[0].Attributes["tileset"].Value);
                tileMaps[i] = new TileMap(tileMapNodes[i].Name, tileset, levelWidth, levelHeight, 32, 32);

                foreach (XmlNode tile in tileMapNodes[i].ChildNodes)
                {
                    x = int.Parse(tile.Attributes["x"].Value);
                    y = int.Parse(tile.Attributes["y"].Value);
                    id = int.Parse(tile.Attributes["id"].Value);
                    tileMaps[i].setTile(x, y, id);
                }
            }



            currentLevel = new Level("test_level", levelWidth, levelHeight, grid, tileMaps);
        }

        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screen)
        {
            currentLevel.Draw(spriteBatch, cameraPos, screen);
        }
    }
}
