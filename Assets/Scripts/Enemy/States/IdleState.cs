using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Idle State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }

    public override void Update()
    {
        base.Update();
    }
}
