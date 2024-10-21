using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlastTrap : MonoBehaviour
{
    // Variables for references
    [Header("References")]
    [SerializeField] private Transform playerObject; // Reference to the player's Transform component
    [SerializeField] private LayerMask player;       // Layer mask to identify the player

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float damageRadius; // Radius in which objects will receive damage
    [SerializeField] private float damageAmount; // Damage dealt to objects within the radius
    [SerializeField] private float explodeDelay; // Time delay before exploding

    private NavMeshAgent agent;        // Reference to the NavMeshAgent component to handle movement
    private bool playerInTriggerRange; // Is the player within the trigger range

    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for trigger range
        playerInTriggerRange = Physics.CheckSphere(transform.position, damageRadius, player);

        // If the player is trigger range, explode the object
        if (playerInTriggerRange)
            Invoke(nameof(Explode), explodeDelay);

    }

    private void Explode()
    {
        Debug.Log("Object exploded!");

        // Health system implementation

        // Destroy the explosive object
        Destroy(gameObject);
    }
}
