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
    private Rigidbody playerRigidbody;
    private groundType currentGroundType;
    private Vector2 moveValue;
    private Vector2 lookValue;
    public float moveSpeed;
    private InputAction moveAction;
    private InputAction lookAction;
    private Vector3 cameraRotation;
    private Dictionary<string, int> collectibleCount;
    public GameObject harpoonGunObject;
    private HarpoonGun harpoonGun;
    private weaponSelection currentWeapon;

    public enum groundType
    {
        Regular,
        Ice,
        Bounce,
        Jumping,
        Swinging
    }

    enum weaponSelection
    {
        fists,
        harpoonGun
    }

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentGroundType = groundType.Jumping;
        lookValue = Vector3.zero;
        collectibleCount = new Dictionary<string, int>();
        harpoonGun = harpoonGunObject.GetComponent<HarpoonGun>();
        currentWeapon = weaponSelection.harpoonGun;
    }

    public void setGroundType(groundType type)
    {
        currentGroundType = type;
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
        switch (currentWeapon)
        {
            case(weaponSelection.fists):
                Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, HitDistance))
                {
                    if(hit.transform.tag == "Enemy")
                    {

                    }
                }
            break;


            case(weaponSelection.harpoonGun):

                harpoonGun.shootRope();

            break;
        }


        

    }
    void OnJump(InputValue value)
    {
        if (currentGroundType != groundType.Jumping)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
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
        velocity.y = playerRigidbody.velocity.y; //keeps the y velocity the same, so is only affected by jump
        playerRigidbody.velocity = velocity;
    }

    void Update()
    {
        cameraRotation.x += lookValue.y * cameraSensitivity;
        cameraRotation.y += lookValue.x * cameraSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(-cameraRotation.x, 0.0f, 0.0f);
        playerRigidbody.transform.localRotation = Quaternion.Euler(0f, cameraRotation.y, 0.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            currentGroundType = (groundType) Enum.Parse(typeof(groundType), collision.gameObject.name);
        }

        if (collision.gameObject.tag == "Collectible")
        {
            if (collectibleCount.ContainsKey((string)collision.gameObject.name))
            {
                collectibleCount[collision.gameObject.name] += 1;
            }
            else collectibleCount.Add(collision.gameObject.name, 1);
            
            Debug.Log(collectibleCount[collision.gameObject.name]);
            
            Destroy(collision.gameObject);

            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            currentGroundType = (groundType)Enum.Parse(typeof(groundType), collision.gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            currentGroundType = groundType.Jumping;
        }
    }
}
