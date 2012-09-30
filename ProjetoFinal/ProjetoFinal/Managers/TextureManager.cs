using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Managers
{
    public enum TextureList
    {
        Bear,
        Arrow,
        RandomSkin,
        CollisionBoxBorder,
        ButtonFrame,
    }

    class TextureManager
    {
        private static TextureManager instance;

        ContentManager Content;
        GraphicsDevice GraphicsDevice;

        public void setContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.Content = content;
            this.GraphicsDevice = graphicsDevice;
        }

        public static TextureManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureManager();

                return instance;
            }
        }

        public Texture2D getTexture(TextureList texture)
        {
            String textureName = "bear";
            String[] skinTextures = { "bear" };
            Random randomSkin = new Random();

            switch (texture)
            {
                case TextureList.Bear:
                    textureName = skinTextures[0];
                    break;
                    
                case TextureList.RandomSkin:
                    textureName = skinTextures[randomSkin.Next(0, skinTextures.Length)];
                    break;

                case TextureList.ButtonFrame:
                    textureName = "button";
                    break;

                case TextureList.Arrow:
                    textureName = "arrow";
                    break;
            }

            return this.Content.Load<Texture2D>(String.Format(@"sprites/{0}", textureName));
        }

        public Texture2D getEmptyTexture()
        {
            var t = new Texture2D(GraphicsDevice, 1, 1);
            return t;
        }

        public Texture2D GetPixelTexture()
        {
            return this.Content.Load<Texture2D>("pixel");
        }
    }
}
