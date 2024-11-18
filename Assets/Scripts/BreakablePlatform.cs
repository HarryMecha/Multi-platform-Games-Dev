using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BreakablePlatform : MonoBehaviour
{

    private float startTime;
    private bool playerStanding = false;
    private float currentOpacity = 1.0f;
    private MeshRenderer meshRenderer;
    private float opacityLoss = 0.7f;
    public Collider platformCollider;
    private Color currentColor;

    private void Awake()
    {
        platformCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        currentColor = meshRenderer.material.color;
        currentColor.a = currentOpacity;
        meshRenderer.material.color = currentColor;
    }

    private void Update()
    {
        /*If Player is standing on the platform it will slowly start to lose opacity until the vale hits zero, wherein which the collider is disabled
         * causing the player object to fall through.*/
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
                platformCollider.enabled = false;
            }
        }
        /*If the player is no longer standing on the platform it will regain it's opacity*/
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
                platformCollider.enabled = true;
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
        playerStanding = false;
    }
}
