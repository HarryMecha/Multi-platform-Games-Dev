using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{

    private float startTime;
    private bool playerStanding = false;
    private bool colliderDestroyed = false;
    private float currentOpacity = 1.0f;
    private MeshRenderer meshRenderer;
    private float opacityLoss = 0.3f;
    private Collider collider;
    private Color currentColor;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (playerStanding)
        {
            if (currentOpacity > 0)
            {
                currentOpacity -= opacityLoss * Time.deltaTime;
                currentColor = meshRenderer.material.color;
                currentColor.a = currentOpacity;
            }
            else
            {
                playerStanding = false;
                colliderDestroyed = true;
                collider.enabled = false;
                
            }
        }
        else
        {
            if (currentOpacity < 1.0f)
            {
                currentOpacity += opacityLoss * Time.deltaTime;
                currentColor = meshRenderer.material.color;
                currentColor.a = currentOpacity;

            }
            else
            {
                colliderDestroyed = false;
                collider.enabled = true;
            }
        }

        meshRenderer.material.color = currentColor;


    }

    private void OnCollisionEnter(Collision collision)
    {
        startTime = Time.deltaTime;
        playerStanding = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("not standing");
        playerStanding = false;
    }
}
