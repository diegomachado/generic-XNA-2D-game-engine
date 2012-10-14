using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = System.Xml.XmlDocument;

namespace OgmoContentPipelineExtension
{
    [ContentImporter(".oel", DisplayName = "Ogmo Editor Level Importer", DefaultProcessor = "LevelImporter")]
    public class LevelImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDocument levelFile = new XmlDocument();
            levelFile.LoadXml(System.IO.File.ReadAllText(filename));            
            return levelFile;
        }
    }
}
