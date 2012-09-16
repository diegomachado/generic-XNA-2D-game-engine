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
        Camera camera = Camera.Instance;
        EventManager eventManager = EventManager.Instance;
        List<Arrow> arrows;
        List<Arrow> toRemove;
        Player localPlayer;
        short localPlayerId;

        public ArrowManager(short localPlayerId, Player localPlayer)
        {
            this.arrows = new List<Arrow>();
            this.toRemove = new List<Arrow>();
            this.localPlayer = localPlayer;
            this.localPlayerId = localPlayerId;
        }

        //TODO: Fazer Pooling de Arrows, dentro da classe Arrow
        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            for (int i = 0; i < arrows.Count; i++)
            { 
                arrows[i].Update(gameTime);

                if (arrows[i].Collided)
                {
                    if (arrows[i].lifeSpan > 4000)
                    { 
                        toRemove.Add(arrows[i]);
                        Entity.Entities.Remove(arrows[i]);
                    }
                }
                else
                {
                    if (arrows[i].OwnerId != localPlayerId)
                    {
                        foreach (Player player in new List<Player>())
                        {
                            if (arrows[i].Collides(localPlayer.CollisionBox))
                            {
                                toRemove.Add(arrows[i]);
                                Console.WriteLine("Arrow collided with Player!");
                                // TODO: Lançar evento de hit ou se comunicar diretamente com o PlayerManager ou através de Player localPlayer, ainda não sei, pensar com bombado
                            }
                        }
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
