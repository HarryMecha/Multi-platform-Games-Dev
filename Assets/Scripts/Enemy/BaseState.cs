// Abstract base class representing a state in the state machine
public abstract class BaseState
{
    // Reference to the enemy controller to access its properties and methods
    protected EnemyController enemyController;

    // Constructor to initialize the state with the associated enemy controller
    protected BaseState(EnemyController enemyController) { this.enemyController = enemyController; }

    // Virtual method called when the state is entered, exited and updated
    // Can be overridden by derived states to define specific enter behavior
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}