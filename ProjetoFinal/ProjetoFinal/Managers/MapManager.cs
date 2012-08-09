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
        Map currentMap;

        private static MapManager instance;
        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MapManager();

                return instance;
            }
        }

        ContentManager content;
        public ContentManager Content
        {
            set { content = value; }
        }

        public Layer CollisionLayer { get { return currentMap.Layers["Collision"]; } }
        public Point MapSize { get { return currentMap.MapSize; } }
        public Point TileSize { get { return currentMap.TileSize; } }
        public bool IsCurrentMapLoaded { get { return (currentMap != null); } }

        public void LoadMap(MapType type)
        {
            switch (type)
            {
                case MapType.Level1:
                    currentMap = content.Load<Map>(@"maps/big");
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Point cameraPosition, Point screenSize)
        {
            currentMap.DrawEfficiently(spriteBatch, cameraPosition, screenSize);
        }

        public void DrawPoorly(SpriteBatch spriteBatch, Point cameraPosition)
        {
            currentMap.Draw(spriteBatch, cameraPosition);
        }
    }
}
