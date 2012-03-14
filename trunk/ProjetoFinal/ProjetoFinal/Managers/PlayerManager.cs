using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Managers
{
    class PlayerManager
    {
        Dictionary<short, Player> players;

        public PlayerManager()
        {
            players = new Dictionary<short,Player>();
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];

            Player player = new Player(TextureManager.Instance.getTexture(TextureList.Bear), Vector2.Zero);

            this.players.Add(id, player);

            return player;
        }

        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
                this.players.Add(id, new Player(TextureManager.Instance.getTexture(TextureList.Bear), Vector2.Zero));
        }

        // TODO: Criar função que atualiza posições de cada um dos jogadores
        public void Update()
        {
            foreach (KeyValuePair<short, Player> player in players)
                player.Value.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<short, Player> player in players)
                player.Value.Draw(spriteBatch);
        }
    }
}
