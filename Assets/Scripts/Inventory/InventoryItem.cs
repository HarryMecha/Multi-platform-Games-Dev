using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    #region Fields
    private Collectible collectible;
    private int count;
    #endregion

    public InventoryItem(Collectible collectibleToAdd, int count)
    {
        collectible = collectibleToAdd;
        this.count = count;
    }
    /*
     * Getter and Setter functions for all the fields in InventoryItem
     */

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
