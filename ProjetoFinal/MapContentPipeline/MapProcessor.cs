﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

using TiledLib;

namespace MapContentPipeline
{
    // Each tile has a texture, source rect, and sprite effects.
    [ContentSerializerRuntimeType("ProjetoFinal.Entities.Tile, ProjetoFinal")]
    public class ProjetoFinalTileContent
    {
        public ExternalReference<Texture2DContent> Texture;
        public Rectangle SourceRectangle;
        public SpriteEffects SpriteEffects;
    }

    // For each layer, we store the size of the layer and the tiles.
    [ContentSerializerRuntimeType("ProjetoFinal.Entities.Layer, ProjetoFinal")]
    public class ProjetoFinalLayerContent
    {
        public int Width;
        public int Height;
        public ProjetoFinalTileContent[] Tiles;
    }

    // For the map itself, we just store the size, tile size, and a list of layers.
    [ContentSerializerRuntimeType("ProjetoFinal.Entities.Map, ProjetoFinal")]
    public class ProjetoFinalMapContent
    {
        public int TileWidth;
        public int TileHeight;
        public List<ProjetoFinalLayerContent> Layers = new List<ProjetoFinalLayerContent>();
    }

    [ContentProcessor(DisplayName = "TMX Processor - ProjetoFinal")]
    public class MapProcessor : ContentProcessor<MapContent, ProjetoFinalMapContent>
    {
        public override ProjetoFinalMapContent Process(MapContent input, ContentProcessorContext context)
        {
            // build the textures
            TiledHelpers.BuildTileSetTextures(input, context);

            // generate source rectangles
            TiledHelpers.GenerateTileSourceRectangles(input);

            // now build our output, first by just copying over some data
            ProjetoFinalMapContent output = new ProjetoFinalMapContent
            {
                TileWidth = input.TileWidth,
                TileHeight = input.TileHeight
            };

            // iterate all the layers of the input
            foreach (LayerContent layer in input.Layers)
            {
                // we only care about tile layers in our demo
                TileLayerContent tlc = layer as TileLayerContent;
                if (tlc != null)
                {
                    // create the new layer
                    ProjetoFinalLayerContent outLayer = new ProjetoFinalLayerContent
                    {
                        Width = tlc.Width,
                        Height = tlc.Height,
                    };

                    // we need to build up our tile list now
                    outLayer.Tiles = new ProjetoFinalTileContent[tlc.Data.Length];
                    for (int i = 0; i < tlc.Data.Length; i++)
                    {
                        // get the ID of the tile
                        uint tileID = tlc.Data[i];

                        // use that to get the actual index as well as the SpriteEffects
                        int tileIndex;
                        SpriteEffects spriteEffects;
                        TiledHelpers.DecodeTileID(tileID, out tileIndex, out spriteEffects);

                        // figure out which tile set has this tile index in it and grab
                        // the texture reference and source rectangle.
                        ExternalReference<Texture2DContent> textureContent = null;
                        Rectangle sourceRect = new Rectangle();

                        // iterate all the tile sets
                        foreach (var tileSet in input.TileSets)
                        {
                            // if our tile index is in this set
                            if (tileIndex - tileSet.FirstId < tileSet.Tiles.Count)
                            {
                                // store the texture content and source rectangle
                                textureContent = tileSet.Texture;
                                sourceRect = tileSet.Tiles[(int)(tileIndex - tileSet.FirstId)].Source;

                                // and break out of the foreach loop
                                break;
                            }
                        }

                        // now insert the tile into our output
                        outLayer.Tiles[i] = new ProjetoFinalTileContent
                        {
                            Texture = textureContent,
                            SourceRectangle = sourceRect,
                            SpriteEffects = spriteEffects
                        };
                    }

                    // add the layer to our output
                    output.Layers.Add(outLayer);
                }
            }

            // return the output object. because we have ContentSerializerRuntimeType attributes on our
            // objects, we don't need a ContentTypeWriter and can just use the automatic serialization.
            return output;
        }
    }
}