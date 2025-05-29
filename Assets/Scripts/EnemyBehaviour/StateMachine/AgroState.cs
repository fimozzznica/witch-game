
using UnityEngine;

namespace EnemyBehaviour.StateMachine
{
    public class AgroState : BaseState
    {
        public override void EnterState(StateManager stateManager)
        {
            stateManager.SetSpeed(stateManager.chaseSpeed);
            stateManager.animator.SetBool("isAgro", true); 
            stateManager.animator.SetBool("isAttack", false);
            stateManager.animator.SetBool("isRetreat", false); 
            stateManager.animator.SetBool("isDeath", false);
        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {
            if (stateManager.DistanceToTarget() < stateManager.attackDistance)
            {
                stateManager.SwitchState(stateManager.attackState);
                return;
            }

            // if (stateManager.DistanceToTarget() < stateManager.retreatDistance)
            // {
            //     stateManager.SwitchState(stateManager.retreatState);
            //     return;
            // }

            if (!stateManager.forcedAgro && stateManager.DistanceToTarget() > stateManager.agroDistance)
            {
                stateManager.SwitchState(stateManager.idleState);
                return;
            }

            stateManager.SetDestination(stateManager.Target.position);
        }
    }
}
