using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform playertemp;
    
    void LateUpdate()
    {
        Vector3 newPosition = playertemp.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
