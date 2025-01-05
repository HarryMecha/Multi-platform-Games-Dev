using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzle : MonoBehaviour
{
    [SerializeField] private Transform rotatingCylinder;
    [SerializeField] private Transform target;
    public float rotationSpeed = 90f;
    private bool canRotate = true; // Flag to control rotation

    void Update()
    {
        // Rotate the wheel only if allowed
        if (canRotate)
        {
            rotatingCylinder.Rotate(transform.forward, rotationSpeed * Time.deltaTime, Space.World);
        }

        canRotate = target.GetComponent<TargetCollision>().askIsHit();
    }

    public bool getCanRotate()
    {
        return canRotate;
    }
}
