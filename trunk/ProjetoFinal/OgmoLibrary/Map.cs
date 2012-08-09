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

            Rectangle destinationRectangle = new Rectangle(0, 0, tileSize.X, tileSize.Y);
            Rectangle sourceRectangle = new Rectangle(0, 0, tileSize.X, tileSize.Y);

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

                        destinationRectangle.X = x * tileSize.X - cameraPosition.X;
                        destinationRectangle.Y = y * tileSize.Y - cameraPosition.Y;

                        sourceRectangle.X = (id * tileSize.X) % spriteSheet.Width;
                        sourceRectangle.Y = ((id * tileSize.X) / spriteSheet.Width) * tileSize.Y;

                        spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White);
                    }
                }
            }
        }

        public void DrawEfficiently(SpriteBatch spriteBatch, Point cameraPosition, Point screenSize)
        {
            int x, y, id;
            Texture2D spriteSheet;
            Layer currentLayer;

            Point tileSize = this.TileSize;

            Rectangle destinationRectangle = new Rectangle(0, 0, tileSize.X, tileSize.Y);
            Rectangle sourceRectangle = new Rectangle(0, 0, tileSize.X, tileSize.Y);

            var orderedLayers = from layer in Layers.Keys
                                orderby Layers[layer].ZIndex descending
                                select layer;

            Point firstTile = PositionToTileCoord(cameraPosition);
            Point lastTile = PositionToTileCoord(new Point(cameraPosition.X + screenSize.X, cameraPosition.Y + screenSize.Y));

            //Console.WriteLine(firstTile.ToString());
            //Console.WriteLine(lastTile.ToString());
            
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

                        // TODO: Rever lógica com o Guifes
                        if (x < firstTile.X || y < firstTile.Y || x > lastTile.X || y > lastTile.Y)
                            continue;

                        // Por que isso buga? Ordem dos Tiles no Dictionary?
                        // if( x > lastTile.X || y > lastTile.Y )
                        //      break;

                        id = tile.Value.Id;

                        destinationRectangle.X = x * tileSize.X - cameraPosition.X;
                        destinationRectangle.Y = y * tileSize.Y - cameraPosition.Y;

                        sourceRectangle.X = (id * tileSize.X) % spriteSheet.Width;
                        sourceRectangle.Y = ((id * tileSize.X) / spriteSheet.Width) * tileSize.Y;

                        spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White);
                    }
                }
            }
        }

        public Point PositionToTileCoord(Point position)
        {
            return new Point((int)position.X / TileSize.X, (int)position.Y / TileSize.Y);
        }
    }
}
