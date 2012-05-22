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
    class MapManager
    {
        Map currentMap;

        public MapManager()
        {

        }

        public MapManager(ContentManager Content)
        {
            currentMap = Content.Load<Map>(@"maps/big");
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

        public void Draw(SpriteBatch spriteBatch, Point cameraPosition)
        {
            currentMap.Draw(spriteBatch, cameraPosition);
        }

        public void DrawEfficiently(SpriteBatch spriteBatch, Point cameraPosition, Point firstTile, Point lastTile)
        {
            currentMap.DrawEfficiently(spriteBatch, cameraPosition, firstTile, lastTile);
        }
    }
}
