using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzle : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform rotatingCylinder;
    [SerializeField] private Transform target;
    public float rotationSpeed = 90f;
    private bool canRotate = true; // Flag to control rotation
    #endregion

    void Update()
    {
        // If the PuzzleHole is able to it will rotate in its forwards direction as time progresses.
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
