using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BaseState currentState;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = new IdleState(this);
        currentState.Enter();
    }

    private void Update()
    {
        
    }

    private void SetDeadAnimation()
    {
        animator.SetBool("isDead", true);
    }
}
