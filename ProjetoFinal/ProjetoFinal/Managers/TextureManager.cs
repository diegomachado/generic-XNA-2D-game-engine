using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Managers
{
    public enum TextureList {
        Bear,
        Ranger
    }

    class TextureManager
    {
        ContentManager Content;

        public TextureManager(ContentManager Content)
        {
            this.Content = Content;
        }

        public Texture2D getTexture(TextureList texture)
        {
            String textureName = "bear";

            switch (texture)
            {
                case TextureList.Bear:
                    textureName = "bear";
                    break;

                case TextureList.Ranger:
                    textureName = "ranger";
                    break;
            }

            return this.Content.Load<Texture2D>( String.Format(@"sprites/{0}", textureName) );       
        }
    }
}
