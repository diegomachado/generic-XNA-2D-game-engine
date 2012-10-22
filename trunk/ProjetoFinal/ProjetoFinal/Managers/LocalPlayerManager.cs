using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

using OgmoEditorLibrary;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;
using ProjetoFinal.EventHeaders;

namespace ProjetoFinal.Managers
{
    class LocalPlayerManager
    {
        EventManager eventManager = EventManager.Instance;
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
            localPlayer = new Player(id, new Vector2(70, 70));  
        }

        Grid grid;
        public void Update(GameTime gameTime)
        {
            if (localPlayer == null) return;

            if (localPlayer.IsDead)
            {
                Vector2 respawnPosition = LevelManager.Instance.currentLevel.GetRandomSpawnPoint();

                eventManager.ThrowPlayerRespawned(this, new PlayerRespawnedEventArgs(localPlayer.id, respawnPosition));

                localPlayer.Respawn(respawnPosition);
            }
            
            localPlayer.Update(gameTime);
            grid = LevelManager.Instance.Grid;
                  
            HandleHorizontalMovement();
            HandleVerticalMovement();
            HandleActions(gameTime);

            localPlayerHorizontalState = localPlayerHorizontalState.Update(playerId, gameTime, localPlayer, grid, localPlayerHorizontalStates);
            localPlayerVerticalState = localPlayerVerticalState.Update(playerId, gameTime, localPlayer, grid, localPlayerVerticalStates);
            localPlayerActionState = localPlayerActionState.Update(playerId, gameTime, localPlayer, localPlayerActionStates);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer == null) return;
            
            localPlayer.Draw(spriteBatch);
            localPlayer.DrawArrowPower(spriteBatch, shootingTimer, camera); 
        }        

        private void HandleVerticalMovement()
        {
            if (inputManager.Jump)
                localPlayerVerticalState = localPlayerVerticalState.Jumped(playerId, localPlayer, localPlayerVerticalStates);
        }

        private void HandleHorizontalMovement()
        {
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
        }

        private void HandleActions(GameTime gameTime)
        {
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
        }
    }
}