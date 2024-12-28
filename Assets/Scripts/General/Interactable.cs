using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject TextBox;
    [SerializeField] GameObject InteractingObject;
    private bool collected;
    [SerializeField] GameObject parentContainer;
    private Interactable[] allInteractables;

    private void Start()
    {
        collected = false;
        allInteractables = parentContainer.GetComponentsInChildren<Interactable>();
    }

    public void showInteractText()
    {
        if (!TextBox.activeSelf && !InteractingObject.activeSelf && !collected)
        {
            TextBox.SetActive(true);
        }
    }
    public void hideInteractText()
    {
            TextBox.SetActive(false);
    }

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
