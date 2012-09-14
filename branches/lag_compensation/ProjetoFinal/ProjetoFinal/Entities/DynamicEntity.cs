using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities.Utils;
using ProjetoFinal.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities
{
    class DynamicEntity : Entity
    {
        public float jumpSpeed;
        public Vector2 speed, minSpeed, maxSpeed;
        public float moveSpeed, gravity, friction;
        public bool isMovingLeft, isMovingRight;

        InputManager inputManager = InputManager.Instance;
        
        public DynamicEntity(Vector2 _position) : base(_position)
        {     
            flags = Flags.Gravity;
            boundingBox = new Rectangle();
            minSpeed = new Vector2();
            maxSpeed = new Vector2();
            moveSpeed = 0;
            jumpSpeed = 0;
            gravity = 0.5f;
            friction = 0.8f;
            isMovingLeft = isMovingRight = false;
        }

        public override void LoadContent()
        {
            this.baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Bear), 1, 1);
        }

        public override void Update(GameTime gameTime)
        {
            /*
            isMovingRight = inputManager.Right ? true : false;
            isMovingLeft  = inputManager.Left  ? true : false;

            if ((isMovingLeft && isMovingRight) || (!isMovingLeft && !isMovingRight)) speed.X = 0;
            
            if (OnGround() || MapCollideY(-1)) speed.Y = 0;            

            if ((flags & Flags.Gravity) == Flags.Gravity) speed.Y += gravity;

            if (InputManager.Instance.Jump) Jump();

            if (isMovingLeft) speed.X -= moveSpeed;
            if (isMovingRight) speed.X += moveSpeed;

            speed.X *= friction;

            speed.X = MathHelper.Clamp(speed.X, minSpeed.X, maxSpeed.X);
            speed.Y = MathHelper.Clamp(speed.Y, minSpeed.Y, maxSpeed.Y);

            baseAnimation.Update();
            MoveBy(speed);
             */
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            baseAnimation.Draw(spriteBatch, position, false);
            //DebugDraw(spriteBatch, spriteFont);
        }

        public bool IsMovingHorizontally() { return (speed.X != 0); }
        public bool IsMovingVertically() { return (speed.Y != 0); }

        public void Jump() 
        {
            if (OnGround()) 
                speed.Y += jumpSpeed; 
        }

        public bool OnGround() { return MapCollideY(1); }

        public void StopMovingHorizontally() { speed.X = 0; }
        public void StopMovingVertically()   { speed.Y = 0; }

        Texture2D debugBackground;
        public void DebugDraw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            debugBackground = TextureManager.Instance.GetPixelTexture();
            spriteBatch.Draw(debugBackground, new Rectangle(0, 0, 230, 170), new Color(0, 0, 0, 0.2f));
            spriteBatch.DrawString(spriteFont, "Position: " + new Point((int)position.X, (int)position.Y), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Speed: " + new Point((int)speed.X, (int)speed.Y), new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(spriteFont, "MoveRight: " + isMovingRight, new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(spriteFont, "MoveLeft: " + isMovingLeft, new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(spriteFont, "onGround: " + OnGround(), new Vector2(10, 90), Color.White);
            Util.DrawRectangle(spriteBatch, CollisionBox, 1, Color.Red);
        }
    }
}
