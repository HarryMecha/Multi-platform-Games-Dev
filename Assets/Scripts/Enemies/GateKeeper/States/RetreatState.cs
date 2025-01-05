
using UnityEngine;

// Represents the "Retreat" state in the enemy's state machine
public class RetreatState : BaseState
{
    // Constructor to initialize the State with the associated enemy controller
    public RetreatState(EnemyController enemyController) : base(enemyController) { }

    // Method called when the state is entered
    public override void Enter()
    {
        base.Enter();
    }

    // Method called when the state is exited
    public override void Exit()
    {
        base.Exit();
    }

    // Method called on every frame update
    public override void Update()
    {
        base.Update();
    }
}
