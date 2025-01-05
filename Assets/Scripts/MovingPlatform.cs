using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startingLocalPosition;
    private Vector3 endLocalPosition;
    private Vector3 previousLocalPlatformPosition;
    public enum MovementDirection
    {
        UpDwn,
        LR,
        FwdBck
    }
    public MovementDirection direction;
    public float movementScalar;
    public float speed;
    private float startTime;
    private float journeyDistance;

    private List<Transform> objectsOnPlatform = new List<Transform>();

    void Awake()
    {
        startTime = Time.time;
        startingLocalPosition = transform.localPosition;
        previousLocalPlatformPosition = startingLocalPosition;

        /* Calculate the platform's end position based on the direction set, in local space. */
        switch (direction)
        {
            case MovementDirection.UpDwn:
                endLocalPosition = (startingLocalPosition + (Vector3.up * movementScalar));
                break;
            case MovementDirection.FwdBck:
                endLocalPosition = (startingLocalPosition + (Vector3.forward * movementScalar));
                break;
            case MovementDirection.LR:
                endLocalPosition = (startingLocalPosition + (Vector3.left * movementScalar));
                break;
        }

        journeyDistance = Vector3.Distance(startingLocalPosition, endLocalPosition);
    }

    void Update()
    {
        /* Handles the movement of the platform using linear interpolation in local space. */
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyDistance;
        transform.localPosition = Vector3.Lerp(startingLocalPosition, endLocalPosition, fractionOfJourney);

        // Check if we reached the end, then reverse direction
        if (transform.localPosition == endLocalPosition)
        {
            Vector3 prevStartPosition = startingLocalPosition;
            startingLocalPosition = endLocalPosition;
            endLocalPosition = prevStartPosition;
            startTime = Time.time;
        }

        // Calculate the platform's global position delta
        Vector3 globalPlatformDelta = transform.position - transform.parent.TransformPoint(previousLocalPlatformPosition);

        // Update the global position of objects on the platform
        foreach (var obj in objectsOnPlatform)
        {
            if (obj != null)
            {
                obj.position += globalPlatformDelta;
            }
        }

        // Update the previous platform position in local space
        previousLocalPlatformPosition = transform.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform obj = collision.collider.transform;
        if (!objectsOnPlatform.Contains(obj))
        {
            objectsOnPlatform.Add(obj);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Transform obj = collision.collider.transform;
        if (objectsOnPlatform.Contains(obj))
        {
            objectsOnPlatform.Remove(obj);
        }
    }
}
