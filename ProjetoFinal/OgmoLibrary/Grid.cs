using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace OgmoEditorLibrary
{
    public class Grid
    {
        public int[,] tiles;
        public int width, height, columns, rows;
        public Rectangle tile;

        public Grid(int width, int height, int tileWidth, int tileHeight)
        {
            if (width == 0 || height == 0 || tileWidth == 0 || tileHeight == 0)
                throw new Exception("Illegal Grid, sizes cannot be 0.");

            this.width = width;
            this.height = height;
            columns = width / tileWidth;
            rows = height / tileHeight;
            tile = new Rectangle(0, 0, tileWidth, tileHeight);
            tiles = new int[columns, rows];
        }

        public void LoadFromString(string bitString)
        {
            string[] rows;
            bitString = bitString.Trim();
            rows = Regex.Split(bitString, "\n");
            
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                    tiles[j, i] = int.Parse(rows[i][j].ToString());
            }
        }

        public bool TileAt(int x, int y)
        {
            return Convert.ToBoolean(tiles[x, y]);
        }

    }
}
