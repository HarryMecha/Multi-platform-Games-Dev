using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public DeadState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Dead State");
        enemyController.SetDeadAnimation(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        enemyController.Dead(Time.time);
        Debug.Log("Destroyed GameObject");
    }
}
