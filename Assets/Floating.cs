using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private float floatSpeed;                    // Speed of the floating motion
    private float floatAmplitude;                // How far the object floats (amplitude)
    private Vector3 startPosition;               // Initial position of the object
    private Vector3 floatDirection = Vector3.up; // Direction of the floating motion

    // Start is called before the first frame update
    void Start()
    {
        // Store the starting position of the object
        startPosition = transform.position;

        // Assign Random values within certain range to add randomess is game
        floatSpeed = Random.Range(1.0f, 2.0f);
        floatAmplitude = Random.Range(0.1f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the floating motion
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Apply the floating motion
        transform.position = startPosition + floatDirection.normalized * offset;
    }
}
