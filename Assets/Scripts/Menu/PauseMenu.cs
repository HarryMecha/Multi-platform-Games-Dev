using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /*
     * PauseMenu is a script that handles the pause menu
     */

    #region Fields
    private GameObject Player;
    private PlayerController Controller;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private GameObject deathMenu;
    #endregion

    private void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        deathMenu.SetActive(false);
        Player = GameObject.FindWithTag("Player");
        Controller = Player.GetComponent<PlayerController>();
        //optionsMenu.SetActive(false);
    }

    /*
     * CloseMenu() close the pause menu GUI element.
     */

    public void ActivateMenu()
    {
        if (pauseMenu.activeSelf)
        {
            Controller.setMenuClosed();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);

        }
        else 
        { 
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Controller.setMenuOpen();
         }       
    }

    public void ActivateInventoryMenu()
    {
        if (inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(false);
            Controller.setMenuClosed();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            

        }
        else
        {
            inventoryMenu.SetActive(true);
            inventoryMenu.GetComponent<InventoryMenuManager>().inventorySetup();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Controller.setMenuOpen();
        }
    }

    public void ActivateOptionsMenu()
    {
        if (optionsMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);

        }
        else
        {
            optionsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }

    public void ActivateDeathMenu()
    {
        if (deathMenu.activeSelf)
        {
            Controller.setMenuClosed();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            deathMenu.SetActive(false);

        }
        else
        {
            deathMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Controller.setMenuOpen();
        }
    }

    public void resetButton()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void quitButton()
    {
        StopAllCoroutines();
        GameObject enviromentManager = GameObject.Find("EnviromentManager");
        enviromentManager.GetComponent<EnviromentManager>().unsubscribeSceneManager();
        Destroy(enviromentManager);
        SceneManager.LoadScene("MainMenu");
    }


    public void CloseMenuViaButton()
    {
        Controller.setMenuClosed();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
    }

}
