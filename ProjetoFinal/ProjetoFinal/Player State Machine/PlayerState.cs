using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;
using OgmoLibrary;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    abstract class PlayerState
    {
        bool isLocal;

        protected PlayerState(bool isLocal)
        {
            this.isLocal = isLocal;
        }

        public abstract PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates);

        #region Public Messages

        public virtual PlayerState Jumped(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            return this;
        }

        public virtual PlayerState MovedLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            return this;
        }

        public virtual PlayerState StoppedMovingLeft(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            return this;
        }

        public virtual PlayerState MovedRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            return this;
        }

        public virtual PlayerState StoppedMovingRight(short playerId, Player localPlayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
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

            if (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2))
                return true;

            return false;
        }

        protected bool checkHorizontalCollision(Rectangle collisionBox, Vector2 speed, Layer collisionLayer)
        {
            Point corner1, corner2;

            if (speed.X < 0)
            {
                corner1 = new Point(collisionBox.Left, collisionBox.Top);
                corner2 = new Point(collisionBox.Left, collisionBox.Bottom);
            }
            else
            {
                corner1 = new Point(collisionBox.Right, collisionBox.Top);
                corner2 = new Point(collisionBox.Right, collisionBox.Bottom);
            }

            return (collisionLayer.GetTileValueByPixelPosition(corner1) || collisionLayer.GetTileValueByPixelPosition(corner2));
        }

        protected bool handleHorizontalCollision(Player localPlayer, Layer collisionLayer)
        {
            Rectangle collisionBoxOffset = localPlayer.CollisionBox;

            for (int i = 0; i < Math.Abs(localPlayer.Speed.X); ++i)
            {
                collisionBoxOffset.Offset(Math.Sign(localPlayer.Speed.X), 0);
                if (!checkHorizontalCollision(collisionBoxOffset, localPlayer.Speed, collisionLayer))
                {
                    localPlayer.Position += new Vector2(Math.Sign(localPlayer.Speed.X), 0);
                }
                else
                {
                    localPlayer.SpeedX = 0;
                    return true;
                }
            }

            return false;
        }

        protected bool handleVerticalCollision(Player localPlayer, Layer collisionLayer)
        {
            Rectangle collisionBoxOffset = localPlayer.CollisionBox;

            for (int i = 0; i < Math.Abs(localPlayer.Speed.Y); ++i)
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

        protected void OnPlayerStateChanged(short playerId, Player player, PlayerStateType playerState)
        {
            player.State = playerState;

            if(isLocal)
                EventManager.Instance.throwPlayerStateChanged(playerId, player);
        }

        // So player doesn't slide forever        
        protected bool clampHorizontalSpeed(Player localPlayer)
        {            
            if (Math.Abs(localPlayer.Speed.X) < 0.2)
            {
                localPlayer.SpeedX = 0;

                return true;   
            }

            return false;
        }

        #endregion
    }
}
