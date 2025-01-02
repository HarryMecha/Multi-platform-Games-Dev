using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Collectible> Items = new List<Collectible>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Collectible item)
    {
        Items.Add(item);
    }

    public void Remove(Collectible item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (var items in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var Icon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text= items.itemName;
            Icon.sprite= items.Icon;
        }
    }
}
