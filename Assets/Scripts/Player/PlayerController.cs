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
    #region Fields
    private Vector3 initalspawnLocation;
    private Vector3 spawnLocation;
    public Camera PlayerCamera;
    public float cameraSensitivity;
    private float initialCameraSensitivity;
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
    public GameObject harpoonGunObject;
    public GameObject playerHandsObject;
    private HarpoonGun harpoonGun;
    private weaponSelection currentWeapon;
    private PlayerHealth Damage;
    public GameObject HUDCanvas;
    private GameObject EnviromentManager;
    [SerializeField] public bool menuOpen;
    private bool tutorial;
    private GameObject lastInteractableHit = null;
    private GameObject lastEnemyHit = null;
    private GameObject lastBarrierHit = null;
    #endregion

    public enum groundType
    {
        Ground,
        Checkpoint,
        OutOfBounds,
        Ice,
        Bounce,
        Jumping,
        Swinging,
        Moving
    }

    enum weaponSelection
    {
        idle,
        fists,
        harpoonGun
    }


    private void Awake()
    {
        EnviromentManager = GameObject.Find("EnviromentManager");
        currentMoveSpeed = moveSpeed;
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentGroundType = groundType.Jumping;
        lookValue = Vector3.zero;
        harpoonGun = harpoonGunObject.GetComponent<HarpoonGun>();
        currentWeapon = weaponSelection.idle;
        spawnLocation = transform.position;
        initialCameraSensitivity = cameraSensitivity;
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
        if (!menuOpen)
        {
            scrollValue = value.Get<Vector2>();
            switch (currentWeapon)
            {
                case (weaponSelection.idle):
                    if (scrollValue.y > 0 && EnviromentManager.GetComponent<PlayerManager>().FistsEquipped)
                    {
                        currentWeapon = weaponSelection.fists;
                        playerHandsObject.SetActive(true);
                        harpoonGunObject.SetActive(false);
                    }
                    if (scrollValue.y < 0 && EnviromentManager.GetComponent<PlayerManager>().HarpoonEquipped)
                    {
                        currentWeapon = weaponSelection.harpoonGun;
                        playerHandsObject.SetActive(false);
                        harpoonGunObject.SetActive(true);
                    }
                    break;
                case (weaponSelection.fists):
                    if (scrollValue.y > 0 && EnviromentManager.GetComponent<PlayerManager>().HarpoonEquipped)
                    {
                        currentWeapon = weaponSelection.harpoonGun;
                        playerHandsObject.SetActive(false);
                        harpoonGunObject.SetActive(true);
                    }
                    if (scrollValue.y < 0)
                    {
                        currentWeapon = weaponSelection.idle;
                        playerHandsObject.SetActive(false);
                        harpoonGunObject.SetActive(false);
                    }
                    break;
                case (weaponSelection.harpoonGun):
                    if (scrollValue.y < 0 && currentGroundType != groundType.Swinging && harpoonGun.isHooked() == false && EnviromentManager.GetComponent<PlayerManager>().FistsEquipped)
                    {
                        currentWeapon = weaponSelection.fists;
                        harpoonGunObject.SetActive(false);
                        playerHandsObject.SetActive(true);
                    }
                    if (scrollValue.y > 0 && currentGroundType != groundType.Swinging && harpoonGun.isHooked() == false)
                    {
                        currentWeapon = weaponSelection.idle;
                        harpoonGunObject.SetActive(false);
                        playerHandsObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    /* OnFire function is called when the InputSystem detects the left mouse is clicked, this will have different effects based on the weapon currently selected,
     * if it is on Fists it will shoot a RayCast detecting if the Object the ray is hitting has a tag of an enemy in which case it will apply damage to that Enemy and
     * if Harpoon Gun is selected it will call a function from the HarpoonGun class attatched to the Player*/
    void OnFire(InputValue value)
    {
        if (menuOpen == false)
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
        if (currentGroundType != groundType.Jumping && currentGroundType != groundType.Swinging && (menuOpen == false && tutorial == false))
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    /* OnReset function is called when the InputSystem detects the R key is pressed, this will Reload the scene, effectively resetting the game.*/
  
    void OnEscape(InputValue value)
    {
        if (HUDCanvas.activeInHierarchy && menuOpen == false)
        {
            HUDCanvas.GetComponent<PauseMenu>().ActivateMenu();
        }
    }

    /* OnConfirm function is called when the InputSystem detects the Enter key is pressed, this is used to progress dialouge in the tutorial scene.*/
    public void OnConfirm(InputValue value)
    {
        EnviromentManager.GetComponent<EnviromentManager>().onConfirmPress();
    }

    /* OnInteract function is called when the InputSystem detects the E key is pressed, this will check if the object the player is looking at holds
     * a collectible script within it and if it does it adds that item to the players inventory and will also use the interactable whether it is a collectible or not.*/
    public void OnInteract(InputValue value)
    {
        if(lastInteractableHit != null)
        {
            Transform PUC = lastInteractableHit.transform.Find("PopUp Canvas");
            if (lastInteractableHit.GetComponent<Collectible>() != null && PUC.Find("Text (TMP)").gameObject.activeSelf)
            {
                Collectible collectible = lastInteractableHit.GetComponent<Collectible>();
                EnviromentManager.GetComponent<PlayerManager>().addToInventory(collectible);
            }
            lastInteractableHit.GetComponent<Interactable>().useInteractable();
            lastInteractableHit = null;
        }
    }

    /* OnInventory function is called when the InputSystem detects the I key is pressed, this will lock open the inventory menu.*/
    public void OnInventory(InputValue value)
    {
        EnviromentManager.GetComponent<EnviromentManager>().onInventoryOpen();
        if (HUDCanvas.activeInHierarchy)
        {
            HUDCanvas.GetComponent<PauseMenu>().ActivateInventoryMenu();
        }
    }


    private void FixedUpdate()
    {
        Vector3 cameraForward = PlayerCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = PlayerCamera.transform.right;

        if (menuOpen == false)
        {
            /* These lines apply the movement force to the character */
            Vector3 moveDirection = (cameraForward * moveValue.y + cameraRight * moveValue.x).normalized;
            Vector3 velocity = moveDirection * currentMoveSpeed;
            velocity.y = playerRigidbody.velocity.y; //keeps the y velocity the same, so is only affected by jump
            playerRigidbody.velocity = velocity;
        }

    }

    void Update()
    {
        if (menuOpen == false)
        {
            /* These lines allign the camera and player object to the current position of the mouse */
            cameraRotation.x += lookValue.y * cameraSensitivity;
            cameraRotation.y += lookValue.x * cameraSensitivity;
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);
            PlayerCamera.transform.localRotation = Quaternion.Euler(-cameraRotation.x, 0.0f, 0.0f);
            playerRigidbody.transform.localRotation = Quaternion.Euler(0f, cameraRotation.y, 0.0f);
        }

        lookingAtInteractable();
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
         * Checkpoints will ammend the players spawning location, bounce will apply a upwards force to the player, collectible will add the current collided item
         * into the players inventory and then set it's collider to false so it can no longer be interacted with. */
        switch (collision.gameObject.tag)
        {
            case ("Collectible"):
                EnviromentManager.GetComponent<PlayerManager>().addToInventory(collision.transform.GetComponent<Collectible>());
                collision.transform.GetChild(0).gameObject.SetActive(false);
                if (collision.transform.GetComponent<BoxCollider>())
                    collision.transform.GetComponent<BoxCollider>().enabled = false;
                break;

            case ("Checkpoint"):
                spawnLocation = transform.position;
                break;

            case ("Bounce"):

                playerRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);

                break;
            /*This is the condition which determines whether the character is out of bounds, this will result the the Player object returning to the last
             * checkpoint they visited */
            case ("OutOfBounds"):
                transform.position = spawnLocation;
                EnviromentManager.GetComponent<PlayerManager>().TakeDamage(10);
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
    /* Setter method for the menuOpen boolean, determines if the menu is open which will lock off certain player functions*/
    public void setMenuOpen()
    {
        Debug.Log("Menu Open");
        menuOpen = true;
        Debug.Log(menuOpen);
    }

    /* Setter method for the menuOpen boolean, determines if the menu is closed which will lock off certain player functions*/
    public void setMenuClosed()
    {
        Debug.Log("Menu Closed");
       
        menuOpen = false;
    }

    /* Setter method for the tutorial boolean, determines if the menu is open which will lock off certain player functions*/
    public void setTutorialOn()
    {
        tutorial = true;
    }

    /* Setter method for the tutorial boolean, determines if the menu is open which will lock off certain player functions*/
    public void setTutorialOff()
    {
        tutorial = false;
    }

    /* Getter method for the current move value*/
    public Vector2 getMoveValue()
    {
        return moveValue;
    }

    /* Getter method for the current input sensitivity*/
    public float getInputSensitivity()
    {
        return cameraSensitivity;
    }

    /* Setter method for the current input sensitivity*/
    public void setInputSensitivity(float sensitivity)
    {
        cameraSensitivity = initialCameraSensitivity * sensitivity;
        //Debug.Log(cameraSensitivity);
    }

    /* The lookingAtInteractable function is called in Update(), it shoots a ray from the middle of the viewport and detects the tag of that GameObject
     * it will then display the text object or healthbar attached to that object to signal to the player they are currently looking at it, it will then also
     * close that menu when the player looks away or at another object.
     */
    private void lookingAtInteractable()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.transform.tag == "Interactable")
            {
                if (hit.transform.gameObject != lastInteractableHit)
                {
                    if (lastInteractableHit != null)
                    {
                        lastInteractableHit.GetComponent<Interactable>().hideInteractText();
                    }
                    lastInteractableHit = hit.transform.gameObject;
                    lastInteractableHit.GetComponent<Interactable>().showInteractText();
                }
            }
            if (hit.transform.tag == "Enemy")
            {
                if (hit.transform.gameObject != lastEnemyHit)
                {
                    if (lastEnemyHit != null)
                    {
                        lastEnemyHit.GetComponent<EnemyHealth>().hideHealthBar();
                    }
                    lastEnemyHit = hit.transform.gameObject;
                    lastEnemyHit.GetComponent<EnemyHealth>().showHealthBar();
                }
            }
            if (hit.transform.tag == "Barrier")
            {
                if (hit.transform.gameObject != lastBarrierHit)
                {
                    if (lastBarrierHit != null)
                    {
                        lastBarrierHit.GetComponent<ProgressionBarrier>().hideInteractText();
                    }
                    lastBarrierHit = hit.transform.gameObject;
                    lastBarrierHit.GetComponent<ProgressionBarrier>().showInteractText();
                }
            }
        }
        else
        {
             if(lastInteractableHit != null)
             {
                lastInteractableHit.GetComponent<Interactable>().hideInteractText();
             }

            if (lastEnemyHit != null)
            {
                lastEnemyHit.GetComponent<EnemyHealth>().hideHealthBar();
            }
            lastEnemyHit = null;

            if (lastBarrierHit != null)
            {
                lastBarrierHit.GetComponent<ProgressionBarrier>().hideInteractText();
            }
            lastBarrierHit = null;
        }
    }

}

