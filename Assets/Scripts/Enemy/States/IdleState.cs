using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Idle State");
        enemyController.SetMotionSpeed(0f);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }

    public override void Update()
    {
        Debug.Log("Idling");
        if (enemyController.playerInSightRange()) enemyController.ChangeState(new ChaseState(enemyController));
        else if (enemyController.playerInAttackRange()) enemyController.ChangeState(new AttackState(enemyController));
        else enemyController.ChangeState(new PatrolState(enemyController));
    }
}
