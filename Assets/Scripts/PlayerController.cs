using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera PlayerCamera;
    public float CameraSensitivity;
    private float rotationX;
    private float rotationY;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rotationX = 0f; 
        rotationY = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        rotationX += Input.GetAxis("Mouse Y") * CameraSensitivity;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        rotationY += Input.GetAxis("Mouse X") * -1 * CameraSensitivity;
        PlayerCamera.transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0f);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastCommand hit;
            
        }
    }
}
