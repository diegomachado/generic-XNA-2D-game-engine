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
using OgmoEditorLibrary;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers
{
    class PlayerManager
    {
        Camera camera = Camera.Instance;
        Dictionary<short, Player> players;
        Dictionary<short, ActionState> actionPlayerState;
        Dictionary<ActionStateType, ActionState> actionPlayerStates;
        Dictionary <short, HorizontalMovementState> horizontalPlayerState;
        Dictionary<HorizontalStateType, HorizontalMovementState> horizontalPlayerStates;
        Dictionary<short, VerticalMovementState> verticalPlayerState;
        Dictionary<VerticalStateType, VerticalMovementState> verticalPlayerStates;

        public PlayerManager()
        {
            players = new Dictionary<short, Player>();

            actionPlayerState = new Dictionary<short, ActionState>();
            actionPlayerStates = new Dictionary<ActionStateType, ActionState>();
            horizontalPlayerState = new Dictionary<short, HorizontalMovementState>();
            horizontalPlayerStates = new Dictionary<HorizontalStateType, HorizontalMovementState>();
            verticalPlayerState = new Dictionary<short, VerticalMovementState> ();
            verticalPlayerStates = new Dictionary<VerticalStateType, VerticalMovementState>();

            actionPlayerStates[ActionStateType.Attacking] = new AttackingState();
            actionPlayerStates[ActionStateType.Defending] = new DefendingState();
            actionPlayerStates[ActionStateType.Idle] = new ActionIdleState();
            actionPlayerStates[ActionStateType.PreparingShot] = new PreparingShotState();
            actionPlayerStates[ActionStateType.Shooting] = new ShootingState();
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

            Player player = new Player(id, new Vector2(240, 240));
            player.LoadContent();
            players.Add(id, player);
            actionPlayerState.Add(id, actionPlayerStates[ActionStateType.Idle]);
            horizontalPlayerState.Add(id, horizontalPlayerStates[HorizontalStateType.Idle]);
            verticalPlayerState.Add(id, verticalPlayerStates[VerticalStateType.Jumping]);

            return player;
        }

        Player player;
        public void AddPlayer(short id)
        {
            if (!this.players.ContainsKey(id))
            {
                player = new Player(id, new Vector2(240, 40));
                player.LoadContent();
                this.players.Add(id, player);
                actionPlayerState.Add(id, actionPlayerStates[ActionStateType.Idle]);
                horizontalPlayerState.Add(id, horizontalPlayerStates[HorizontalStateType.Idle]);
                verticalPlayerState.Add(id, verticalPlayerStates[VerticalStateType.Jumping]);
            }
        }

        public void Update(GameTime gameTime, Grid grid)
        {
            foreach (KeyValuePair<short, Player> p in players)
            {
                Player player = p.Value;
                short playerId = p.Key;

                actionPlayerState[playerId] = actionPlayerState[playerId].Update(playerId, gameTime, player, actionPlayerStates);
                horizontalPlayerState[playerId] = horizontalPlayerState[playerId].Update(playerId, gameTime, player, grid, horizontalPlayerStates);
                verticalPlayerState[playerId] = verticalPlayerState[playerId].Update(playerId, gameTime, player, grid, verticalPlayerStates);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Player player;

            foreach (KeyValuePair<short, Player> p in players)
            {
                player = p.Value;
                player.Draw(spriteBatch);
            }
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

            if (player.LastUpdateTime < messageTime)
            {
                players[playerId].LastUpdateTime = messageTime;

                switch (stateType)
                {
                    case UpdatePlayerStateType.Action:

                        actionPlayerState[playerId] = actionPlayerStates[(ActionStateType)playerState];

                        break;
                }
            }
        }

        public void UpdatePlayerMovementState(short playerId, Vector2 position, Vector2 speed, double messageTime, UpdatePlayerStateType stateType, short playerState)
        {
            Player player = GetPlayer(playerId);

            if (player.LastUpdateTime < messageTime)
            {
                // TODO: esse codigo tem que subir, não eh pra ter NetTime.Now aqui
                //float timeDelay = (float)(NetTime.Now - messageTime);

                // TODO: Rever conta pro Lag compensation ficar certinho
                // Lag Compensation

                players[playerId].position = position;// + (speed * timeDelay); // TODO: Usar velocidade local ou da rede?
                players[playerId].LastUpdateTime = messageTime;
                players[playerId].speed = speed;

                switch (stateType)
                {
                    case UpdatePlayerStateType.Horizontal:

                        horizontalPlayerState[playerId] = horizontalPlayerStates[(HorizontalStateType)playerState];

                        /*if ((HorizontalStateType)playerState == HorizontalStateType.WalkingLeft ||
                            (HorizontalStateType)playerState == HorizontalStateType.WalkingRight)
                        {
                            players[playerId].speed.X = 0;
                        }*/

                        break;

                    case UpdatePlayerStateType.Vertical:

                        verticalPlayerState[playerId] = verticalPlayerStates[(VerticalStateType)playerState];

                        /*if ((VerticalStateType)playerState == VerticalStateType.StartedJumping)
                        {
                            players[playerId].speed.Y = 0;
                        }*/

                        break;
                }
            }
        }
    }
}
