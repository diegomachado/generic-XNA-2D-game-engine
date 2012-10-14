using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OgmoEditorLibrary
{
    public class TileMap
    {
        public string name;
        public Texture2D tileset;
        public int width, height, rows, columns;
        public Rectangle tile;
        public int[,] tiles;

        public TileMap(string name, Texture2D tileset, int width, int height, int tileWidth, int tileHeight)
        {
            this.name = name;
            this.tileset = tileset;
            this.width = width;
            columns = width / tileWidth;
            rows = height / tileHeight;
            this.height = height;
            tile = new Rectangle(0, 0, tileWidth, tileHeight);
            tiles = new int[columns, rows];
            ResetTiles();
        }

        public void setTile(int x, int y, int value)
        {
            tiles[x, y] = value;
        }

        public int TileAt(int x, int y)
        {
            return tiles[x,y];
        }

        public int TileAtPixel(int x, int y)
        {
            return tiles[x / tile.Width, y / tile.Height];
        }

        private Point startTile, endTile;
        private Rectangle source, destination;
        public void Draw(SpriteBatch spriteBatch, Point cameraPos, Rectangle screenSize)
        {
            source.Width = destination.Width = tile.Width;
            source.Height = destination.Height = tile.Height;
            
            startTile.X = cameraPos.X / tile.Width;
            startTile.Y = cameraPos.Y / tile.Height;
            endTile.X = (int)Math.Min((cameraPos.X + screenSize.Width) / tile.Width + 1, tiles.GetLength(0));
            endTile.Y = (int)Math.Min((cameraPos.Y + screenSize.Height) / tile.Height + 1, tiles.GetLength(1));
            
            for (int col = startTile.X; col < endTile.X; col++)
            {
                for (int row = startTile.Y; row < endTile.Y; row++)
                {
                    if (tiles[col, row] == -1) continue;

                    destination.X = col * tile.Width - cameraPos.X;
                    destination.Y = row * tile.Height - cameraPos.Y;

                    source.X = (tiles[col, row] * tile.Width) % tileset.Width;
                    source.Y = ((tiles[col, row] * tile.Width) / tileset.Width) * tile.Height;
                    
                    spriteBatch.Draw(tileset, destination, source, Color.White);
                }
            }
        }       

        private void ResetTiles()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
                for (int j = 0; j < tiles.GetLength(1); j++)
                    tiles[i, j] = -1;
        }

    }
}
