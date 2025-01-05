using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressionBarrier : MonoBehaviour
{
    [SerializeField] GameObject TextBox;
    private bool closed;
    [SerializeField] private string collectibleNeeded;
    [SerializeField] private int amountNeeded;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        closed = true;
        playerManager = GameObject.Find("EnviromentManager").GetComponent<PlayerManager>();
    }

    public void showInteractText()
    {
        if (!TextBox.activeSelf)
        {
            if (playerManager.getItemCount(collectibleNeeded) < amountNeeded)
            {
                TextBox.SetActive(true);
                TextBox.GetComponent<TextMeshProUGUI>().text = "Found " + playerManager.getItemCount(collectibleNeeded) + "/" + amountNeeded +
                    "<br>" + collectibleNeeded + "<br>Found";
            }
            else
            {
                transform.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
    public void hideInteractText()
    {
        TextBox.SetActive(false);
    }
}
