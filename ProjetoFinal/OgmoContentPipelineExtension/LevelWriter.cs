using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = System.Xml.XmlDocument;

namespace OgmoContentPipelineExtension
{
    [ContentTypeWriter]
    public class LevelWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.OuterXml);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "OgmoEditorLibrary.LevelReader, OgmoEditorLibrary";
        }
    }
}