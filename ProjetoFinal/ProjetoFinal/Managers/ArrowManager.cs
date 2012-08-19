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

        public ArrowManager()
        {
            arrows = new List<Arrow>();

            eventManager.ArrowShot += OnArrowShot;
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            List<Arrow> toRemove = new List<Arrow>();

            foreach (Arrow arrow in arrows)
            {
                arrow.Speed += (arrow.Gravity / 200);
                arrow.Position += arrow.Speed;

                Rectangle collisionBox = arrow.CollisionBox;
                collisionBox.Offset((int)arrow.SpeedX, (int)arrow.SpeedY);

                // TODO: Ta meio muquirana ainda, da pra melhorar mas funciona
                if (checkCollision(collisionBox, collisionLayer))
                    toRemove.Add(arrow);
            }

            foreach (Arrow arrow in toRemove)
                arrows.Remove(arrow);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(spriteBatch, camera);
                
                // TODO: Que porra eh essa que tu pos aqui bomber?
                //spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 430, 170, 170), new Color(0, 0, 0, 0.2f));
            }
        }

        private void OnArrowShot(object sender, ArrowShotEventArgs arrowShotEventArgs)
        {
            arrows.Add(new Arrow(arrowShotEventArgs.playerId, arrowShotEventArgs.position, arrowShotEventArgs.speed));
        }

        // TODO: Meio acoxambrado, talvez de pra fazer melhor
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
