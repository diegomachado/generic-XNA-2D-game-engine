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
            int rows, columns;
            
            // Collections
            Dictionary<string, Layer> layers = new Dictionary<string,Layer>();

            mapSize  = new Point(input.ReadInt32(), input.ReadInt32());
            tileSize = new Point(input.ReadInt32(), input.ReadInt32());

            layerCount = input.ReadInt32();

            for (int i = 0; i < layerCount; i++)
            {
                Dictionary<int, Dictionary<int, Tile>> tiles = new Dictionary<int, Dictionary<int, Tile>>();

                layerName  = input.ReadString();
                exportMode = input.ReadString();

                columns = input.ReadInt32();
                rows = input.ReadInt32();

                for (int column = 0; column < columns; column++)
                {
                    tiles.Add(column, new Dictionary<int, Tile>());

                    for (int row = 0; row < rows; row++)
                        tiles[column].Add(row, new Tile(new Point(input.ReadInt32(), input.ReadInt32()), input.ReadInt32()));
                }

                if (exportMode == "XML")
                {
                    spriteSheetPath = input.ReadString();
                    spriteSheet = input.ContentManager.Load<Texture2D>(@"maps/" + spriteSheetPath);
                    layers.Add(layerName, new Layer(layerName, tiles, spriteSheet, exportMode, 0));

                    continue;
                }

                layers.Add(layerName, new Layer(layerName, tiles, exportMode, 0));
            }

            return new Map(mapSize, tileSize, layers);
        }
    }
}
