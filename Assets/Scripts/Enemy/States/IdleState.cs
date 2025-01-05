
using UnityEngine;

// Represents the "Idle" state in the enemy's state machine
public class IdleState : BaseState
{
    // Constructor to initialize the State with the associated enemy controller
    public IdleState(EnemyController enemyController) : base(enemyController) { }

    // Method called when the state is entered
    public override void Enter()
    {
        Debug.Log("Entered Idle State");
        enemyController.SetMotionSpeed(0f);
    }

    // Method called when the state is exited
    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }

    // Method called on every frame update
    public override void Update()
    {
        Debug.Log("Idling");
        if (enemyController.playerInSightRange()) enemyController.ChangeState(new ChaseState(enemyController));
        else if (enemyController.playerInAttackRange()) enemyController.ChangeState(new AttackState(enemyController));
        else enemyController.ChangeState(new PatrolState(enemyController));
    }
}
