using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using OgmoLibrary;

namespace ProjetoFinal.Managers
{
    public enum MapType
    {
        Level1,
    }

    class MapManager
    {
        private static MapManager instance;

        public ContentManager Content
        {
            set { content = value; }
        }
        ContentManager content;
       
        Map currentMap;

        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MapManager();

                return instance;
            }
        }

        public void LoadMap(MapType type)
        {
            switch(type)
            {
                case MapType.Level1:
                    currentMap = content.Load<Map>(@"maps/big");
                    break;
            }
        }

        public Layer GetCollisionLayer()
        {
            return currentMap.Layers["Collision"];
        }

        public Point GetMapSize()
        {
            return currentMap.MapSize;
        }

        public Point GetTileSize()
        {
            return currentMap.TileSize;
        }

        public void Update()
        {
        }      

        public void Draw(SpriteBatch spriteBatch, Point cameraPosition, Point firstTile, Point lastTile)
        {
            currentMap.DrawEfficiently(spriteBatch, cameraPosition, firstTile, lastTile);
        }

        public void DrawPoorly(SpriteBatch spriteBatch, Point cameraPosition)
        {
            currentMap.Draw(spriteBatch, cameraPosition);
        }

        public bool IsCurrentMapLoaded
        {
            get
            {
                return (currentMap != null);  
            }
        }
    }
}
