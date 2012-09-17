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
        ~DynamicEntity()
        {
            Console.WriteLine(this);
            Entity.Entities.Remove(this);
        }

        public override void LoadContent()
        {
            this.baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Bear), 1, 1);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            baseAnimation.Draw(spriteBatch, position, false);
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
    }
}
