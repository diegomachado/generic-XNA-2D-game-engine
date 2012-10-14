using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// Type we want to import.
using TImport = System.Xml.XmlDocument;

namespace OgmoContentPipelineExtension
{
    [ContentImporter(".oel", DisplayName = "Ogmo Map Importer", DefaultProcessor = "MapProcessor")]
    public class MapImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDocument mapFile = new XmlDocument();

            mapFile.LoadXml(System.IO.File.ReadAllText(filename));
            
            return mapFile;
        }
    }
}
