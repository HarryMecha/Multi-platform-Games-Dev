using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Attacking State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void Update()
    {
        Debug.Log("Attacking");
    }
}
