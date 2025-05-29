
using UnityEngine;

namespace EnemyBehaviour.StateMachine
{
    public class IdleState : BaseState
    {
        private Vector3 targetPoint;
        private float patrolTimer = 0f;
        private bool isWaiting = false;
        private float idleTimeCounter = 0f;

        public override void EnterState(StateManager stateManager)
        {
            stateManager.SetSpeed(stateManager.patrolSpeed);
            ChooseNewPatrolPoint(stateManager);

            idleTimeCounter = 0f;
            stateManager.forcedAgro = false;

            stateManager.animator.SetBool("isAgro", false);
            stateManager.animator.SetBool("isAttack", false);
            stateManager.animator.SetBool("isRetreat",false );
            stateManager.animator.SetBool("isDeath", false);
        }

        public override void ExitState(StateManager stateManager)
        {

        }

        public override void UpdateState(StateManager stateManager)
        {
            idleTimeCounter += Time.deltaTime;

            if (idleTimeCounter >= stateManager.forceAgroTime)
            {
                stateManager.forcedAgro = true;
                stateManager.SwitchState(stateManager.agroState);
                return;
            }

            if (stateManager.DistanceToTarget() < stateManager.agroDistance)
            {
                stateManager.SwitchState(stateManager.agroState);
                return;
            }

            if (isWaiting)
            {
                patrolTimer += Time.deltaTime;
                if (patrolTimer >= stateManager.patrolWaitTime)
                {
                    isWaiting = false;
                    patrolTimer = 0f;
                    ChooseNewPatrolPoint(stateManager);
                    stateManager.SetSpeed(stateManager.patrolSpeed);
                }
            }
            else
            {
                if (Vector3.Distance(stateManager.transform.position, targetPoint) < 1.5f)
                {
                    isWaiting = true;
                    stateManager.SetSpeed(0f);
                }
            }
        }

        private void ChooseNewPatrolPoint(StateManager stateManager)
        {
            Vector2 randomCircle = Random.insideUnitCircle * stateManager.patrolRadius;
            targetPoint = stateManager.startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
            stateManager.SetDestination(targetPoint);
        }
    }
}