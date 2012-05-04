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
        Vector2 acceleration = Vector2.Zero;
        Vector2 moveAmount;

        public PlayerManager()
        {
            players = new Dictionary<short,Player>();
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];

            Player player = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(96, 240), new Rectangle(6, 2, 24, 30));

            this.players.Add(id, player);

            return player;
        }

        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
                this.players.Add(id, new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(96, 240), new Rectangle(6, 2, 24, 30)));
        }

        public void Update()
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;
                acceleration = Vector2.Zero;

                player.CollisionBox.X = (int)player.Position.X + player.BoundingBox.X;
                player.CollisionBox.Y = (int)player.Position.Y + player.BoundingBox.Y;

                switch (player.State)
                {
                    case PlayerState.WalkingLeft:
                        acceleration -= player.Speed;
                        break;

                    case PlayerState.WalkingRight:
                        acceleration += player.Speed;
                        break;

                    case PlayerState.Jumping:
                        acceleration += player.JumpForce;
                        break;
                    
                    case PlayerState.JumpingLeft:
                        acceleration += player.JumpForce;
                        acceleration -= player.Speed;
                        player.State = PlayerState.WalkingLeft;

                        break;

                    case PlayerState.JumpingRight:                        
                        acceleration += player.JumpForce;
                        acceleration += player.Speed;
                        player.State = PlayerState.WalkingRight;
                        break;                    
                }

                acceleration += player.Gravity;
                moveAmount += acceleration;
                moveAmount.X *= player.Friction;
                
                player.Position += moveAmount;
                Console.WriteLine(player.Position.ToString());
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (KeyValuePair<short, Player> player in players)
            {
                DrawBoundingBox(player.Value.CollisionBox, 1, spriteBatch, TextureManager.Instance.getPixelTextureByColor(Color.Red));
                player.Value.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, player.Key.ToString(), new Vector2(player.Value.Position.X + 8, player.Value.Position.Y - 25), Color.White);                
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
