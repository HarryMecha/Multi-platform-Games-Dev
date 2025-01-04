
using UnityEngine;

// Represents the "Patrol" state in the enemy's state machine
public class PatrolState : BaseState
{
    // Constructor to initialize the State with the associated enemy controller
    public PatrolState(EnemyController enemyController) : base(enemyController) { }

    // Method called when the state is entered
    public override void Enter()
    {
        Debug.Log("Entered Patrol State");
    }

    // Method called when the state is exited
    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }

    // Method called on every frame update
    public override void Update()
    {
        Debug.Log("Patroling");
        enemyController.Patrolling();
        if (enemyController.playerInSightRange()) enemyController.ChangeState(new ChaseState(enemyController));

    }
}
