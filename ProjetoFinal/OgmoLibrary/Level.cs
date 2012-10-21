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

        public Level(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
        }
        
        public void LoadContent(XmlDocument levelFile){ }

        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screenSize)
        {
            for (int i = 0; i < tileMaps.Length; i++)
                tileMaps[i].Draw(spriteBatch, cameraPos, screenSize);
        }

        #region Level Constructors        

        public static int GetWidth(XmlDocument levelFile)
        {
            return Convert.ToInt32(levelFile.SelectSingleNode("/level").Attributes["width"].Value);
        }
        
        public static int GetHeight(XmlDocument levelFile)
        {
            return Convert.ToInt32(levelFile.SelectSingleNode("/level").Attributes["height"].Value);
        }
        
        public static string GetGridData(XmlDocument levelFile)
        {
            return levelFile.SelectSingleNode("/level/*[@exportMode='Bitstring']").InnerText;            
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
        
        public static List<LevelEntity> GetLevelEntities(XmlDocument levelFile)
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
            int id = randomizer.Next(levelEntities.Count);
            return new Vector2(levelEntities[id].position.X, 
                               levelEntities[id].position.Y);
        }

    }
}
