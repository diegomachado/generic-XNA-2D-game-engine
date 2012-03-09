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
        short playerIdCounter = 0;

        public PlayerManager()
        {
            players = new Dictionary<short,Player>();
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];
            else
                return null;
        }

        public Player AddPlayer(short id, Texture2D texture, Vector2 position)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];
            
            Player player = new Player(id, texture, position);

            this.players.Add(player.id, player);

            return player;
        }

        public Player AddPlayer(Texture2D texture)
        {
            return new Player(playerIdCounter++, texture, Vector2.Zero);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(KeyValuePair<short, Player> player in players)
                player.Value.Draw(spriteBatch);
        }

        // TODO: Criar função que atualiza posições de cada um dos jogadores
        public void Update()
        {
            foreach (KeyValuePair<short, Player> player in players)
                player.Value.Update();
        }
    }
}
