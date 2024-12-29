using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Attacking State");
        enemyController.SetMotionSpeed(0f);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void Update()
    {
        Debug.Log("Attacking");
        enemyController.Attacking();
        if (!enemyController.playerInAttackRange()) enemyController.ChangeState(new ChaseState(enemyController));
    }
}
