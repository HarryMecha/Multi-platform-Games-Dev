using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressionBarrier : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject TextBox;
    private bool closed;
    [SerializeField] private string collectibleNeeded;
    [SerializeField] private int amountNeeded;
    [SerializeField] private PlayerManager playerManager;
    #endregion

    private void Start()
    {
        closed = true;
        playerManager = GameObject.Find("EnviromentManager").GetComponent<PlayerManager>();
    }

    /* This piece of code just checks if the required amount of a certain item has been collected by the player, it disable it's collider if it has
     * and display to the player that they still need to collect a certain number if they haven't
     */
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
