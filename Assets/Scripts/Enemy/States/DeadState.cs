
using UnityEngine;

// Represents the "Dead" state in the enemy's state machine
public class DeadState : BaseState
{
    // Constructor to initialize the State with the associated enemy controller
    public DeadState(EnemyController enemyController) : base(enemyController) { }

    // Method called when the state is entered
    public override void Enter()
    {
        Debug.Log("Entered Dead State");
        enemyController.SetDeadAnimation(true);
    }

    // Method called when the state is exited
    public override void Exit()
    {
        base.Exit();
    }

    // Method called on every frame update
    public override void Update()
    {
        enemyController.Dead(Time.time);
        Debug.Log("Destroyed GameObject");
    }
}
