using System;
using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace OgmoEditorLibrary
{
    public class Level
    {
        public string name;
        public int width, height;
        public Grid grid;
        public TileMap[] tileMaps;
        public List<LevelEntity> levelEntities;

        Random randomizer = new Random();

        // TODO: Ainda to achando esquisito definir o tamanho dos tiles aqui
        // Tirar e defini-los após instanciação do mapa
        public Level(string name, XmlDocument levelFile, ContentManager content)
        {
            this.name = name;
            width = ExtractWidth(levelFile);
            height = ExtractHeight(levelFile);
            grid = ExtractGrid(levelFile, width, height, 32, 32);
            tileMaps = ExtractTileMaps(levelFile, width, height, 32, 32, content);
            levelEntities = ExtractLevelEntities(levelFile);
        }
        
        public void LoadContent(XmlDocument levelFile)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screenSize)
        {
            for (int i = 0; i < tileMaps.Length; i++)
                tileMaps[i].Draw(spriteBatch, cameraPos, screenSize);
        }

        #region Level Constructors        

        private int ExtractWidth(XmlDocument levelFile)
        {
            return Convert.ToInt32(levelFile.SelectSingleNode("/level").Attributes["width"].Value);
        }

        private int ExtractHeight(XmlDocument levelFile)
        {
            return Convert.ToInt32(levelFile.SelectSingleNode("/level").Attributes["height"].Value);
        }

        private Grid ExtractGrid(XmlDocument levelFile, int width, int height, int tileWidth, int tileHeight)
        {            
            grid = new Grid(width, height, tileWidth, tileHeight);
            grid.LoadFromString(levelFile.SelectSingleNode("/level/*[@exportMode='Bitstring']").InnerText);
            return grid;
        }

        private TileMap[] ExtractTileMaps(XmlDocument levelFile, int levelWidth, int levelHeight, int tileWidth, int tileHeight, ContentManager content)
        {
            Texture2D tileset;
            XmlNodeList tileMapNodes = levelFile.SelectNodes("/level/*[@exportMode='XML']");

            tileMaps = new TileMap[tileMapNodes.Count];

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

        private List<LevelEntity> ExtractLevelEntities(XmlDocument levelFile)
        {
            List<LevelEntity> levelEntities = new List<LevelEntity>();
            XmlNodeList entitiesNodes = levelFile.SelectNodes("/level/Entities/*");

            foreach (XmlNode entity in entitiesNodes)
            {
                levelEntities.Add(new LevelEntity(entity.Name, 
                                                  int.Parse(entity.Attributes["id"].Value), 
                                                  int.Parse(entity.Attributes["x"].Value), 
                                                  int.Parse(entity.Attributes["y"].Value)));
            }

            return levelEntities;
        }
        
        #endregion

        public List<LevelEntity> EntitiesByType(string type)
        {
            return levelEntities.FindAll(entity => entity.type == type);
        }

        public Vector2 GetRandomSpawnPoint()
        {
            return new Vector2(levelEntities[randomizer.Next(levelEntities.Count)].position.X, 
                               levelEntities[randomizer.Next(levelEntities.Count)].position.Y);
        }

    }
}
