using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /*
     * PauseMenu is a script that handles the pause menu
     */

    #region Fields
    public GameObject pauseMenu;
    #endregion
    
    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    /*
     * CloseMenu() close the pause menu GUI element.
     */
    public void ActivateMenu()
    {
        if (pauseMenu.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);

        }
        else { 
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
         }       
    }
    public void CloseMenuViaButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
    }

}
