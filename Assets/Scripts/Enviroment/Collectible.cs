/* ACKNOWLEDGMENTS
 * Code has been modified from Player script found at: https://www.youtube.com/watch?v=H3pCcKnBRHw&ab_channel=QuickDev
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [CustomEditor(typeof(Collectible))]

    public Collectible(string name, string description, Sprite sprite, bool useable)
    {
        Name = name;
        Description = description;
        InventoryPicture = sprite;
        isUseable = useable;
    }

    public class Collectible_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (Collectible)target;

            script.Name = EditorGUILayout.TextField("Name",script.Name);
            script.Description = EditorGUILayout.TextField("Description", script.Description);
            script.InventoryPicture = EditorGUILayout.ObjectField("Inventory Picture", script.InventoryPicture, typeof(Sprite), true) as Sprite;
            script.isUseable = EditorGUILayout.Toggle("is Usable?", script.isUseable);

            if (script.isUseable == false)
            {
                return;
            }
            else
            {

                script.HealthIncrease = EditorGUILayout.FloatField("Health Increase", script.HealthIncrease);
            }

        }

    }
}
