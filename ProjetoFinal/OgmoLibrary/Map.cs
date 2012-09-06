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
        
        public Map(Point mapSize, Point tileSize, Dictionary<string, Layer> layers)
        {
            MapSize = mapSize;
            TileSize = tileSize;
            Layers = layers;
        } 

        int x, y, id;
        Texture2D spriteSheet;
        Layer currentLayer;
        Point tileSize, firstTile, lastTile;
        Rectangle destinationRectangle, sourceRectangle;
        List<Layer> layersToDraw = new List<Layer>();
        public void Draw(SpriteBatch spriteBatch, Point cameraPosition, Point screenSize)
        {
            tileSize = this.TileSize;

            sourceRectangle.X      = destinationRectangle.X = 0;
            sourceRectangle.Y      = destinationRectangle.Y = 0;
            sourceRectangle.Width  = destinationRectangle.Width = tileSize.X;
            sourceRectangle.Height = destinationRectangle.Height = tileSize.Y;
                                                
            firstTile.X = cameraPosition.X / tileSize.X;
            firstTile.Y = cameraPosition.Y / tileSize.Y;
            lastTile.X = (cameraPosition.X + screenSize.X) / tileSize.X;
            lastTile.Y = (cameraPosition.Y + screenSize.Y) / tileSize.Y;

            foreach (KeyValuePair<string, Layer> layer in Layers)
            {
                currentLayer = layer.Value;

                if (currentLayer.ExportMode == "XML" && currentLayer.Tiles.Count > 0)
                {
                    spriteSheet = currentLayer.SpriteSheet;

                    foreach (KeyValuePair<Point, Tile> tile in currentLayer.Tiles)
                    {
                        x = tile.Key.X;
                        y = tile.Key.Y;

                        if (x < firstTile.X || y < firstTile.Y || x > lastTile.X || y > lastTile.Y)
                            continue;

                        if (x > lastTile.X || y > lastTile.Y)
                            break;

                        id = tile.Value.Id;

                        destinationRectangle.X = x * tileSize.X - cameraPosition.X;
                        destinationRectangle.Y = y * tileSize.Y - cameraPosition.Y;

                        sourceRectangle.X = (id * tileSize.X) % spriteSheet.Width;
                        sourceRectangle.Y = ((id * tileSize.X) / spriteSheet.Width) * tileSize.Y;

                        spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.Peru);
                    }
                }
            }
            
        }
    }
}
