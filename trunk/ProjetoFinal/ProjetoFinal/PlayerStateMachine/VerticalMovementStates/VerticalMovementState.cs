using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using OgmoLibrary;
using ProjetoFinal.Entities;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum VerticalStateType : short
    {
        Idle,
        Jumping,
        StartedJumping
    }

    abstract class VerticalMovementState : PlayerState
    {
        public abstract VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<VerticalStateType, VerticalMovementState> playerStates);

        #region Public Messages

        public virtual VerticalMovementState Jumped(short playerId, Player player, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            return this;
        }

        #endregion

        #region Protected Methods
        protected bool checkVerticalCollision(Rectangle collisionBox, Vector2 speed, Layer collisionLayer)
        {
            Point corner1, corner2;

            if (speed.Y < 0)
            {
                corner1 = new Point(collisionBox.Left, collisionBox.Top);
                corner2 = new Point(collisionBox.Right, collisionBox.Top);
            }
            else
            {
                corner1 = new Point(collisionBox.Left, collisionBox.Bottom);
                corner2 = new Point(collisionBox.Right, collisionBox.Bottom);
            }

            return (collisionLayer.TileIdByPixelPosition(corner1) || collisionLayer.TileIdByPixelPosition(corner2));
        }

        protected bool handleVerticalCollision(Player localPlayer, Layer collisionLayer, double elapsedTime)
        {
            Rectangle collisionBoxOffset = localPlayer.CollisionBox;

            for (int i = 0; i < Math.Abs(localPlayer.Speed.Y * elapsedTime); ++i)
            {
                collisionBoxOffset.Offset(0, Math.Sign(localPlayer.Speed.Y));

                if (!checkVerticalCollision(collisionBoxOffset, localPlayer.Speed, collisionLayer))
                {
                    localPlayer.Position += new Vector2(0, Math.Sign(localPlayer.Speed.Y));
                }
                else
                {
                    localPlayer.SpeedY = 0;
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
