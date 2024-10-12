using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Camera PlayerCamera;
    public float cameraSensitivity;
    public float HitDistance;
    public float jumpForce;
    private Rigidbody rigidbody;
    private string currentGroundType;
    public Vector2 moveValue;
    public float moveSpeed;
    public Vector2 lookValue;
    private InputAction moveAction;
    private InputAction lookAction;
    private Vector3 cameraRotation;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentGroundType = "Jumping";
        lookValue = Vector3.zero;
    }

    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, HitDistance))
        {
            Debug.Log("You are looking at " + hit.transform.name);
        }

    }
    void OnJump(InputValue value)
    {
        if (currentGroundType != "Jumping")
        {
          rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }
    

    private void FixedUpdate()
    {
        Vector3 cameraForward = PlayerCamera.transform.forward;
        cameraForward.y = 0; 
        cameraForward.Normalize(); 
        Vector3 cameraRight = PlayerCamera.transform.right;
        Vector3 moveDirection = (cameraForward * moveValue.y + cameraRight * moveValue.x).normalized;
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rigidbody.velocity.y; //keeps the y velocity the same, so is only affected by jump
        rigidbody.velocity = velocity;
    }

    void Update()
    {
        cameraRotation.x += lookValue.y * cameraSensitivity;
        cameraRotation.y += lookValue.x * cameraSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(-cameraRotation.x, 0.0f, 0.0f);
        rigidbody.transform.localRotation = Quaternion.Euler(0f, cameraRotation.y, 0.0f);
        Debug.Log(currentGroundType);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            currentGroundType = collision.gameObject.name;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            currentGroundType = collision.gameObject.name;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            currentGroundType = "Jumping";
        }
    }
}
