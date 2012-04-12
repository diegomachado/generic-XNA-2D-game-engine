using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using OgmoLibrary;

using TWrite = OgmoLibrary.Map;

namespace OgmoContentPipelineExtension
{
    [ContentTypeWriter]
    public class MapWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            Layer currentLayer;
            List<Tile> tiles;

            output.Write(value.MapSize.X);
            output.Write(value.MapSize.Y);
            output.Write(value.TileSize.X);
            output.Write(value.TileSize.Y);

            output.Write(value.Layers.Count());

            foreach(KeyValuePair<string, Layer> layer in value.Layers)
            {
                tiles = layer.Value.Tiles;
                currentLayer = layer.Value;

                output.Write(currentLayer.Name);
                output.Write(currentLayer.ExportMode);                
                
                output.Write(tiles.Count);

                foreach (Tile tile in tiles)
                {
                    output.Write(tile.Position.X);
                    output.Write(tile.Position.Y);
                    output.Write(tile.Id);
                }

                if (!string.IsNullOrEmpty(currentLayer.SpriteSheetPath))
                    output.Write(currentLayer.SpriteSheetPath);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "OgmoLibrary.MapReader, OgmoLibrary";
        }
    }
}
