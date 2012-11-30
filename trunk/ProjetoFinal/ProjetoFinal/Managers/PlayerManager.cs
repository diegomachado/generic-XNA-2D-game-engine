using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using ProjetoFinal.Entities;
using ProjetoFinal.EventHeaders;
using Microsoft.Xna.Framework.Input;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.PlayerStateMachine.ActionStates;
using ProjetoFinal.PlayerStateMachine.MovementStates.HorizontalMovementStates;
using ProjetoFinal.PlayerStateMachine.MovementStates.VerticalMovementStates;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers
{
    class PlayerManager
    {
        public Dictionary<short, Player> players { get; private set; }

        LevelManager levelManager = LevelManager.Instance;
        Camera camera = Camera.Instance;

        public PlayerManager()
        {
            players = new Dictionary<short, Player>();
        }

        public void LoadContent(){}

        public void Update(GameTime gameTime, Grid grid)
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;
                short playerId = p.Key;

                player.Update(gameTime);

                player.HorizontalState = player.HorizontalState.Update(playerId, gameTime, player, levelManager.currentLevel.grid, PlayerStates.horizontalStates);
                player.VerticalState = player.VerticalState.Update(playerId, gameTime, player, levelManager.currentLevel.grid, PlayerStates.verticalStates);
                player.ActionState = player.ActionState.Update(playerId, gameTime, player, PlayerStates.actionStates);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Player player;

            foreach (KeyValuePair<short, Player> p in players)
            {
                player = p.Value;
                player.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, p.Key.ToString(), player.position - new Vector2(-13, 28) - camera.Position, Color.White);        
            }
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];

            Player player = new Player(id, new Vector2(240, 240));
            player.LoadContent();
            players.Add(id, player);

            return player;
        }

        public void AddPlayer(short id, Vector2 position)
        {
            if (!this.players.ContainsKey(id))
            {
                Player player = new Player(id, position);
                player.LoadContent();
                this.players.Add(id, player);
            }
        }

        public void AddPlayer(short id, int x, int y)
        {
            if (!this.players.ContainsKey(id))
            {
                Player player = new Player(id, new Vector2(x, y));
                player.LoadContent();
                this.players.Add(id, player);
            }
        }

        public void RemovePlayer(short id)
        {
            this.players.Remove(id);
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, SpriteBatch spriteBatch, Texture2D borderTexture)
        {
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White);  
            spriteBatch.Draw(borderTexture, new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); 
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White);   
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White);
        }

        public void UpdatePlayerState(short playerId, Vector2 position, double messageTime, UpdatePlayerStateType stateType, short playerState)
        {
            Player player = GetPlayer(playerId);

            if (player.lastUpdateTime < messageTime)
            {
                players[playerId].lastUpdateTime = messageTime;

                switch (stateType)
                {
                    case UpdatePlayerStateType.Action:
                        player.ActionState = PlayerStates.actionStates[(ActionStateType)playerState];
                        player.ActionStateType = (ActionStateType)playerState;
                        break;
                }
            }
        }

        public void UpdatePlayerMovementState(short playerId, Vector2 position, Vector2 speed, double messageTime, UpdatePlayerStateType stateType, short playerState)
        {
            Player player = GetPlayer(playerId);

            if (player.lastUpdateTime < messageTime)
            {
                // TODO: esse codigo tem que subir, não eh pra ter NetTime.Now aqui
                //float timeDelay = (float)(NetTime.Now - messageTime);

                // TODO: Rever conta pro Lag compensation ficar certinho
                // Lag Compensation

                players[playerId].position = position;// + (speed * timeDelay); // TODO: Usar velocidade local ou da rede?
                players[playerId].lastUpdateTime = messageTime;
                players[playerId].speed = speed;

                switch (stateType)
                {
                    case UpdatePlayerStateType.Horizontal:

                        player.HorizontalState = PlayerStates.horizontalStates[(HorizontalStateType)playerState];
                        player.HorizontalStateType = (HorizontalStateType)playerState;
                        break;

                    case UpdatePlayerStateType.Vertical:

                        player.VerticalState = PlayerStates.verticalStates[(VerticalStateType)playerState];
                        player.VerticalStateType = (VerticalStateType)playerState;
                        break;
                }
            }
            else
            {
                Console.WriteLine("This can't happen many times in a row!!! FUCK FUCKLY FUCK FUCK FUCK");
            }
        }
    }
}
