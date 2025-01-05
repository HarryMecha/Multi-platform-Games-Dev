using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private Collectible collectible;
    private int count;

    public InventoryItem(Collectible collectibleToAdd, int count)
    {
        collectible = collectibleToAdd;
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
