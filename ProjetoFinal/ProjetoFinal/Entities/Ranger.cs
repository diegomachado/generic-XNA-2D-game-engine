using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities.Utils;
using ProjetoFinal.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities
{
    class Ranger : Entity
    {
        public Ranger(Vector2 position) : base(position)
        {
            type = Type.Generic;
            flags = Flags.Gravity;
            Dead = false;
            Position = position;
            MoveLeft = false;
            MoveRight = false;
            speed = Vector2.Zero;
            minSpeed = new Vector2(0, 0);
            maxSpeed = new Vector2(0, 0);
            Acceleration = Vector2.Zero;
            Gravity = new Vector2(0, 20f);    
        }

        public virtual void LoadContent()
        {
            baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Ranger), 1, 1);
        }

        public virtual void Update()
        {
            if (!MoveLeft && !MoveRight)
                StopMove();

            if (MoveLeft)
                AccelerationX = -0.5f;
            else if (MoveRight)
                AccelerationX = 0.5f;

            if ((flags & Flags.Gravity) == Flags.Gravity)
                AccelerationY = 0.75f;

            baseAnimation.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            baseAnimation.Draw(spriteBatch, new Vector2(0, 0));
        }

    }
}
