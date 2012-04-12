using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using OgmoLibrary;

namespace ProjetoFinal.Managers
{
    class MapManager
    {
        Map currentMap;

        public MapManager()
        {

        }

        public MapManager(ContentManager Content)
        {
            currentMap = Content.Load<Map>(@"maps/big");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentMap.Draw(spriteBatch);
        }

    }
}
