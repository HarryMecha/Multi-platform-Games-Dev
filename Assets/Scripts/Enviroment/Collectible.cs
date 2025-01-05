
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
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
   

    //Hidden Field is Useable is false
    [HideInInspector] public float HealthIncrease;


    public Collectible(string name, string description, Sprite sprite, bool useable)
    {
        Name = name;
        Description = description;
        InventoryPicture = sprite;
        isUseable = useable;
    }
}
