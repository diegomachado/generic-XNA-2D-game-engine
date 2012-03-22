using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            Player player = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(0, 240));

            this.players.Add(id, player);

            return player;
        }

        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
                this.players.Add(id, new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(0, 240)));
        }

        // TODO: Criar função que atualiza posições de cada um dos jogadores
        public void Update(Rectangle clientBounds)
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;

                acceleration = Vector2.Zero;

                if (player.state == PlayerState.WalkingLeft)
                    acceleration += new Vector2(-0.5f, 0.0f);

                if (player.state == PlayerState.WalkingRight)
                    acceleration += new Vector2(0.5f, 0.0f);

                if (player.state == PlayerState.Jumping)
                    if (player.position.Y == (clientBounds.Height - player.Height))
                        acceleration += new Vector2(0.0f, player.jumpForce);

                // TODO: Ajeitar colisão com o chão e testes se o jogador esta no chão ou não

                acceleration += new Vector2(0.0f, player.gravity);

                player.speed += acceleration;
                player.speed.X *= player.friction;

                player.position += player.speed;
                player.position = new Vector2(MathHelper.Clamp(player.position.X, 0, clientBounds.Width - player.Width),
                                              MathHelper.Clamp(player.position.Y, 0, clientBounds.Height - player.Height));

                if (player.position.Y == (clientBounds.Height - player.Height))
                {
                    if (player.state == PlayerState.Jumping)
                        player.state = PlayerState.Idle;

                    player.speed.Y = 0.0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (KeyValuePair<short, Player> player in players)
            {
                player.Value.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, player.Key.ToString(), new Vector2(player.Value.position.X + 8, player.Value.position.Y - 25), Color.White);
            }
        }
    }
}
