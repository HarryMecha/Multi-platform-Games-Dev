using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private List<GameObject> InventorySlots;
    
    [SerializeField] private GameObject HighLightedItem;
    private PlayerManager playerManager;
    private List<InventoryItem> Inventory;
    private GameObject selectedObject;
    private Collectible swappingObject1;
    private Collectible swappingObject2;
    #endregion

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        playerManager = GameObject.Find("EnviromentManager").GetComponent<PlayerManager>();
       Inventory = playerManager.Inventory;
        selectedObject = new GameObject();
        slotSelected(selectedObject);
    }

    /*
     * This code setsup the GUI for the inventory menu, first it checks the current size of the inventory, it then populates this many of the inventory slots inventory item components, setting the rest to null.
     * It then goes through this list checks each of the slots and edits the displayed values based on the currently populated, or unpopulated, invetory item component, it's name, description, image and current count.
     * these are set to empty if no item exists.
     */
    public void inventorySetup()
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            Debug.Log(Inventory.Count);
            if (i < Inventory.Count)
            {
                InventorySlots[i].GetComponent<InventoryItem>().setCollectible(Inventory[i].getCollectible());
                Debug.Log(Inventory[i].getCollectible());
                InventorySlots[i].GetComponent<InventoryItem>().setCount(Inventory[i].getCount());
            }
            else
            {
                InventorySlots[i].GetComponent<InventoryItem>().setCollectible(null);
                InventorySlots[i].GetComponent<InventoryItem>().setCount(0);
            }
        }

        foreach (GameObject slot in InventorySlots)
        {
            if(slot.GetComponent<InventoryItem>().getCollectible() != null)
            {
                Transform slotPanel = slot.transform.Find("InventorySlot");
                Transform slotImage = slotPanel.transform.Find("InventoryImage");
                Color color = slotImage.GetComponent<Image>().color;
                color.a = 1;
                slotImage.GetComponent<Image>().color = color;
                slotImage.GetComponent<Image>().sprite = slot.GetComponent<InventoryItem>().getCollectible().InventoryPicture;
                Transform countPanel = slotPanel.gameObject.transform.Find("CountPanel");
                color = countPanel.GetComponent<Image>().color;
                color.a = 1;
                countPanel.GetComponent<Image>().color = color;
                Transform countText = countPanel.gameObject.transform.Find("CountText");
                countText.GetComponent<TextMeshProUGUI>().text = slot.GetComponent<InventoryItem>().getCount().ToString();
            }
            else
            {
                Transform slotPanel = slot.transform.Find("InventorySlot");
                Transform slotImage = slotPanel.transform.Find("InventoryImage");
                Color color = slotImage.GetComponent<Image>().color;
                color.a = 0;
                slotImage.GetComponent<Image>().color = color;
                Transform countPanel = slotPanel.gameObject.transform.Find("CountPanel");
                color = countPanel.GetComponent<Image>().color;
                color.a = 0;
                countPanel.GetComponent<Image>().color= color;
                Transform countText = countPanel.gameObject.transform.Find("CountText");
                countText.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
    /*
     * This code will be run when the button attached to each of the slots is pressed, this will open up the inventories side menu and populate it with information about the item in that slot.
     * It will also activate or deactive certain buttons dependant on the type of item it is, useable and equippable items.
     * However if the move item button has been pressed it considers this button press as a selection of a second item and will call the swapItems function in playerMnager which swap their positions in the inventory
     * this will be graphically displayed to the user.
     */
    public void slotSelected(GameObject selectedSlot)
    {
        selectedObject = selectedSlot;
        if (selectedSlot.gameObject.GetComponent<InventoryItem>() == null)
        {
            HighLightedItem.transform.parent.gameObject.SetActive(false);
            return;
        }
        if (selectedSlot.gameObject.GetComponent<InventoryItem>().getCollectible() != null)
        {
            HighLightedItem.transform.parent.gameObject.SetActive(true);
            Collectible selectedCollectible = selectedSlot.gameObject.GetComponent<InventoryItem>().getCollectible();
            HighLightedItem.transform.Find("Item Description").GetComponent<TextMeshProUGUI>().text 
            = selectedCollectible.Description;
            HighLightedItem.transform.Find("Item Name").GetComponent<TextMeshProUGUI>().text
            = selectedCollectible.Name;
            HighLightedItem.transform.Find("HighlightedImage").GetComponent<Image>().sprite
            = selectedCollectible.InventoryPicture;
            if (selectedCollectible.isUseable)
            {
                HighLightedItem.transform.Find("Use Item Button").gameObject.SetActive(true);
            }
            else
            {
                HighLightedItem.transform.Find("Use Item Button").gameObject.SetActive(false);
            }
            if (selectedCollectible.isEquippable)
            {
                switch (selectedCollectible.EquipType)
                {
                    case (Collectible.equipType.harpoon):
                        if (playerManager.currentHarpoonEquipped == selectedCollectible.Name)
                        {
                            //Debug.Log("Hello");
                            HighLightedItem.transform.Find("Unequip Item Button").gameObject.SetActive(true);
                            HighLightedItem.transform.Find("Equip Item Button").gameObject.SetActive(false);
                        }
                        else
                        {
                            //Debug.Log("Hello2");
                            HighLightedItem.transform.Find("Equip Item Button").gameObject.SetActive(true);
                            HighLightedItem.transform.Find("Unequip Item Button").gameObject.SetActive(false);
                        }
                        break;

                }
            }
            else
            {
                HighLightedItem.transform.Find("Equip Item Button").gameObject.SetActive(false);
                HighLightedItem.transform.Find("Unequip Item Button").gameObject.SetActive(false);
            }

            if (swappingObject1 != null && selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible() != swappingObject1)
            {
                swappingObject2 = selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible();
                playerManager.swapItems(swappingObject1, swappingObject2);
                inventorySetup();
                swappingObject1 = null;
                swappingObject2 = null; 
            }


        }
        else
        {
            HighLightedItem.transform.parent.gameObject.SetActive(false);
        }
    }
    /*
     * This code will be run when a useable item is selected and the Use Item Button is pressed, it will then call the useItem function in the playerManager and will then refresh
     * the GUI to reflect a decrease of an items count or removal of that item from the inventory
     */

    public void useSelectedObject()
    {
        if (selectedObject.gameObject.GetComponent<InventoryItem>() == null)
        {
            return;
        }
        if (selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible() != null)
        {
            playerManager.useItem(selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible());
            inventorySetup();
            slotSelected(selectedObject);

        }
    }

    /*
     * This code will be run when a equippable item is selected and the Use Item Button is pressed, it will then call the equipItem function in the playerManager and will then refresh
     * the GUI to set the equip button to unequip
     */
    public void equipSelectedObject()
    {
        if (selectedObject.gameObject.GetComponent<InventoryItem>() == null)
        {
            return;
        }
        if (selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible() != null)
        {
            playerManager.equipItem(selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible());
            inventorySetup();
            slotSelected(selectedObject);

        }
    }

    /*
     * This code will be run when a equippable item is selected and the Use Item Button is pressed, it will then call the unequipItem function in the playerManager and will then refresh
     * the GUI to set the equip button to equip
     */
    public void unequipSelectedObject()
    {
        if (selectedObject.gameObject.GetComponent<InventoryItem>() == null)
        {
            return;
        }
        if (selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible() != null)
        {
            playerManager.unequipItem(selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible());
            inventorySetup();
            slotSelected(selectedObject);

        }
    }

    /*
     * This code will be run when the Move Item Button is pressed, it will set the currently selected item as the first of two items required to switch their places
     */
    public void moveSelectedObject()
    {
        if (selectedObject.gameObject.GetComponent<InventoryItem>() == null)
        {
            return;
        }
        if (selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible() != null)
        {
            if (swappingObject1 == null)
            {
                swappingObject1 = selectedObject.gameObject.GetComponent<InventoryItem>().getCollectible();
            }

        }
    }


}
