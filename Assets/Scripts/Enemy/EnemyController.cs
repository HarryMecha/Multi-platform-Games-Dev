using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Transform playerObject;

    [Header("Define Range")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;

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
        currentState.Update();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool playerInSightRange()
    {
        if (Vector3.Distance(playerObject.position, transform.position) <= sightRange) return true;
        else return false;
    }

    public bool playerInAttackRange()
    {
        if (Vector3.Distance(playerObject.position, transform.position) <= attackRange) return true;
        else return false;
    }

    public void SetMotionSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void SetDeadAnimation(bool value)
    {
        animator.SetBool("isDead", value);
    }

    public void Dead(float time)
    {
        if(time >= 5f) Destroy(gameObject);
    }
}
