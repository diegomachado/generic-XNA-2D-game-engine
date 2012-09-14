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
        Player localPlayer;
        short localPlayerId;
        float arrowLifeSpan = 3.0f;

        public ArrowManager(short localPlayerId, Player localPlayer)
        {
            this.arrows = new List<Arrow>();
            this.localPlayer = localPlayer;
            this.localPlayerId = localPlayerId;
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            foreach (Arrow arrow in arrows)
                arrow.Update(gameTime);

            /*float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            List<Arrow> toRemove = new List<Arrow>();

            foreach (Arrow arrow in arrows)
            {
                if (arrow.Collided)
                {
                    arrow.Timer += elapsedTime;

                    if (arrow.Timer > arrowLifeSpan)
                    {
                        toRemove.Add(arrow);
                        Entity.Entities.Remove(arrow);
                    }
                }
                else
                {
                    arrow.speed.Y += arrow.gravity * Arrow.gravityFactor; //TODO: * elapsedTime;

                    // TODO: Ta meio muquirana ainda, da pra melhorar mas funciona
                    // TODO: Girar collisionBox das flechas?

                    Rectangle collisionBox = arrow.CollisionBox;
                    collisionBox.Offset((int)(arrow.speed.X * elapsedTime), (int)(arrow.speed.Y * elapsedTime));

                    if (arrow.OwnerId != localPlayerId) // Check collision with localPlayer
                    {
                        foreach (Player player in new List<Player>())
                        {
                            if (collisionBox.Intersects(localPlayer.CollisionBox))
                            {
                                toRemove.Add(arrow);

                                // TODO: Lançar evento de hit ou se comunicar diretamente com o PlayerManager ou através de Player localPlayer, ainda não sei, pensar com bombado
                            }
                        }
                    }
                    else if (checkCollision(collisionBox, collisionLayer)) // Check collision with ground
                    {
                        arrow.Collided = true;
                    }
                    else
                    {
                        arrow.position += arrow.speed * elapsedTime;
                    }
                }
            }

            foreach (Arrow arrow in toRemove)
            {
                arrows.Remove(arrow);
            }*/
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
