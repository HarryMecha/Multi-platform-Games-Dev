using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    #region Fields
    private bool notHit;
    #endregion

    private void Start()
    {
        notHit = true;
    }

    /* This piece of code just registers if the attached object has been hit or not by the player
     */
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        notHit = false;
    }

    public bool askIsHit()
    {
        return notHit;
    }
}
