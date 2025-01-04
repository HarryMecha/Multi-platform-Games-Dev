using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using System.Security.Cryptography;
using Unity.VisualScripting;
using System.Linq;

public class EnviromentManager : MonoBehaviour
{
    private Scene currentScene;
    private string currentSceneName;
    private string previousSceneName = null;
    public GameObject Player;
    public PlayerController Controller;
    public PlayerManager Manager;
    private GameObject DialougeCanvas;
    private bool confirmPressed;
    private bool inventoryOpen;
    [SerializeField] private Collectible DivingSuit;
    [SerializeField] private Collectible HarpoonGun;
    public Animator transition;
   [SerializeField] private List<Camera> additionalSceneCameras;
     private Camera playerCamera;



    public enum sceneType
    {
        Tutorial,
        Cutscene,
        Main
    }


    public void onConfirmPress()
    {
        confirmPressed = true;
    }

    public void onInventoryOpen()
    {
        inventoryOpen = !inventoryOpen;
    }


    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    

    IEnumerator ScriptedEvent()
    {
        switch (currentSceneName)
        {
            case ("TutorialScene"):
                previousSceneName = currentSceneName;
                Debug.Log("Cooroutine");
                // Disable CurveHUD
                GameObject curveHUDTransform = GameObject.Find("CurveHUD");
                if (curveHUDTransform != null)
                {
                    curveHUDTransform.gameObject.SetActive(false);
                }
                Controller.setMenuOpen();
                Controller.setTutorialOn();

                GameObject ConsoleFixed = GameObject.Find("ConsoleFixed");
                ConsoleFixed.SetActive(false);

                // Enable Dialogue Panel
                GameObject DialougePanel = DialougeCanvas.transform.GetChild(0).gameObject;
                DialougePanel.SetActive(true);

                // Update Dialogue Text
                AnimatedText DialougePanelText = DialougePanel.transform.GetChild(0).GetComponent<AnimatedText>();
                DialougePanelText.addToItemInfo("Hello --BZZT-- Hello, can you hear me Captain? --BZZT-- [PRESS 'Enter']");
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                // Wait for user to confirm
                
                yield return new WaitUntil(() => confirmPressed);
                confirmPressed = false;

                DialougePanelText.addToItemInfo("Oh good you're awake --BZZT-- I was worried after we hit that last batch of turbulence --BZZT-- it seemed like you hit your head pretty hard --BZZT-- " +
                    "Sorry to say Captain but the submarine's in a bit of disarray but nothing we can't fix. [PRESS 'Enter']");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                confirmPressed = false;
                yield return new WaitUntil(() => confirmPressed);
                confirmPressed = false;

                DialougePanelText.addToItemInfo("Who am I? Wow you must've hit your head quite hard huh? --BZZT-- I am Mari your onboard assistant, now let's get you back on your sea legs, use WASD to move around the ship.");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                Controller.setMenuClosed();

                yield return new WaitUntil(() => (Controller.getMoveValue().x > 0.0) || (Controller.getMoveValue().y > 0.0));
                DialougePanel.SetActive(false);
                yield return new WaitForSeconds(3);
                
                DialougePanel.SetActive(true);
                DialougePanelText.addToItemInfo("Good to see you back on your feet Captain. --BZZT-- Now let's get you suited up, go to your suit locker and put one on.");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                Manager.TakeDamage(40);
                yield return new WaitUntil(() => Manager.isInInventory("Diving Suit"));
                Manager.setFistsEquipped(Manager.isInInventory("Diving Suit"));
                curveHUDTransform.gameObject.GetComponent<AudioSource>().Play();


                DialougePanelText.addToItemInfo("Looking good Captain --BZZT-- Now that that's sorted let's get to fixing that navigational terminal, it seems to be on the fritz, use the scroll wheel to select the fists option, giving it a good" +
                    " smack should sort it out,  use left click to punch it until it starts cooperating again!");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;

                yield return new WaitUntil(() => ConsoleFixed.activeSelf);

                DialougePanelText.addToItemInfo("That seems to have sorted it --BZZT-- Captain, you seem to be a little hurt must've been that hit on the head --BZZT-- why don't you try" +
                    " having a drink that'll sort you right out.");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                yield return new WaitForSeconds(3);

                DialougePanelText.addToItemInfo("Ah seems like it's just out of reach... --BZZT-- hmmmmmm --BZZT-- Oh I know why don't you grab one of those Harpoon guns from the cabinet that'll " +
                    "give you the distance you need!");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;

                yield return new WaitUntil(() => Manager.isInInventory("Harpoon Gun"));
                Manager.setHarpoonEquipped(Manager.isInInventory("Harpoon Gun"));

                DialougePanelText.addToItemInfo("Now move your scroll wheel until you see the Harpoon Gun, then aim well and fire and that can should be yours!");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;

                yield return new WaitUntil(() => Manager.isInInventory("Soda Can"));

                DialougePanelText.addToItemInfo("Great, looks like you've hooked one, make sure to grab both in order to restore your health to full, --BZZT--, now press 'I' to open up your inventory and use those items, to refill your health");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;

                yield return new WaitUntil(() => inventoryOpen);
                DialougePanel.SetActive(false);
                yield return new WaitUntil(() => Manager.currentHealth == 100);
                yield return new WaitUntil(() => inventoryOpen);

                DialougePanel.SetActive(true);
                DialougePanelText.addToItemInfo("I'm sure that hit the spot, now just sit back, relax and enjoy the views of the ocean we should arrive at our destination in --BZZT--  2 hours. [PRESS 'Enter']");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                confirmPressed = false;
                yield return new WaitUntil(() => confirmPressed);
                DialougePanel.SetActive(false);
                confirmPressed = false;
                yield return new WaitForSeconds(10);

                GameObject[] lightList = GameObject.FindGameObjectsWithTag("Light");
                foreach (GameObject light in lightList)
                {
                    light.GetComponent<WarningLight>().warningLightActive();
                }

                DialougePanel.SetActive(true);
                DialougePanelText.addToItemInfo("Uh oh that's not good, looks like punching the navigational system was not a good idea... we're sinking fast --BZZT-- CAPTAIN LOOK OUT WE'RE ABOUT TO --BZZZZZZZT--");
                DialougePanelText.incrimentText();
                DialougePanelText.ActivateText();
                yield return new WaitUntil(() => DialougePanelText.isFinished);
                DialougePanelText.isFinished = false;
                yield return new WaitForSeconds(7);
                DialougePanel.SetActive(false);
                transition.SetTrigger("Start");
                curveHUDTransform.gameObject.SetActive(false);
                yield return new WaitForSeconds(1);
                playerCamera.enabled = false;
                additionalSceneCameras[0].enabled = true;
                Animator animator = GameObject.Find("MiniSub").GetComponent<Animator>();
                animator.SetTrigger("TutorialFinished");
                yield return new WaitForSeconds(1.8f);
                SceneManager.LoadScene("Level101");
                StopCoroutine(ScriptedEvent());
                break;

            case ("Level101"):
                DialougePanel = DialougeCanvas.transform.GetChild(0).gameObject;
                DialougePanelText = DialougePanel.transform.GetChild(0).GetComponent<AnimatedText>();
                DialougePanel.SetActive(false);
                Controller.setMenuClosed();
                if (!Manager.isInInventory("Diving Suit"))
                {
                    Manager.addToInventory(DivingSuit);
                }
                if (!Manager.isInInventory("Harpoon Gun"))
                {
                    Manager.addToInventory(HarpoonGun);
                }
                Manager.setFistsEquipped(Manager.isInInventory("Diving Suit"));
                //Debug.Log(Manager.FistsEquipped);
                Manager.setHarpoonEquipped(Manager.isInInventory("Harpoon Gun"));
                //Debug.Log(Manager.HarpoonEquipped);
                break;

            case ("Level102"):
                DialougePanel = DialougeCanvas.transform.GetChild(0).gameObject;
                DialougePanelText = DialougePanel.transform.GetChild(0).GetComponent<AnimatedText>();
                 DialougePanel.SetActive(false);
                Controller.setMenuClosed();
                Manager.addToInventory(DivingSuit);
                Manager.addToInventory(HarpoonGun);
                Manager.setFistsEquipped(Manager.isInInventory("Diving Suit"));
                //Debug.Log(Manager.FistsEquipped);
                Manager.setHarpoonEquipped(Manager.isInInventory("Harpoon Gun"));
                //Debug.Log(Manager.HarpoonEquipped);
                break;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        Debug.Log(scene.name);
        if (scene.name == "MainMenu")
        {
            StopAllCoroutines();
            return;
        }
        
        if (this.gameObject != null)
        {
            setupPlayer();
            Debug.Log("This");
            StartCoroutine(ScriptedEvent());
        }

    }

    public void unsubscribeSceneManager()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void setupPlayer()
    {
        Debug.Log("Player Setup");
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        Player = GameObject.FindWithTag("Player");
        Controller = Player.GetComponent<PlayerController>();
        Manager = transform.GetComponent<PlayerManager>();
        DialougeCanvas = GameObject.Find("Dialouge Canvas");
        confirmPressed = false;
        inventoryOpen = false;
        additionalSceneCameras = GameObject.FindObjectsByType<Camera>(FindObjectsSortMode.None).ToList<Camera>();
        playerCamera = Player.transform.Find("Main Camera").GetComponent<Camera>();
        additionalSceneCameras.Remove(playerCamera);
        foreach (Camera camera in additionalSceneCameras)
        {
            camera.enabled = false;
        }
        Manager.setupHealthBar();
        Debug.Log(Manager.Inventory);
        foreach (InventoryItem item in Manager.Inventory)
        {
            Debug.Log(item.getCollectible().Name);
        }
    }

}
