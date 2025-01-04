using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> InventorySlots;
    
    [SerializeField] private GameObject HighLightedItem;
    private PlayerManager playerManager;
    private List<InventoryItem> Inventory;
    private GameObject selectedObject;
    private Collectible swappingObject1;
    private Collectible swappingObject2;

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
                            Debug.Log("Hello");
                            HighLightedItem.transform.Find("Unequip Item Button").gameObject.SetActive(true);
                            HighLightedItem.transform.Find("Equip Item Button").gameObject.SetActive(false);
                        }
                        else
                        {
                            Debug.Log("Hello2");
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
