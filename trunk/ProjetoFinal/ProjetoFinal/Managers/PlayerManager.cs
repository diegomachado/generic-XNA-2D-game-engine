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
    class PlayerManager
    {
        Camera camera = Camera.Instance;
        Dictionary<short, Player> players;
        Dictionary <short, HorizontalMovementState> horizontalPlayerState;
        Dictionary<HorizontalStateType, HorizontalMovementState> horizontalPlayerStates;
        Dictionary<short, VerticalMovementState> verticalPlayerState;
        Dictionary<VerticalStateType, VerticalMovementState> verticalPlayerStates;

        public PlayerManager()
        {
            players = new Dictionary<short, Player>();
            horizontalPlayerState = new Dictionary<short, HorizontalMovementState>();
            horizontalPlayerStates = new Dictionary<HorizontalStateType, HorizontalMovementState>();
            verticalPlayerState = new Dictionary<short, VerticalMovementState> ();
            verticalPlayerStates = new Dictionary<VerticalStateType, VerticalMovementState>();

            horizontalPlayerStates[HorizontalStateType.Idle] = new HorizontalIdleState();
            horizontalPlayerStates[HorizontalStateType.StoppingWalkingLeft] = new StoppingWalkingLeftState();
            horizontalPlayerStates[HorizontalStateType.StoppingWalkingRight] = new StoppingWalkingRightState();
            horizontalPlayerStates[HorizontalStateType.WalkingLeft] = new WalkingLeftState();
            horizontalPlayerStates[HorizontalStateType.WalkingRight] = new WalkingRightState();
            verticalPlayerStates[VerticalStateType.Idle] = new VerticalIdleState();
            verticalPlayerStates[VerticalStateType.Jumping] = new JumpingState();
            verticalPlayerStates[VerticalStateType.StartedJumping] = new StartedJumpingState();
        }

        public Player GetPlayer(short id)
        {
            if (this.players.ContainsKey(id))
                return this.players[id];

            Player player = new Player(new Vector2(240, 240));
            player.LoadContent();
            players.Add(id, player);
            horizontalPlayerState.Add(id, horizontalPlayerStates[HorizontalStateType.Idle]);
            verticalPlayerState.Add(id, verticalPlayerStates[VerticalStateType.Jumping]);

            return player;
        }

        Player player;
        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
            {
                player = new Player(new Vector2(240, 40));
                player.LoadContent();
                this.players.Add(id, player);
                horizontalPlayerState.Add(id, horizontalPlayerStates[HorizontalStateType.Idle]);
                verticalPlayerState.Add(id, verticalPlayerStates[VerticalStateType.Jumping]);
            }
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;
                short playerId = p.Key;

                horizontalPlayerState[playerId] = horizontalPlayerState[playerId].Update(playerId, gameTime, player, collisionLayer, horizontalPlayerStates);
                verticalPlayerState[playerId] = verticalPlayerState[playerId].Update(playerId, gameTime, player, collisionLayer, verticalPlayerStates);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Player player;

            foreach (KeyValuePair<short, Player> p in players)
            {
                player = p.Value;
                player.Draw(spriteBatch);
                
                //spriteBatch.DrawString(spriteFont, player.LastState.ToString(), new Vector2(player.Position.X + 8, player.Position.Y - 25) - Camera.Instance.Position, Color.White);

                spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(), new Rectangle(0, 430, 170, 170), new Color(0, 0, 0, 0.2f));
                spriteBatch.DrawString(spriteFont, "X: " + (int)player.position.X, new Vector2(5f, 455f), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + (int)player.position.Y, new Vector2(5f, 475f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)player.speed.X, new Vector2(5f, 495f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)player.speed.Y, new Vector2(5f, 515f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)Camera.Instance.Position.X, new Vector2(5f, 535f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)Camera.Instance.Position.Y, new Vector2(5f, 555f), Color.White);
                //spriteBatch.DrawString(spriteFont, "State: " + player.LastState, new Vector2(5f, 575f), Color.White);
            }
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, SpriteBatch spriteBatch, Texture2D borderTexture)
        {
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White);  
            spriteBatch.Draw(borderTexture, new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); 
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White);   
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White);
        }

        public void UpdatePlayer(short playerId, Vector2 position, Vector2 speed, double messageTime, UpdatePlayerStateType updatePlayerStateMessageType, short playerState)
        {
            Player player = GetPlayer(playerId);

            // TODO: Dropar mensagens fora de ordem ou não????
            //if (player.LastUpdateTime < messageTime)
            //{
                // TODO: esse codigo tem que subir, não eh pra ter NetTime.Now aqui
                float timeDelay = (float)(NetTime.Now - messageTime);

                // TODO: Rever conta pro Lag compensation ficar certinho
                // Lag Compensation

                players[playerId].position = position + (speed * timeDelay); // TODO: Usar velocidade local ou da rede?
                players[playerId].LastUpdateTime = messageTime;

                switch (updatePlayerStateMessageType)
                {
                    case UpdatePlayerStateType.Horizontal:
                        horizontalPlayerState[playerId] = horizontalPlayerStates[(HorizontalStateType)playerState];

                        if ((HorizontalStateType)playerState == HorizontalStateType.WalkingLeft ||
                            (HorizontalStateType)playerState == HorizontalStateType.WalkingRight)
                        {
                            players[playerId].speed.X = 0;
                        }
                        break;
                    case UpdatePlayerStateType.Vertical:
                        verticalPlayerState[playerId] = verticalPlayerStates[(VerticalStateType)playerState];

                        if ((VerticalStateType)playerState == VerticalStateType.StartedJumping)
                        {
                            players[playerId].speed.Y = 0;
                        }
                        break;
                }
            //}
        }
    }
}
