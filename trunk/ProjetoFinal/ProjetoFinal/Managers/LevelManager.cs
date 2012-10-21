using System;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using OgmoEditorLibrary;
using System.Collections.Generic;

namespace ProjetoFinal.Managers
{
    class LevelManager
    {
        XmlDocument levelFile;
        List<Level> levels = new List<Level>();
        public Level currentLevel;

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

        public void LoadLevel(string levelName)
        {
            levelFile = content.Load<XmlDocument>(@"levels/" + levelName);
            currentLevel = new Level("battlefield", Level.GetWidth(levelFile), Level.GetHeight(levelFile));
            currentLevel.grid = new Grid(Level.GetWidth(levelFile), Level.GetHeight(levelFile), 32, 32);
            currentLevel.grid.LoadFromString(Level.GetGridData(levelFile));
            currentLevel.tileMaps = Level.ExtractTileMaps(levelFile, Level.GetWidth(levelFile), Level.GetHeight(levelFile), 32, 32, content);
            currentLevel.levelEntities = Level.GetLevelEntities(levelFile);
            levels.Add(currentLevel);
        }

        public static TileMap[] ExtractTileMaps(XmlDocument levelFile, int levelWidth, int levelHeight, int tileWidth, int tileHeight, ContentManager content)
        {
            Texture2D tileset;
            XmlNodeList tileMapNodes = levelFile.SelectNodes("/level/*[@exportMode='XML']");

            TileMap[] tileMaps = new TileMap[tileMapNodes.Count];

            for (int i = 0; i < tileMapNodes.Count; i++)
            {
                tileset = content.Load<Texture2D>(@"levels/" + tileMapNodes[0].Attributes["tileset"].Value);
                tileMaps[i] = new TileMap(tileMapNodes[i].Name, tileset, levelWidth, levelHeight, tileWidth, tileHeight);

                foreach (XmlNode tile in tileMapNodes[i].ChildNodes)
                {
                    tileMaps[i].setTile(int.Parse(tile.Attributes["x"].Value),
                                        int.Parse(tile.Attributes["y"].Value),
                                        int.Parse(tile.Attributes["id"].Value));
                }
            }

            return tileMaps;
        }

        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screen)
        {
            currentLevel.Draw(spriteBatch, cameraPos, screen);
        }
    }
}
