
using UnityEngine;

namespace EnemyBehaviour.StateMachine
{
    public class AttackState : BaseState
    {
        private float attackTimer = 0f;

        public override void EnterState(StateManager stateManager)
        {
            stateManager.SetSpeed(0);
            attackTimer = 0f;
            stateManager.animator.SetBool("isAgro", false);
            stateManager.animator.SetBool("isAttack", true);
            stateManager.animator.SetBool("isRetreat", false);
            stateManager.animator.SetBool("isDeath", false);
        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {
            attackTimer += Time.deltaTime;

            if (stateManager.DistanceToTarget() > stateManager.attackDistance)
            {
                stateManager.SwitchState(stateManager.agroState);
                return;
            }

            //if (stateManager.DistanceToTarget() < stateManager.retreatDistance)
            //{
            //stateManager.SwitchState(stateManager.retreatState);
            //return;
            //}

            if (attackTimer >= stateManager.attackDuration)
            {
                stateManager.SwitchState(stateManager.retreatState);
            }
        }
    }
}