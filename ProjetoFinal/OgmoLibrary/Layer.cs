using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace OgmoLibrary
{
    public class Layer
    {
        public string Name { get; set; }
        public string SpriteSheetPath { get; set; } 
        public Texture2D SpriteSheet { get; set; }
        
        public Dictionary<int, Dictionary<int, Tile>> Tiles { get; set; }
        public int ZIndex { get; set; }
        public string ExportMode { get; set; }

        public Layer()
        {
        }

        public Layer(string name, Dictionary<int, Dictionary<int, Tile>> tiles, string exportMode, int zIndex)
        {
            Name = name;
            Tiles = tiles;
            ExportMode = exportMode;            
            ZIndex = zIndex;
        }

        public Layer(string name, Dictionary<int, Dictionary<int, Tile>> tiles, string spriteSheetPath, string exportMode, int zIndex)
        {
            Name = name;
            Tiles = tiles;
            SpriteSheetPath = spriteSheetPath;
            ExportMode = exportMode;
            ZIndex = zIndex;
        }

        public Layer(string name, Dictionary<int, Dictionary<int, Tile>> tiles, Texture2D spriteSheet, string exportMode, int zIndex)
        {
            Name = name;
            Tiles = tiles;
            SpriteSheet = spriteSheet;
            ExportMode = exportMode;
            ZIndex = zIndex;
        }
        
        public Tile GetTileId(Point xy)
        {
            return Tiles[xy.X][xy.Y];
        }
    }
}
