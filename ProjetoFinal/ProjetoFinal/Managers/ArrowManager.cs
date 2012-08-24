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

            eventManager.PlayerStateChangedWithArrow += OnArrowShot;
        }

        // TODO: Colocar lógicas de Update que independem do Manager dentro do Update de Arrow
        // Ou seja, encapsular as coisas :)
        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            List<Arrow> toRemove = new List<Arrow>();

            foreach (Arrow arrow in arrows)
            {
                if (arrow.Collided)
                {
                    arrow.Timer += elapsedTime;

                    if (arrow.Timer > arrowLifeSpan)
                        toRemove.Add(arrow);
                }
                else
                {
                    arrow.Speed += arrow.Gravity * Arrow.gravityFactor; //TODO: * elapsedTime;

                    // TODO: Ta meio muquirana ainda, da pra melhorar mas funciona
                    // TODO: Girar collisionBox das flechas?

                    Rectangle collisionBox = arrow.CollisionBox;
                    collisionBox.Offset((int)(arrow.SpeedX * elapsedTime), (int)(arrow.SpeedY * elapsedTime));

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
                        arrow.Position += arrow.Speed * elapsedTime;
                    }
                }
            }

            foreach (Arrow arrow in toRemove)
            {
                arrows.Remove(arrow);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(spriteBatch, camera);

                //Util.DrawRectangle(arrow.CollisionBox, 1, Color.Red, spriteBatch, camera);

                // TODO: Que porra eh essa que tu pos aqui bomber?
                //spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 430, 170, 170), new Color(0, 0, 0, 0.2f));
            }
        }

        private void OnArrowShot(object sender, PlayerStateChangedWithArrowEventArgs playerStateChangedWithArrowEventArgs)
        {
            arrows.Add(new Arrow(playerStateChangedWithArrowEventArgs.PlayerId, playerStateChangedWithArrowEventArgs.Player.Position, playerStateChangedWithArrowEventArgs.ShotSpeed));
        }

        // TODO: Meio acoxambrado, da pra fazer melhor
        private bool checkCollision(Rectangle collisionBox, Layer collisionLayer)
        {
            Point corner1 = new Point(collisionBox.Left, collisionBox.Top);
            Point corner2 = new Point(collisionBox.Right, collisionBox.Top);
            Point corner3 = new Point(collisionBox.Left, collisionBox.Bottom);
            Point corner4 = new Point(collisionBox.Right, collisionBox.Bottom);

            return (collisionLayer.GetTileValueByPixelPosition(corner1) ||
                    collisionLayer.GetTileValueByPixelPosition(corner2) ||
                    collisionLayer.GetTileValueByPixelPosition(corner3) ||
                    collisionLayer.GetTileValueByPixelPosition(corner4));
        }
    }
}
