using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.EventArgs;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

using OgmoLibrary;

namespace ProjetoFinal.Managers
{
    #region StatesAndShitCommented
    public enum MovementState
    {
        Idle,

        WalkingLeft,
        WalkingRight,
        WalkingDead,

        Jumping,
        JumpingLeft,
        JumpingRight,

        Falling,
        FallingLeft,
        FallingRight
    }

    public enum ActionState
    {
        Idle,
        Striking,
        Shooting        
    }
    #endregion

    class LocalPlayerManager
    {
        public short playerId { get; set; }
        
        Player localPlayer;
        Vector2 speed = Vector2.Zero;

        KeyboardState lastKeyboardState;
        LocalPlayerState localPlayerState;
        Dictionary<PlayerState, LocalPlayerState> localPlayerStates = new Dictionary<PlayerState, LocalPlayerState>();
        
        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayerManager()
        {
            localPlayerStates[PlayerState.Idle] = new IdleState();
            localPlayerStates[PlayerState.JumpingStraight] = new JumpingStraightState();
            localPlayerStates[PlayerState.JumpingLeft] = new JumpingLeftState();
            localPlayerStates[PlayerState.JumpingRight] = new JumpingRightState();
            localPlayerStates[PlayerState.WalkingLeft] = new WalkingLeftState();
            localPlayerStates[PlayerState.WalkingRight] = new WalkingRightState();

            localPlayerState = localPlayerStates[PlayerState.JumpingStraight];
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(240, 40), new Rectangle(5, 1, 24, 30));   
        }
        
        protected void OnPlayerStateChanged(PlayerState playerState)
        {
            localPlayer.State = playerState;

            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(playerId, localPlayer));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Layer collisionLayer)
        {
            if (localPlayer == null)
                return;

            // Input

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space))
                localPlayerState = localPlayerState.Jumped(localPlayer, localPlayerStates);

            if ( (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right)) || 
                 (keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.D)) )
                localPlayerState = localPlayerState.MovingLeft(localPlayer, localPlayerStates);
            else if ( (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left)) || 
                      (keyboardState.IsKeyDown(Keys.D) && !keyboardState.IsKeyDown(Keys.A)) )
                localPlayerState = localPlayerState.MovingRight(localPlayer, localPlayerStates);

            localPlayerState = localPlayerState.Update(gameTime, localPlayer, collisionLayer, localPlayerStates);

            Camera.Instance.Position = localPlayer.Position
                                        + new Vector2(localPlayer.Skin.Width / 2, localPlayer.Skin.Height / 2)
                                        - new Vector2(Game.ScreenSize.X / 2, Game.ScreenSize.Y / 2);

            lastKeyboardState = keyboardState;      
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                
                spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 0, 170, 170), new Color(0, 0, 0, 0.2f));
                
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 20) - Camera.Instance.Position, Color.White);
                spriteBatch.DrawString(spriteFont, "OnGround: " + localPlayer.OnGround.ToString(), new Vector2(5f, 5f), Color.White);                
                spriteBatch.DrawString(spriteFont, "X: " + (int)localPlayer.Position.X, new Vector2(5f, 25f), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + (int)localPlayer.Position.Y, new Vector2(5f, 45f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)localPlayer.Speed.X, new Vector2(5f, 65f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)localPlayer.Speed.Y, new Vector2(5f, 85f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)Camera.Instance.Position.X, new Vector2(5f, 105f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)Camera.Instance.Position.Y, new Vector2(5f, 125f), Color.White);
                spriteBatch.DrawString(spriteFont, "State: " + localPlayerState, new Vector2(5f, 145f), Color.White);
           }
        }

        public void DrawPoint(SpriteBatch spriteBatch, Point position, int size, Color color)
        {
            // TODO: Transformar conta com Camera em uma funcao de Camera
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(position.X - (int)Camera.Instance.Position.X, position.Y - (int)Camera.Instance.Position.Y, size, size), Color.White);
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, Color color, SpriteBatch spriteBatch)
        {
            // TODO: Transformar conta com Camera em uma funcao de Camera
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, borderWidth, r.Height), Color.White); // Left
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Right - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, borderWidth, r.Height), Color.White); // Right
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Top - (int)Camera.Instance.Position.Y, r.Width, borderWidth), Color.White); // Top
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)Camera.Instance.Position.X, r.Bottom - (int)Camera.Instance.Position.Y, r.Width, borderWidth), Color.White); // 
        }
    }
}