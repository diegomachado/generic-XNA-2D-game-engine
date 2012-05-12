using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OgmoLibrary
{
    public class Map
    {
        public Point MapSize { get; set; }
        public Point TileSize { get; set; } 
        public Dictionary<string, Layer> Layers;

        public Map()
        {
        }
        
        public Map(Point mapSize, Point tileSize, Dictionary<string, Layer> layers)
        {
            MapSize = mapSize;
            TileSize = tileSize;
            Layers = layers;
        }

        // TODO: Develop dat xit Map::MapToString()
        public void MapToString()
        {          
        }

        public void Draw(SpriteBatch spriteBatch, Point cameraPosition)
        {
            int x, y, id;
            Texture2D spriteSheet;
            Layer currentLayer;

            Point tileSize = this.TileSize;

            var orderedLayers = from layer in Layers.Keys
                                orderby Layers[layer].ZIndex descending
                                select layer;

            foreach (string layerName in orderedLayers)
            {
                currentLayer = Layers[layerName];

                if (currentLayer.ExportMode == "XML")
                {
                    spriteSheet = currentLayer.SpriteSheet;

                    foreach (KeyValuePair<Point, Tile> tile in currentLayer.Tiles)
                    {
                        x = tile.Key.X;
                        y = tile.Key.Y;
                        id = tile.Value.Id;

                        // TODO: Colocar essas contas em um metodo de Camera
                        spriteBatch.Draw(spriteSheet,
                                            new Rectangle(x * tileSize.X - cameraPosition.X, y * tileSize.Y - cameraPosition.Y, tileSize.X, tileSize.Y),
                                            new Rectangle((id * tileSize.X) % spriteSheet.Width,
                                                        ((id * tileSize.X) / spriteSheet.Width) * tileSize.Y,
                                                        tileSize.X,
                                                        tileSize.Y),
                                            Color.White);
                    }
                }
            }
        }
    }
}
