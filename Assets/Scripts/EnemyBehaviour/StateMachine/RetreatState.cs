
using UnityEngine;

namespace EnemyBehaviour.StateMachine
{
    public class RetreatState : BaseState
    {
        private Vector3 retreatPosition;
        private float retreatTimer = 0f;

        public override void EnterState(StateManager stateManager)
        {
            stateManager.SetSpeed(stateManager.retreatSpeed);
            retreatTimer = 0f;
            CalculateRetreatPosition(stateManager);
            stateManager.animator.SetBool("isAgro", false);
            stateManager.animator.SetBool("isAttack", false);
            stateManager.animator.SetBool("isRetreat", true);
            stateManager.animator.SetBool("isDeath", false);
            
        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {
            retreatTimer += Time.deltaTime;

            if (retreatTimer >= stateManager.retreatDuration ||
                Vector3.Distance(stateManager.transform.position, retreatPosition) < 0.5f)
            {
                if (stateManager.DistanceToTarget() < stateManager.agroDistance)
                {
                    stateManager.SwitchState(stateManager.agroState);
                }
                else
                {
                    stateManager.SwitchState(stateManager.idleState);
                }
            }
        }

        private void CalculateRetreatPosition(StateManager stateManager)
        {
            Vector3 directionFromTarget = (stateManager.transform.position - stateManager.Target.position).normalized;

            retreatPosition = stateManager.transform.position + directionFromTarget * stateManager.retreatAmount;

            stateManager.SetDestination(retreatPosition);
        }
    }
}
