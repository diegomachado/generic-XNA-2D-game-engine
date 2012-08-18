using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetoFinal.Managers.LocalPlayerStates;

namespace ProjetoFinal.Entities
{
    class Player : Entity
    {
        public VerticalStateType LastVerticalState { get; set; }
        public HorizontalStateType LastHorizontalState { get; set; }
        public ActionStateType LastActionState { get; set; }
        public Vector2 JumpForce { get; set; }
        public Vector2 walkForce { get; set; }        
        public float Friction { get; set; }
        public bool FacingLeft { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition, Rectangle boundingBox)
            : base(playerSkin, playerPosition, boundingBox)
        {            
            walkForce = new Vector2(60, 0);
            Friction = 0.85f;
            Gravity = new Vector2(0, 20f);
            JumpForce = new Vector2(0, -480f);
            LastVerticalState = VerticalStateType.Idle;
            LastHorizontalState = HorizontalStateType.Idle;
            LastActionState = ActionStateType.Idle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Tirar a conta Camera.Instance.Position - Position daqui e jogar ela dentro de Camera tipo: Camera.Instance.ScreenToCameraCoordinates(Position)
            if (FacingLeft)
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(skin, Position - camera.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
