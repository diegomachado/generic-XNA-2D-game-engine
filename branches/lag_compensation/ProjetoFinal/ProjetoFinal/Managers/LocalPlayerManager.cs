using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;

namespace ProjetoFinal.Managers
{
    class LocalPlayerManager
    {
        Player localPlayer;
        float shootingTimer;

        HorizontalMovementState localPlayerHorizontalState;
        VerticalMovementState localPlayerVerticalState;
        ActionState localPlayerActionState;
        Dictionary<HorizontalStateType, HorizontalMovementState> localPlayerHorizontalStates = new Dictionary<HorizontalStateType, HorizontalMovementState>();
        Dictionary<VerticalStateType, VerticalMovementState> localPlayerVerticalStates = new Dictionary<VerticalStateType, VerticalMovementState>();
        Dictionary<ActionStateType, ActionState> localPlayerActionStates = new Dictionary<ActionStateType, ActionState>();

        public short playerId { get; set; }
        public Player LocalPlayer { get { return localPlayer; } }

        InputManager inputManager;
        Camera camera;

        public LocalPlayerManager()
        {
            localPlayerHorizontalStates[HorizontalStateType.Idle] = new HorizontalIdleState();
            localPlayerHorizontalStates[HorizontalStateType.WalkingLeft] = new WalkingLeftState();
            localPlayerHorizontalStates[HorizontalStateType.WalkingRight] = new WalkingRightState();
            localPlayerHorizontalStates[HorizontalStateType.StoppingWalkingLeft] = new StoppingWalkingLeftState();
            localPlayerHorizontalStates[HorizontalStateType.StoppingWalkingRight] = new StoppingWalkingRightState();
            localPlayerHorizontalState = localPlayerHorizontalStates[HorizontalStateType.Idle];

            localPlayerVerticalStates[VerticalStateType.Idle] = new VerticalIdleState();
            localPlayerVerticalStates[VerticalStateType.Jumping] = new JumpingState();
            localPlayerVerticalStates[VerticalStateType.StartedJumping] = new StartedJumpingState();
            localPlayerVerticalState = localPlayerVerticalStates[VerticalStateType.Jumping];

            localPlayerActionStates[ActionStateType.Idle] = new ActionIdleState();
            localPlayerActionStates[ActionStateType.Attacking] = new AttackingState();
            localPlayerActionStates[ActionStateType.Defending] = new DefendingState();
            localPlayerActionStates[ActionStateType.Shooting] = new ShootingState();
            localPlayerActionStates[ActionStateType.PreparingShot] = new PreparingShotState();
            localPlayerActionState = localPlayerActionStates[ActionStateType.Idle];

            inputManager = InputManager.Instance;
            camera = Camera.Instance;
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(new Vector2(240, 40));  
        }

        Layer collisionLayer;
        public void Update(GameTime gameTime)
        {
            if (localPlayer == null)
                return;

            collisionLayer = MapManager.Instance.CollisionLayer;
                  
            #region Horizontal Movement
            if (inputManager.Left)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.MovedLeft(playerId, localPlayer, localPlayerHorizontalStates);
            }
            else if (inputManager.PreviouslyLeft)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.StoppedMovingLeft(playerId, localPlayer, localPlayerHorizontalStates);
            }

            if (inputManager.Right)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.MovedRight(playerId, localPlayer, localPlayerHorizontalStates);
            }
            else if (inputManager.PreviouslyRight)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.StoppedMovingRight(playerId, localPlayer, localPlayerHorizontalStates);
            }
            #endregion

            #region Vertical Movement
            if (inputManager.Jump)
            {
                localPlayerVerticalState = localPlayerVerticalState.Jumped(playerId, localPlayer, localPlayerVerticalStates);
            }
            #endregion

            #region Actions
            if (inputManager.PreparingShot)
            {
                localPlayerActionState = localPlayerActionState.PreparingShot(playerId, localPlayer, localPlayerActionStates);
                shootingTimer += gameTime.ElapsedGameTime.Milliseconds;
                shootingTimer = MathHelper.Clamp(shootingTimer, 0, 1000);
            }
            else
            {
                localPlayerActionState = localPlayerActionState.ShotReleased(playerId, localPlayer, shootingTimer, camera.CameraToWorld(inputManager.MousePosition), localPlayerActionStates);
                shootingTimer = 0f;
            }
            #endregion

            localPlayerHorizontalState = localPlayerHorizontalState.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerHorizontalStates);
            localPlayerVerticalState = localPlayerVerticalState.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerVerticalStates);
            localPlayerActionState = localPlayerActionState.Update(playerId, gameTime, localPlayer, localPlayerActionStates);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
                localPlayer.Draw(spriteBatch);

            // TODO: Colocar dentro de Arrow
            // TODO: Ajeitar a barrinha de charge pra crescer pra cima :$
            if (shootingTimer != 0)
                if(localPlayer.FacingRight)
                    spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(), new Rectangle((int)localPlayer.position.X - 5 - (int)camera.Position.X, (int)localPlayer.position.Y + localPlayer.Height - (int)camera.Position.Y, 5, (int)shootingTimer / 50), Color.Yellow);
                else
                    spriteBatch.Draw(TextureManager.Instance.GetPixelTexture(), new Rectangle((int)localPlayer.position.X + 30 - (int)camera.Position.X, (int)localPlayer.position.Y + localPlayer.Height - (int)camera.Position.Y, 5, (int)shootingTimer / 50), Color.Yellow);

            //spriteBatch.Draw(TextureManager.Instance.GetPixelTextureByColor(Color.Black), new Rectangle(0, 0, 230, 170), new Color(0, 0, 0, 0.2f));
            //spriteBatch.DrawString(spriteFont, "" + localPlayerHorizontalState, new Vector2(localPlayer.position.X + 8, localPlayer.position.Y - 20) - camera.Position, Color.White);
            //spriteBatch.DrawString(spriteFont, "" + localPlayerVerticalState, new Vector2(localPlayer.position.X + 8, localPlayer.position.Y - 40) - camera.Position, Color.White);
            //spriteBatch.DrawString(spriteFont, "X: " + (int)localPlayer.position.X, new Vector2(5f, 05f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Y: " + (int)localPlayer.position.Y, new Vector2(5f, 25f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)localPlayer.speed.X, new Vector2(5f, 45f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)localPlayer.speed.Y, new Vector2(5f, 65f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)camera.Position.X, new Vector2(5f, 85f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)camera.Position.Y, new Vector2(5f, 105f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Horizontal State: " + localPlayerHorizontalState, new Vector2(5f, 125f), Color.White);
            //spriteBatch.DrawString(spriteFont, "Vertical State: " + localPlayerVerticalState, new Vector2(5f, 145f), Color.White);
        
        }
    }
}