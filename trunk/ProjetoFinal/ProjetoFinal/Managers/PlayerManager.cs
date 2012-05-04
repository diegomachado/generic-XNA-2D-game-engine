using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using ProjetoFinal.Entities;
using ProjetoFinal.EventArgs;

namespace ProjetoFinal.Managers
{
    class PlayerManager
    {
        Dictionary<short, Player> players;
        Vector2 acceleration;

        public PlayerManager()
        {
            players = new Dictionary<short,Player>();
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];

            Player player = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(0, 240), new Rectangle(6, 2, 24, 30));

            this.players.Add(id, player);

            return player;
        }

        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
                this.players.Add(id, new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(0, 240), new Rectangle(6, 2, 24, 30)));
        }

        // TODO: Criar função que atualiza posições de cada um dos jogadores
        public void Update()
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;

                player.CollisionBox.X = (int)player.Position.X + player.BoundingBox.X;
                player.CollisionBox.Y = (int)player.Position.Y + player.BoundingBox.Y;

                acceleration = Vector2.Zero;

                switch (player.State)
                {
                    case PlayerState.WalkingLeft:
                        acceleration += new Vector2(-0.5f, 0.0f);
                        break;

                    case PlayerState.WalkingRight:
                        acceleration += new Vector2(0.5f, 0.0f);
                        break;

                    case PlayerState.JumpingRight:
                        acceleration += player.JumpForce;
                        acceleration += new Vector2(0.5f, 0.0f);

                        player.State = PlayerState.WalkingRight;
                        break;

                    case PlayerState.JumpingLeft:
                        acceleration += player.JumpForce;
                        acceleration += new Vector2(-0.5f, 0.0f);

                        player.State = PlayerState.WalkingLeft;
                        break;

                    case PlayerState.Jumping:
                        acceleration += player.JumpForce;
                        break;
                }

                // TODO: Ajeitar colisão com o chão e testes se o jogador esta no chão ou não

                acceleration += player.Gravity;

                player.speed += acceleration;
                player.speed.X *= player.Friction;

                player.Position += player.speed;

                if (player.State == PlayerState.Jumping)
                    player.State = PlayerState.Idle;

                player.speed.Y = 0.0f;                
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (KeyValuePair<short, Player> player in players)
            {
                player.Value.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, player.Key.ToString(), new Vector2(player.Value.Position.X + 8, player.Value.Position.Y - 25), Color.White);
                DrawBoundingBox(player.Value.CollisionBox, 1, spriteBatch, TextureManager.Instance.getPixelTextureByColor(Color.Red));
            }
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, SpriteBatch spriteBatch, Texture2D borderTexture)
        {
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White);  
            spriteBatch.Draw(borderTexture, new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); 
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White);   
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White);
        }
    }
}
