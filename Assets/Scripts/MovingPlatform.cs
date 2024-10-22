using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startingPosition;
    private Vector3 endPosition;
    public enum movementDirection
    {
        UpDwn,
        LR,
        FwdBck
    }
    public movementDirection direction;
    public float movementScalar;
    public float speed;
    private float startTime;
    private float journeyDistance;


    void Awake()
    {
        startTime = Time.time;
        startingPosition = transform.position;

        /*switch statement calculates the moving platforms end position based on the direction set.*/
        switch (direction)
        {
            case (movementDirection.UpDwn):
                endPosition = (startingPosition + (Vector3.up * movementScalar));
                break;
            case (movementDirection.FwdBck):
                endPosition = (startingPosition + (Vector3.forward * movementScalar));
                break;
            case (movementDirection.LR):
                endPosition = (startingPosition + (Vector3.left * movementScalar));
                break;
        }
        journeyDistance = Vector3.Distance(startingPosition, endPosition);
    }

    void Update()
    {
        /*This handles the movement of the platform using linear interpolation, moving the platform from start to end position and then reset in the opposite direction.*/
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyDistance;
        transform.position = Vector3.Lerp(startingPosition, endPosition, fractionOfJourney);
        if (transform.position == endPosition)
        {
            Vector3 prevStartPosition = startingPosition;
            startingPosition = endPosition;
            endPosition = prevStartPosition;
            startTime = Time.time;
        }
    }
}