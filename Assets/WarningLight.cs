using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    [SerializeField] GameObject LightSource;
    
    public void warningLightActive()
    {
       LightSource.GetComponent<AudioSource>().Play();
       LightSource.GetComponent<Animator>().enabled = true;
    }
}
