using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Collectible collectible;
    [SerializeField] private int count;

    public InventoryItem(Collectible collectible, int count)
    {
        this.collectible = collectible;
        this.count = count;
    }

    public Collectible getCollectible() { return collectible; }
    public int  getCount() { return  count; }

    public void addToCount() {  count++; }

    public void removeFromCount() { count--; }

    public void setCollectible(Collectible collectibleToSet)
    {
        collectible = collectibleToSet;
    }

    public void setCount(int countToSet) { count = countToSet; }


}
