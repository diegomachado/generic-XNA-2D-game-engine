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

        public string MapToString()
        {
            string s = "";

            foreach(KeyValuePair<string, Layer> layer in Layers)
            {
                s += "Layer:" + layer.Key + Environment.NewLine;
                s += "Tile Count: " + layer.Value.Tiles.Count + Environment.NewLine + Environment.NewLine;
                
                foreach (Tile tile in layer.Value.Tiles)
                    s += "[" + tile.Position.X + "," + tile.Position.Y + "] - " + tile.Id + Environment.NewLine;

                s += "----------------------------------" + Environment.NewLine;                
            }

            return s;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x, y, id;
            Texture2D spriteSheet;

            Point tileSize = this.TileSize;

            foreach (KeyValuePair<string, Layer> layer in Layers)
            {
                if (layer.Value.ExportMode == "XML")
                {
                    spriteSheet = layer.Value.SpriteSheet;

                    foreach (Tile tile in layer.Value.Tiles)
                    {
                        x = tile.Position.X;
                        y = tile.Position.Y;
                        id = tile.Id;

                        spriteBatch.Draw(spriteSheet,
                                         new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y),
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
