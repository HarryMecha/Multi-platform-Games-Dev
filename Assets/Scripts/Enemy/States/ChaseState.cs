
using UnityEngine;

// Represents the "Chase" state in the enemy's state machine
public class ChaseState : BaseState
{
    // Constructor to initialize the State with the associated enemy controller
    public ChaseState(EnemyController enemyController) : base(enemyController) { }

    // Method called when the state is entered
    public override void Enter()
    {
        Debug.Log("Entered Chasing State");
        enemyController.SetMotionSpeed(1f);
        enemyController.PlayRoarAudio();
    }

    // Method called when the state is exited
    public override void Exit()
    {
        Debug.Log("Exiting Chasing State");
    }

    // Method called on every frame update
    public override void Update()
    {
        Debug.Log("Chasing");
        enemyController.Chasing();
        if (!enemyController.playerInSightRange()) enemyController.ChangeState(new IdleState(enemyController));
        else if (enemyController.playerInAttackRange()) enemyController.ChangeState(new AttackState(enemyController));
    }
}
