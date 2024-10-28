public abstract class BaseState
{
    protected EnemyController enemyController;

    protected BaseState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
