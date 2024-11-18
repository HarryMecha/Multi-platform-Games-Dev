using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector3 initalspawnLocation;
    private Vector3 spawnLocation;
    public Camera PlayerCamera;
    public float cameraSensitivity;
    public float HitDistance;
    public float jumpForce;
    public float bounceForce;
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
    public GameObject HUDCanvas;

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
        currentWeapon = weaponSelection.fists;
        spawnLocation = transform.position;
        initalspawnLocation = transform.position;
    }

    /* setGroundType function is called by this and other scripts in order to set the current groundType of the player object */
    public void setGroundType(groundType type)
    {
        currentGroundType = type;
    }

    /* OnMove function is called when the InputSystem detects WASD key presses, this will just ammend the moveValue field to the direction
     * assigned key that has been pressed which is used when applying force to the player GameObject to simulate movement*/
    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    /* OnLook function is called when the InputSystem detects mouse movement, this will just ammend the moveValue field to the current
     *  position of the mouse on the Game Screen */
    public void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>();
    }

    /* OnSprit function is called when the InputSystem detects the Shift key is pressed, this will just ammend the currentMoveSpeed 
     * increasing the movementSpeed whilst the key is being held which is used when applying force to the player GameObject to simulate movement*/
    public void OnSprint(InputValue value)
    {
        currentMoveSpeed = moveSpeed + (5f * value.Get<float>());
    }

    /* OnScroll function is called when the InputSystem detects the scroll wheel is moved, this will just ammend the current weapon enum 
     * this  will determine which weapon the player is holding, which will have different properties when the Fire key is pressed.*/
    void OnScroll(InputValue value)
    {
        scrollValue = value.Get<Vector2>();
        if (currentWeapon == weaponSelection.fists && scrollValue.y > 0)
        {
            currentWeapon = weaponSelection.harpoonGun;
            playerHandsObject.SetActive(false);
            harpoonGunObject.SetActive(true);
        }
        if (currentWeapon == weaponSelection.harpoonGun && scrollValue.y < 0 && currentGroundType != groundType.Swinging && harpoonGun.isHooked() == false)
        {
            currentWeapon = weaponSelection.fists;
            harpoonGunObject.SetActive(false);
            playerHandsObject.SetActive(true);
        }
    }

    /* OnFire function is called when the InputSystem detects the left mouse is clicked, this will have different effects based on the weapon currently selected,
     * if it is on Fists it will shoot a RayCast detecting if the Object the ray is hitting has a tag of an enemy in which case it will apply damage to that Enemy and
     * if Harpoon Gun is selected it will call a function from the HarpoonGun class attatched to the Player*/
    void OnFire(InputValue value)
    {
        switch (currentWeapon)
        {
            case (weaponSelection.fists):
                Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, HitDistance))
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        hit.transform.GetComponent<EnemyHealth>().TakeDamage(10);
                    }
                }
                break;


            case (weaponSelection.harpoonGun):
                if ((gameObject.GetComponent<SpringJoint>() == null) && (gameObject.GetComponent<ConfigurableJoint>() == null))
                    harpoonGun.shootRope();

                break;
        }
    }

    /* OnJump function is called when the InputSystem detects the spacebar is pressed, it will perform a check to see what the currentGround type is and will
     * perform an action accordingly.*/
    void OnJump(InputValue value)
    {
        if (currentGroundType == groundType.Swinging)
        {
            harpoonGun.stopRope();
            currentGroundType = groundType.Jumping;
        }
        if (currentGroundType != groundType.Jumping && currentGroundType != groundType.Swinging)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    /* OnReset function is called when the InputSystem detects the R key is pressed, this will Reload the scene, effectively resetting the game.*/
    void OnReset(InputValue value)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = PlayerCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = PlayerCamera.transform.right;

        /* These lines apply the movement force to the character */
        Vector3 moveDirection = (cameraForward * moveValue.y + cameraRight * moveValue.x).normalized;
        Vector3 velocity = moveDirection * currentMoveSpeed;
        velocity.y = playerRigidbody.velocity.y; //keeps the y velocity the same, so is only affected by jump
        playerRigidbody.velocity = velocity;
    }

    void Update()
    {
        /* These lines allign the camera and player object to the current position of the mouse */
        cameraRotation.x += lookValue.y * cameraSensitivity;
        cameraRotation.y += lookValue.x * cameraSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(-cameraRotation.x, 0.0f, 0.0f);
        playerRigidbody.transform.localRotation = Quaternion.Euler(0f, cameraRotation.y, 0.0f);

        /*This is the condition which determines whether the character is out of bounds, this will result the the Player object returning to the last
         * checkpoint they visited */
        if (transform.position.y < -10)
        {
            transform.position = spawnLocation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /* Sets the currentGround type to the tag coiincides with when the player is on the ground layer. */
        if (currentGroundType != groundType.Swinging)
        {

            if (collision.gameObject.layer == 3)
            {
                setGroundType((groundType)Enum.Parse(typeof(groundType), collision.gameObject.tag));
            }
        }

        /* This switch case will determine special cases for player collision determined by object tag, collectibles will be added to players Dictionary field,
         * Checkpoints will ammend the players spawning location, bounce will apply a upwards force to the player. */
        switch (collision.gameObject.tag)
        {
            case ("Collectible"):
                if (collectibleCount.ContainsKey((string)collision.gameObject.name))
                {
                    collectibleCount[collision.gameObject.name] += 1;
                }
                else collectibleCount.Add(collision.gameObject.name, 1);
                Destroy(collision.gameObject);
                if (collision.gameObject.name == "Win")
                {
                    WinCondition();
                }
                break;

            case ("Checkpoint"):
                spawnLocation = transform.position;
                break;

            case ("Bounce"):

                playerRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);

                break;
        }

    }


    private void OnCollisionExit(Collision collision)
    {
        /* Sets the currentGround type jumping when not on the ground */
        if (currentGroundType != groundType.Swinging)
        {
            if (collision.gameObject.layer == 3)
            {
                setGroundType(groundType.Jumping);
            }
        }
    }

    /* Resets the players position to the inital spawn location */
    private void resetPosition()
    {
        spawnLocation = initalspawnLocation;
    }

    /* Returns the current ground type the player is standing on. */
    public groundType GetGroundType()
    {
        return currentGroundType;
    }

    /* Changes UI object when player reaches win condition */
    private void WinCondition()
    {
        HUDCanvas.transform.GetChild(0).gameObject.SetActive(false);
        HUDCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }
}

