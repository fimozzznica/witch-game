
using UnityEngine;
using UnityEngine.AI;

namespace EnemyBehaviour.StateMachine
{
    public class StateManager : MonoBehaviour
    {
        [Header("Референсы")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform playerTransform;
        [SerializeField] public Animator animator;


        [Header("Скорости")]
        [SerializeField] public float patrolSpeed = 1.5f;
        [SerializeField] public float chaseSpeed = 3.5f;
        [SerializeField] public float retreatSpeed = 4f;

        [Header("Расстояния")]
        [SerializeField] public float agroDistance = 10f;
        [SerializeField] public float attackDistance = 2f;
        [SerializeField] public float retreatDistance = 1f;
        [SerializeField] public float retreatAmount = 3.5f;

        [Header("Настройки Idle")]
        [SerializeField] public float patrolRadius = 5f;
        [SerializeField] public float patrolWaitTime = 2f;

        [Header("Настройки боя")]
        [SerializeField] public float attackDuration = 6f;
        [SerializeField] public float retreatDuration = 3f;
        [SerializeField] public float forceAgroTime = 60f;

        private BaseState currentState;
        public IdleState idleState = new IdleState();
        public AgroState agroState = new AgroState();
        public AttackState attackState = new AttackState();
        public RetreatState retreatState = new RetreatState();

        public bool forcedAgro = false;
        public Vector3 startPosition { get; private set; }
        public Transform Target => playerTransform;

        private void Start()
        {
            startPosition = transform.position;
            SwitchState(idleState);
        }

        private void Update()
        {
            if (currentState != null && playerTransform != null)
            {
                currentState.UpdateState(this);
            }
            Debug.Log(currentState);
        }

        public void SwitchState(BaseState newState)
        {
            currentState?.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }

        public void SetSpeed(float speed)
        {
            if (agent != null && agent.isActiveAndEnabled)
                agent.speed = speed;
        }

        public void SetDestination(Vector3 destination)
        {
            if (agent != null && agent.isActiveAndEnabled)
                agent.SetDestination(destination);
        }

        public float DistanceToTarget()
        {
            if (playerTransform == null)
                return float.MaxValue;

            return Vector3.Distance(transform.position, playerTransform.position);
        }

        void ChekConditions()
        {
            if(currentState== attackState)
            {
                if (DistanceToTarget()>= attackDistance)
                {
                    SwitchState(agroState);
                    return;
                }
            }
        }

        void TestAnimationEvent()
        {
            Debug.Log("Animation Event");
        }
    }
}