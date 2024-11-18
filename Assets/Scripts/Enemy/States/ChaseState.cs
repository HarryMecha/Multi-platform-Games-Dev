using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Chasing State");
        enemyController.SetMotionSpeed(1f);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Chasing State");
    }

    public override void Update()
    {
        Debug.Log("Chasing");
    }
}
