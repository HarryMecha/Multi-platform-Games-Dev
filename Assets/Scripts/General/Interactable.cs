using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject TextBox;
    [SerializeField] GameObject InteractingObject;
    private bool collected;
    [SerializeField] GameObject parentContainer;
    private Interactable[] allInteractables;
    #endregion

    /*
     * This script handles the interactable objects within the scene
     */
    private void Start()
    {
        collected = false;
        allInteractables = parentContainer.GetComponentsInChildren<Interactable>();
    }

    /*
     * This function will show the text object attached to the item when it is looked at by the player.
     */
    public void showInteractText()
    {
        if (!TextBox.activeSelf && !InteractingObject.activeSelf && !collected)
        {
            TextBox.SetActive(true);
        }
    }
    /*
     * This function will hide the text object attached to the item when it is looked away from by the player.
     */
    public void hideInteractText()
    {
            TextBox.SetActive(false);
    }

    /*
     * This function will use the collectible, setting it to inactive within the scene and sets it to collected in the allInteractables list which holds all the interactables in a scene.
     */
    public void useInteractable()
    {
        if (!InteractingObject.activeSelf) {
            if (InteractingObject.name == "CurveHUD")
            {
                InteractingObject.SetActive(!InteractingObject.activeSelf);
            }
        gameObject.SetActive(false);
            foreach (var child in allInteractables)
            {
                collected = true;
            }
    }
    }                                                                           
}
