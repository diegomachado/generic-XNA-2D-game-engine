using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using ProjetoFinal.Entities;
using ProjetoFinal.EventArgs;
using ProjetoFinal.Managers.LocalPlayerStates;
using Microsoft.Xna.Framework.Input;
using OgmoLibrary;

namespace ProjetoFinal.Managers
{
    class PlayerManager
    {
        Dictionary<short, Player> players;
        Dictionary <short, PlayerState> playerState;
        Dictionary<PlayerStateType, PlayerState> playerStates;

        public PlayerManager()
        {
            players = new Dictionary<short, Player>();
            playerState = new Dictionary<short, PlayerState>();
            playerStates = new Dictionary<PlayerStateType, PlayerState>();

            playerStates[PlayerStateType.Idle] = new IdleState(false);
            playerStates[PlayerStateType.JumpingStraight] = new JumpingStraightState(false);
            playerStates[PlayerStateType.JumpingLeft] = new JumpingLeftState(false);
            playerStates[PlayerStateType.JumpingRight] = new JumpingRightState(false);
            playerStates[PlayerStateType.WalkingLeft] = new WalkingLeftState(false);
            playerStates[PlayerStateType.WalkingRight] = new WalkingRightState(false);
            playerStates[PlayerStateType.StoppingJumpingLeft] = new StoppingJumpingLeftState(false);
            playerStates[PlayerStateType.StoppingJumpingRight] = new StoppingJumpingRightState(false);
            playerStates[PlayerStateType.StoppingWalkingLeft] = new StoppingWalkingLeftState(false);
            playerStates[PlayerStateType.StoppingWalkingRight] = new StoppingWalkingRightState(false);
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];

            Player player = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(240, 240), new Rectangle(5, 1, 24, 30));

            players.Add(id, player);
            playerState.Add(id, playerStates[PlayerStateType.JumpingStraight]);

            return player;
        }

        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
            {
                this.players.Add(id, new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(240, 40), new Rectangle(5, 1, 24, 30)));
                playerState.Add(id, playerStates[PlayerStateType.JumpingStraight]);
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Layer collisionLayer)
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;
                short playerId = p.Key;

                playerState[playerId] = playerState[playerId].Update(playerId, gameTime, player, collisionLayer, playerStates);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Player player;

            foreach (KeyValuePair<short, Player> p in players)
            {
                player = p.Value;
                player.Draw(spriteBatch);
                
                spriteBatch.DrawString(spriteFont, player.State.ToString(), new Vector2(player.Position.X + 8, player.Position.Y - 25) - Camera.Instance.Position, Color.White);

                spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 430, 170, 170), new Color(0, 0, 0, 0.2f));

                spriteBatch.DrawString(spriteFont, "OnGround: " + player.OnGround.ToString(), new Vector2(5f, 435f), Color.White);
                spriteBatch.DrawString(spriteFont, "X: " + (int)player.Position.X, new Vector2(5f, 455f), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + (int)player.Position.Y, new Vector2(5f, 475f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)player.Speed.X, new Vector2(5f, 495f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)player.Speed.Y, new Vector2(5f, 515f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)Camera.Instance.Position.X, new Vector2(5f, 535f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)Camera.Instance.Position.Y, new Vector2(5f, 555f), Color.White);
                spriteBatch.DrawString(spriteFont, "State: " + player.State, new Vector2(5f, 575f), Color.White);
            }
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, SpriteBatch spriteBatch, Texture2D borderTexture)
        {
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White);  
            spriteBatch.Draw(borderTexture, new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); 
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White);   
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White);
        }

        public void UpdatePlayer(short playerId, PlayerStateType playerStateType, Vector2 position, double updateTime)
        {
            players[playerId].Position = position;
            players[playerId].State = playerStateType;
            players[playerId].LastUpdateTime = updateTime;

            playerState[playerId] = playerStates[playerStateType];
        }
    }
}
