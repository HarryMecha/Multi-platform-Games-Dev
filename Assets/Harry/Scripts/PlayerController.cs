using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private Vector3 spawnLocation;
    public Camera PlayerCamera;
    public float cameraSensitivity;
    public float HitDistance;
    public float jumpForce;
    private Rigidbody playerRigidbody;
    private groundType currentGroundType;
    private Vector2 moveValue;
    private Vector2 lookValue;
    private Vector2 scrollValue;
    private float sprintValue;
    public float moveSpeed;
    private float currentMoveSpeed;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction swapAction;
    private Vector3 cameraRotation;
    private Dictionary<string, int> collectibleCount;
    public GameObject harpoonGunObject;
    public GameObject playerHandsObject;
    private HarpoonGun harpoonGun;
    private weaponSelection currentWeapon;
    private PlayerHealth Damage;

    public enum groundType
    {
        Ground,
        Checkpoint,
        Ice,
        Bounce,
        Jumping,
        Swinging,
        Moving
    }

    enum weaponSelection
    {
        fists,
        harpoonGun
    }

    private void Awake()
    {
        
        currentMoveSpeed = moveSpeed;
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentGroundType = groundType.Jumping;
        lookValue = Vector3.zero;
        collectibleCount = new Dictionary<string, int>();
        harpoonGun = harpoonGunObject.GetComponent<HarpoonGun>();
        currentWeapon = weaponSelection.harpoonGun;
        spawnLocation = transform.position;
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

    public void OnSprint(InputValue value)
    {
        currentMoveSpeed =  moveSpeed + (5f * value.Get<float>());
    }

    void OnScroll(InputValue value)
    {
        scrollValue = value.Get<Vector2>();
        if(currentWeapon == weaponSelection.fists && scrollValue.y > 0)
        {
            //Debug.Log("Fist");
            currentWeapon = weaponSelection.harpoonGun;
            playerHandsObject.SetActive(false);
            harpoonGunObject.SetActive(true);
        }
        if (currentWeapon == weaponSelection.harpoonGun && scrollValue.y < 0 && currentGroundType != groundType.Swinging)
        {
            //Debug.Log("Harpoon");
            currentWeapon = weaponSelection.fists;
            harpoonGunObject.SetActive(false);
            playerHandsObject.SetActive(true);
        }
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
                if ((gameObject.GetComponent<SpringJoint>() == null) && (gameObject.GetComponent<ConfigurableJoint>() == null))
                harpoonGun.shootRope();

            break;
        }
    }
    void OnJump(InputValue value)
    {    
        if(currentGroundType == groundType.Swinging)
                {
            harpoonGun.stopRope();
                }
        if (currentGroundType != groundType.Jumping && currentGroundType != groundType.Swinging)
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
        Vector3 velocity = moveDirection * currentMoveSpeed;
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

        if (transform.position.y < -10)
        {
            transform.position = spawnLocation;
        }
        //Debug.Log(currentGroundType);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == 3)
        {
            setGroundType((groundType)Enum.Parse(typeof(groundType), collision.gameObject.tag));
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

        if (collision.gameObject.tag == "Checkpoint")
        {
            spawnLocation = transform.position;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            setGroundType((groundType)Enum.Parse(typeof(groundType), collision.gameObject.tag));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            setGroundType(groundType.Jumping);
        }
    }
}
