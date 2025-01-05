using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    private bool notHit;

    private void Start()
    {
        notHit = true;
    }

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
