using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using ProjetoFinal.Entities;
using ProjetoFinal.EventHeaders;
using ProjetoFinal.Managers.LocalPlayerStates;
using Microsoft.Xna.Framework.Input;
using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers
{
    class ArrowManager
    {
        Dictionary<short, Arrow> arrows;

        public ArrowManager()
        {
            arrows = new Dictionary<short, Arrow>();
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            foreach (KeyValuePair<short, Arrow> a in arrows)
            {
                Arrow arrow = a.Value;
                short playerId = a.Key;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Arrow arrow;

            foreach (KeyValuePair<short, Arrow> a in arrows)
            {
                arrow = a.Value;
                arrow.Draw(spriteBatch);
                
                spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 430, 170, 170), new Color(0, 0, 0, 0.2f));
            }
        }

        public void UpdateArrow(short playerId, Vector2 position, Vector2 speed, float updateTime)
        {
            arrows[playerId].Position = position + (speed * updateTime);
            arrows[playerId].LastUpdateTime = updateTime;
        }
    }
}
