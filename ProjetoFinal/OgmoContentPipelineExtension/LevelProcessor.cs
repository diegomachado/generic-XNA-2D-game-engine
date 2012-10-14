using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.Xml.XmlDocument;
using TOutput = System.Xml.XmlDocument;

namespace OgmoContentPipelineExtension
{
    [ContentProcessor(DisplayName = "Ogmo Editor Level Processor")]
    public class LevelProcessor : ContentProcessor<TInput, TOutput>
    {        
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return input;
        }
    }
}