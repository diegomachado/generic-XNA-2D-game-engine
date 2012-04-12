using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = OgmoLibrary.Map;

namespace OgmoLibrary
{
    public class MapReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            // Map Values
            Point mapSize, tileSize;
            int layerCount;

            // Layer Values
            string layerName, spriteSheetPath, exportMode;
            Texture2D spriteSheet;
            int tilesCount;
            
            // Collections
            Dictionary<string, Layer> layers = new Dictionary<string,Layer>();
            

            mapSize  = new Point(input.ReadInt32(), input.ReadInt32());
            tileSize = new Point(input.ReadInt32(), input.ReadInt32());

            layerCount = input.ReadInt32();

            for (int i = 0; i < layerCount; i++)
            {
                List<Tile> tiles = new List<Tile>();

                layerName  = input.ReadString();
                exportMode = input.ReadString();             

                tilesCount = input.ReadInt32();            
                for (int j = 0; j < tilesCount; j++)
                    tiles.Add( new Tile( new Point(input.ReadInt32(), input.ReadInt32()), input.ReadInt32()) );

                if (exportMode == "XML")
                {
                    spriteSheetPath = input.ReadString();
                    spriteSheet = input.ContentManager.Load<Texture2D>(spriteSheetPath);
                    layers.Add(layerName, new Layer(layerName, tiles, spriteSheet, exportMode, 0));

                    continue;
                }

                layers.Add(layerName, new Layer(layerName, tiles, exportMode, 0));
            }

            return new Map(mapSize, tileSize, layers);
        }
    }
}
