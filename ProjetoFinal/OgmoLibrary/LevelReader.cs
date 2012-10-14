using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

using TRead = System.Xml.XmlDocument;

namespace OgmoEditorLibrary
{
    public class LevelReader : ContentTypeReader<TRead>
    {
        XmlDocument levelXml = new XmlDocument();

        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            levelXml.InnerXml = input.ReadString();
            return levelXml;
        }
    }
}