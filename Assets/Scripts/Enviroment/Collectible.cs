using UnityEngine;

public class Collectible : MonoBehaviour
{
    #region Fields
    public string Name;
    public string Description;
    public Sprite InventoryPicture;
    public bool isUseable;
    public bool isEquippable;
    [SerializeField] public equipType EquipType;
   
    public enum equipType
    {
        none,
        harpoon,
        divingsuit
    }
   


    [HideInInspector] public float HealthIncrease;
 #endregion

    //Constructor for collectible object
    public Collectible(string name, string description, Sprite sprite, bool useable)
    {
        Name = name;
        Description = description;
        InventoryPicture = sprite;
        isUseable = useable;
    }
}
