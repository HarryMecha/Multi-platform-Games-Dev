using UnityEngine;

public class PatrolState : BaseState
{
    public PatrolState(EnemyController enemyController) : base(enemyController) { }

    public override void Enter()
    {
        Debug.Log("Entered Patrol State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }

    public override void Update()
    {
        Debug.Log("Patroling");
        enemyController.Patrolling();
    }
}