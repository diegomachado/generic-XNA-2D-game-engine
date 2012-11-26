using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;

using OgmoEditorLibrary;
using ProjetoFinal.EventHeaders;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers
{
    class LocalPlayerManager
    {
        public short playerId;
        public Player localPlayer;
        float shootingTimer;

        LevelManager levelManager = LevelManager.Instance;
        EventManager eventManager = EventManager.Instance;
        InputManager inputManager = InputManager.Instance;
        Camera camera = Camera.Instance;

        public LocalPlayerManager()
        {
            inputManager = InputManager.Instance;
            camera = Camera.Instance;
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;

            Vector2 spawnPosition = levelManager.currentLevel.GetRandomSpawnPoint();

            eventManager.ThrowPlayerCreated(this, new PlayerSpawnedEventArgs(id, spawnPosition));

            localPlayer = new Player(id, spawnPosition);
        }

        public void Update(GameTime gameTime)
        {
            if (localPlayer == null) return;

            if (localPlayer.IsDead)
            {
                Vector2 respawnPosition = levelManager.currentLevel.GetRandomSpawnPoint();

                eventManager.ThrowPlayerSpawned(this, new PlayerSpawnedEventArgs(localPlayer.id, respawnPosition));

                localPlayer.Respawn(respawnPosition);
            }
            
            localPlayer.Update(gameTime);
                  
            HandleHorizontalMovement();
            HandleVerticalMovement();
            HandleActions(gameTime);

            localPlayer.HorizontalState = localPlayer.HorizontalState.Update(playerId, gameTime, localPlayer, levelManager.currentLevel.grid, PlayerStates.horizontalStates);
            localPlayer.VerticalState = localPlayer.VerticalState.Update(playerId, gameTime, localPlayer, levelManager.currentLevel.grid, PlayerStates.verticalStates);
            localPlayer.ActionState = localPlayer.ActionState.Update(playerId, gameTime, localPlayer, PlayerStates.actionStates);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer == null) 
                return;
            
            localPlayer.Draw(spriteBatch);
            localPlayer.DrawArrowPower(spriteBatch, shootingTimer, camera);

            spriteBatch.DrawString(spriteFont, localPlayer.id.ToString(), localPlayer.position - new Vector2(-13, 28) - camera.Position, Color.White);

        //    spriteBatch.DrawString(spriteFont, localPlayer.VerticalStateType.ToString(), localPlayer.position - new Vector2(0, 20) - camera.Position, Color.White);
        //    spriteBatch.DrawString(spriteFont, localPlayer.HorizontalStateType.ToString(), localPlayer.position - new Vector2(0, 40) - camera.Position, Color.White);
        //    spriteBatch.DrawString(spriteFont, localPlayer.ActionStateType.ToString(), localPlayer.position - new Vector2(0, 60) - camera.Position, Color.White);
        }  

        private void HandleHorizontalMovement()
        {
            if (inputManager.Left)
            {
                localPlayer.HorizontalState = localPlayer.HorizontalState.MovedLeft(playerId, localPlayer, PlayerStates.horizontalStates);
            }
            else if (inputManager.PreviouslyLeft)
            {
                localPlayer.HorizontalState = localPlayer.HorizontalState.StoppedMovingLeft(playerId, localPlayer, PlayerStates.horizontalStates);
            }
            else if (inputManager.Right)
            {
                localPlayer.HorizontalState = localPlayer.HorizontalState.MovedRight(playerId, localPlayer, PlayerStates.horizontalStates);
            }
            else if (inputManager.PreviouslyRight)
            {
                localPlayer.HorizontalState = localPlayer.HorizontalState.StoppedMovingRight(playerId, localPlayer, PlayerStates.horizontalStates);
            }
        }

        private void HandleVerticalMovement()
        {
            if (inputManager.Jump)
                localPlayer.VerticalState = localPlayer.VerticalState.Jumped(playerId, localPlayer, PlayerStates.verticalStates);
        }

        private void HandleActions(GameTime gameTime)
        {            
            if (inputManager.PreparingShot)
            {
                localPlayer.ActionState = localPlayer.ActionState.PreparingShot(playerId, localPlayer, PlayerStates.actionStates);
                shootingTimer += gameTime.ElapsedGameTime.Milliseconds;
                shootingTimer = MathHelper.Clamp(shootingTimer, 0, 1000);
            }
            else
            {
                localPlayer.ActionState = localPlayer.ActionState.ShotReleased(playerId, localPlayer, shootingTimer, camera.CameraToWorld(inputManager.MousePosition), PlayerStates.actionStates);
                shootingTimer = 0f;
            }
        }
    }
}