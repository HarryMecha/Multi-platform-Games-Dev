using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BGMChanger : MonoBehaviour
{
    /*
     * BGMChanger is a script that controls the background music of scenes.
     */

    #region Fields
    AudioSource BGM;
    public AudioClip[] audioClips;
    #endregion

    void Start()
    {
        BGM = GetComponent<AudioSource>();
        BGM.Play();
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }


    /*
     * ChangedActiveScene is a function in the UnityEngine.SceneManagement library that runs when a new scene is loaded. the case statement then takes the name of the newScene and plays
     * the audio clip at the corresponding array index.
     */
    public void ChangedActiveScene(Scene prevScene, Scene newScene)
    {
        BGM.Stop();
        switch (newScene.name)
        {
            case ("StartScene"):
                BGM.clip = audioClips[0];
                BGM.Play();
                break;
            case ("GarageScene"):
                BGM.clip = audioClips[1];
                BGM.Play();
                break;
            case ("OverworldScene"):
                BGM.clip = audioClips[2];
                BGM.Play();
                break;
            case ("RaceScene"):
                BGM.clip = audioClips[3];
                BGM.Play();
                break;


        }


    }
}
