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
            int x, y, id, layerCount;

            // Layer Values
            string layerName, spriteSheetPath, exportMode;
            Texture2D spriteSheet;
            int zOrder, tileCount;
            
            // Collections
            Dictionary<string, Layer> layers = new Dictionary<string,Layer>();

            mapSize  = new Point(input.ReadInt32(), input.ReadInt32());
            tileSize = new Point(input.ReadInt32(), input.ReadInt32());
            layerCount = input.ReadInt32();

            for (int i = 0; i < layerCount; i++)
            {
                Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

                layerName  = input.ReadString();
                exportMode = input.ReadString();
                zOrder = input.ReadInt32();

                tileCount = input.ReadInt32();

                for (int tileIterator = 0; tileIterator < tileCount; tileIterator++)
                {
                    x = input.ReadInt32();
                    y = input.ReadInt32();
                    id = input.ReadInt32();

                    tiles.Add(new Point(x, y), new Tile(new Point(x,y), id));
                }

                if (exportMode == "XML")
                {
                    spriteSheetPath = input.ReadString();
                    spriteSheet = input.ContentManager.Load<Texture2D>(@"maps/" + spriteSheetPath);
                    layers.Add(layerName, new Layer(layerName, tiles, spriteSheet, exportMode, zOrder));

                    continue;
                }

                layers.Add(layerName, new Layer(layerName, tiles, exportMode, zOrder));
            }

            return new Map(mapSize, tileSize, layers);
        }
    }
}
