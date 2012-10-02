using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lidgren.Network;

using OgmoLibrary;

using ProjetoFinal.Entities;
using ProjetoFinal.EventHeaders;
using ProjetoFinal.Managers.LocalPlayerStates;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers
{
    class ArrowManager
    {
        Camera camera = Camera.Instance;
        EventManager eventManager = EventManager.Instance;
        NetworkManager networkManager = NetworkManager.Instance;
        List<Arrow> arrows;
        List<Arrow> toRemove;
        Player localPlayer;

        public ArrowManager(Player localPlayer)
        {
            this.arrows = new List<Arrow>();
            this.toRemove = new List<Arrow>();
            this.localPlayer = localPlayer;
        }

        //TODO: Fazer Pooling de Arrows, dentro da classe Arrow
        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            for (int i = 0; i < arrows.Count; i++)
            { 
                arrows[i].Update(gameTime);

                if (arrows[i].Collided)
                {
                    if (arrows[i].lifeSpan <= 0)
                    { 
                        toRemove.Add(arrows[i]);
                        Entity.Entities.Remove(arrows[i]);
                    }
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
                arrows.Remove(toRemove[i]);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (Arrow arrow in arrows)
                arrow.Draw(spriteBatch);
        }

        public void CreateArrow(short playerId, Vector2 position, Vector2 speed)
        {
            arrows.Add(new Arrow(playerId, position, speed));
        }
    }
}
