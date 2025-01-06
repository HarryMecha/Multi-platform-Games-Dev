using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    #region Fields
    
    [SerializeField] GameObject LightSource;
    #endregion

    /* This function is simply an activator for the lightsources letting them play audio and animate
     */
    public void warningLightActive()
    {
       LightSource.GetComponent<AudioSource>().Play();
       LightSource.GetComponent<Animator>().enabled = true;
    }
}
