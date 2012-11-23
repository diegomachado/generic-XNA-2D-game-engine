using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.PlayerStateMachine.MovementStates.HorizontalMovementStates;
using ProjetoFinal.PlayerStateMachine.MovementStates.VerticalMovementStates;
using ProjetoFinal.PlayerStateMachine.ActionStates;

namespace ProjetoFinal.PlayerStateMachine
{
    static class PlayerStates
    {
        public static Dictionary<HorizontalStateType, HorizontalMovementState>
            horizontalStates = new Dictionary<HorizontalStateType, HorizontalMovementState>()
        {
            {HorizontalStateType.Idle, new HorizontalIdleState()},
            {HorizontalStateType.StoppingWalkingLeft, new StoppingWalkingLeftState()},
            {HorizontalStateType.StoppingWalkingRight, new StoppingWalkingRightState()},
            {HorizontalStateType.WalkingLeft, new WalkingLeftState()},
            {HorizontalStateType.WalkingRight, new WalkingRightState()}
        };

        public static Dictionary<VerticalStateType, VerticalMovementState>
            verticalStates = new Dictionary<VerticalStateType, VerticalMovementState>()
        {
            {VerticalStateType.Idle, new VerticalIdleState()},
            {VerticalStateType.StartedJumping, new StartedJumpingState()},
            {VerticalStateType.Jumping, new JumpingState()}
        };

        public static Dictionary<ActionStateType, ActionState>
            actionStates = new Dictionary<ActionStateType, ActionState>()
        {
            {ActionStateType.Idle, new ActionIdleState()},
            {ActionStateType.Attacking, new AttackingState()},
            {ActionStateType.Defending, new DefendingState()},
            {ActionStateType.PreparingShot, new PreparingShotState()},
            {ActionStateType.Shooting, new ShootingState()}
        };
    }
}
