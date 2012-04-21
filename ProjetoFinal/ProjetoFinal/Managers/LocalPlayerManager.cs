using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.EventArgs;
using ProjetoFinal.Entities;

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
        Vector2 acceleration = Vector2.Zero;
        KeyboardState lastKeyboardState;

        //MovementState movementState = MovementState.Idle;

        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public LocalPlayerManager()
        {
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(96,240), new Rectangle(6, 2, 24, 30));        
        }
        
        protected void OnPlayerStateChanged(PlayerState playerState)
        {
            localPlayer.State = playerState;

            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(playerId, localPlayer));
        }

        // TODO: Refactor
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, Rectangle clientBounds, Layer collisionLayer)
        {
            if (localPlayer != null)
            {
                // Atualizo os valores da CollisionBox, tendo como parâmetro a posição atual do player
                // mais os valores da BoundingBox
                localPlayer.CollisionBox.X = (int)localPlayer.Position.X + localPlayer.BoundingBox.X;
                localPlayer.CollisionBox.Y = (int)localPlayer.Position.Y + localPlayer.BoundingBox.Y;

                // If Ternário. Se a velocidade vertical é nula, ele está no chão (Bug: Wall Jump)
                //localPlayer.OnGround = localPlayer.speed.Y != 0 ? false : true;                     

                // A princípio a aceleração é zero
                acceleration = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Left))
                        OnPlayerStateChanged(PlayerState.WalkingLeft);

                    acceleration += new Vector2(-0.5f, 0.0f);
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if (lastKeyboardState != null && !lastKeyboardState.IsKeyDown(Keys.Right))
                        OnPlayerStateChanged(PlayerState.WalkingRight);

                    acceleration += new Vector2(0.5f, 0.0f);
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    // Se player está no chão, ele pode pular
                    if (localPlayer.OnGround)
                    {
                        if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
                            OnPlayerStateChanged(PlayerState.Jumping);

                        if (keyboardState.IsKeyDown(Keys.Left))
                            OnPlayerStateChanged(PlayerState.JumpingLeft);
                        else if (keyboardState.IsKeyDown(Keys.Right))
                            OnPlayerStateChanged(PlayerState.JumpingRight);
                        else
                            OnPlayerStateChanged(PlayerState.Jumping);

                        acceleration += localPlayer.JumpForce;
                    }
                }

                if (!keyboardState.IsKeyDown(Keys.Space) && !keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
                    OnPlayerStateChanged(PlayerState.Idle);

                // Adiciono a gravidade do player à aceleração
                // Pensar: Vale a gravidade ser global, ou em cada player nos dará maior flexibilidade?
                acceleration += localPlayer.Gravity;
                
                // TODO: Refactor
                localPlayer.speed.Y += acceleration.Y;

                // Limito a velocidade vertical do Player
                if (localPlayer.speed.Y >= 10)
                    localPlayer.speed.Y = 10;

                // Atualizo a velocidade horizontal do player
                localPlayer.speed.X += acceleration.X;

                // TODO: Friction pode estar associada ao tile em que o player pisa
                localPlayer.speed.X *= localPlayer.Friction;

                // Seto um retangulo que armazena a próxima posição a ser ocupada pelo player
                Rectangle nextPosition = localPlayer.CollisionBox;

                // Por enquanto, só atualizo o retangulo horizontalmente
                nextPosition.Offset((int)localPlayer.speed.X, 0);

                Point corner1, corner2;

                // TODO: Falta setar a posicao do cara depois da colisao, nao so anular a velocidade dele.
                // TODO: Criar uma função booleana em Tiles que retorna se ele é passável ou não, por pixel, pra não precisar ficar dividindo tudo por 32 (na mão)

                // Se player tiver andando pra esquerda, seto os pontos de colisão TL, BL (em relação ao XY do tileGrid)
                if (localPlayer.speed.X < 0)
                {
                    corner1 = new Point((int)nextPosition.Left / 32, (int) (nextPosition.Top + 1) / 32);
                    corner2 = new Point((int)nextPosition.Left / 32, (int) (nextPosition.Bottom - 1) / 32);
                }
                // Se player tiver andando pra direita, seto os pontos de colisão TR, BR (em relação ao XY do tileGrid)
                else
                {
                    corner1 = new Point((int)nextPosition.Right / 32, (int) (nextPosition.Top + 1) / 32);
                    corner2 = new Point((int)nextPosition.Right / 32, (int) (nextPosition.Bottom - 1) / 32);
                }
                
                // Se eu tiver uma parede em algum desses pontos de colisão, anulo a velocidade horizontal
                if (collisionLayer.Tiles[corner1].Id == 1 || collisionLayer.Tiles[corner2].Id == 1)
                    localPlayer.speed.X = 0;

                // Por enquanto, só atualizo a próxima posição verticalmente
                nextPosition.Offset(0, (int)localPlayer.speed.Y);

                // Análogo ao acima, porem os pontos de colisão são no topo e no fundo
                if (localPlayer.speed.Y < 0)
                {
                    corner1 = new Point((int)(nextPosition.Left + 1) / 32, (int)nextPosition.Top / 32);
                    corner2 = new Point((int)(nextPosition.Right - 1) / 32, (int)nextPosition.Top / 32);

                    // Checo paredes, anulo velocidade vertical
                    if (collisionLayer.Tiles[corner1].Id == 1 || collisionLayer.Tiles[corner2].Id == 1)
                        localPlayer.speed.Y = 0;

                    localPlayer.OnGround = false;
                }
                else
                {
                    corner1 = new Point((int)(nextPosition.Left + 1) / 32, (int)nextPosition.Bottom / 32);
                    corner2 = new Point((int)(nextPosition.Right - 1) / 32, (int)nextPosition.Bottom / 32);

                    // Checo paredes, anulo velocidade vertical
                    if (collisionLayer.Tiles[corner1].Id == 1 || collisionLayer.Tiles[corner2].Id == 1)
                    {
                        if (localPlayer.speed.Y > 0)
                            localPlayer.OnGround = true;

                        localPlayer.speed.Y = 0;
                    }
                    else
                    {
                        localPlayer.OnGround = false;
                    }
                }
                
                // Atualizo a posição de acordo com a velocidade
                localPlayer.Position += localPlayer.speed;
                lastKeyboardState = keyboardState;
            }
        }         

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, playerId.ToString(), new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 25), Color.White);
                DrawBoundingBox(localPlayer.CollisionBox, 1, spriteBatch, TextureManager.Instance.getPixelTextureByColor(Color.Red));
            }
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, SpriteBatch spriteBatch, Texture2D borderTexture)
        {
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, borderWidth, r.Height), Color.White); // Left
            spriteBatch.Draw(borderTexture, new Rectangle(r.Right, r.Top, borderWidth, r.Height), Color.White); // Right
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Top, r.Width, borderWidth), Color.White); // Top
            spriteBatch.Draw(borderTexture, new Rectangle(r.Left, r.Bottom, r.Width, borderWidth), Color.White); // Bottom
        }
    }
}