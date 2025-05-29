namespace EnemyBehaviour.StateMachine
{
    public abstract class BaseState
    {
        public abstract void EnterState(StateManager enemyStateManager);
        public abstract void ExitState(StateManager enemyStateManager);
        public abstract void UpdateState(StateManager enemyStateManager);
    }
}