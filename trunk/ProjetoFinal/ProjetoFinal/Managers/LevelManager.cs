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

        public void LoadLevel(string levelName)
        {
            levelFile = content.Load<XmlDocument>(@"levels/" + levelName);
            currentLevel = new Level("battlefield", levelFile, content);
            levels.Add(currentLevel);
        }       

        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screen)
        {
            currentLevel.Draw(spriteBatch, cameraPos, screen);
        }
    }
}
