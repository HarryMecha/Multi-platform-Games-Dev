
using UnityEngine;

// Represents the "Attack" state in the enemy's state machine
public class AttackState : BaseState
{
    // Constructor to initialize the State with the associated enemy controller
    public AttackState(EnemyController enemyController) : base(enemyController) { }

    // Method called when the state is entered
    public override void Enter()
    {
        Debug.Log("Entered Attacking State");
        enemyController.SetMotionSpeed(0f);
        enemyController.PlayAttackAudio();
    }

    // Method called when the state is exited
    public override void Exit()
    {
        Debug.Log("Exiting Attacking State");
    }

    // Method called on every frame update
    public override void Update()
    {
        Debug.Log("Attacking");
        enemyController.Attacking();
        if (!enemyController.playerInAttackRange()) enemyController.ChangeState(new ChaseState(enemyController));
    }
}
