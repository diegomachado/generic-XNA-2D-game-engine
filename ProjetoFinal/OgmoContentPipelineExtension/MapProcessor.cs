using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using System.Xml;

using OgmoLibrary;

using TInput = System.Xml.XmlDocument;
using TOutput = OgmoLibrary.Map;

namespace OgmoContentPipelineExtension
{
    [ContentProcessor(DisplayName = "Ogmo Map Processor")]
    public class MapProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // Map Values
            Point mapSize, tileSize;

            // XML Values
            XmlNode levelNode;
            XmlNodeList layerNodes, tileNodes;        
            string exportMode, layerName;

            // Layer Values
            string spriteSheetPath;

            // Tile Values
            int tileX, tileY, tileId;
            string bitString;            
            List<string> bitStringLines = new List<string>();

            // Collections
            Dictionary<string, Layer> layers = new Dictionary<string,Layer>();        

            // Misc
            int rows, columns;

            /*
             * Utilizing input as XmlDocument
             * TODO: Thinking about using XmlReader, to check if is faster (think about big XML files)
             */
            levelNode = input.SelectSingleNode("/level");

            /* 
             * TODO: Do a Try/Catch in these statements
             * TODO: Create a way to set these values by Class Method 
             */
            mapSize = new Point(int.Parse(levelNode.Attributes["width"].Value),
                                 int.Parse(levelNode.Attributes["height"].Value));

            tileSize = new Point(int.Parse(levelNode.Attributes["TileWidth"].Value),
                                 int.Parse(levelNode.Attributes["TileHeight"].Value));

            layerNodes = input.SelectNodes("/level/*");

            foreach (XmlNode node in layerNodes)
            {
                exportMode = node.Attributes["exportMode"].Value;
                
                if(!string.IsNullOrEmpty(exportMode))
                {
                    switch (exportMode)
                    {
                        case "XML":
                        {
                            Dictionary<int, Dictionary<int, Tile>> tiles = new Dictionary<int, Dictionary<int, Tile>>();
                            
                            layerName = node.Name;
                            spriteSheetPath = node.Attributes["tileset"].Value;
                            
                            tileNodes = node.ChildNodes;
                            foreach (XmlNode tileNode in tileNodes)
                            {
                                tileX = int.Parse(tileNode.Attributes["x"].Value);
                                tileY = int.Parse(tileNode.Attributes["y"].Value);
                                tileId = int.Parse(tileNode.Attributes["id"].Value);

                                if (!tiles.ContainsKey(tileX))
                                    tiles.Add(tileX, new Dictionary<int, Tile>());

                                tiles[tileX].Add(tileY, new Tile(new Point(tileX, tileY), tileId));
                            }

                            layers.Add(layerName, new Layer(layerName, tiles, spriteSheetPath ,exportMode, 0));

                            break;
                        }

                        case "XMLCoords":
                            // TODO: Develop the XMLCoords type Node
                            break;

                        case "CSV":
                            // TODO: Develop the CSV type Node
                            break;
                    
                        case "Bitstring":
                        {
                            Dictionary<int, Dictionary<int, Tile>> tiles = new Dictionary<int, Dictionary<int, Tile>>();
                            layerName = node.Name;

                            bitString = node.InnerText.Replace(" ", "");
                            bitStringLines = bitString.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                            
                            for (int i = 0; i < bitStringLines.Count; i++)
                                if (string.IsNullOrEmpty(bitStringLines[i]) || string.IsNullOrWhiteSpace(bitStringLines[i]))
                                    bitStringLines.RemoveAt(i);
                        
                            rows = bitStringLines.Count();
                            columns = bitStringLines[0].Length;

                            for (int column = 0; column < columns; column++)
                            {
                                tiles.Add(column, new Dictionary<int, Tile>());

                                for (int row = 0; row < rows; row++)
                                    tiles[column].Add(row, new Tile(new Point(column, row), (int)Char.GetNumericValue(bitStringLines[row][column])));
                            }

                            layers.Add(layerName, new Layer(layerName, tiles, exportMode, 0));

                            break;
                        }

                        default:
                            //TODO: Throw an Exception here
                            break;
                    }
                }
            }

            foreach (KeyValuePair<string, Layer> layer in layers)
            {
                Console.WriteLine(layer.Value.Tiles.Count);
            }

            return new OgmoLibrary.Map(mapSize, tileSize, layers);
        }
    }
}

/* Fuck dat xit
            List<Tile> tileShitness = new List<Tile>();

            tileShitness.Add(new Tile(new Point(0, 0), 1));

            Console.WriteLine("tileShitness.Cunt: " + tileShitness.Count);

            tileShitness = new List<Tile>();
            tileShitness.Add(new Tile(new Point(1, 1), 0));*/