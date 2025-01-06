using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{
    #region Fields
    [SerializeField] private string LevelName;
    #endregion
    /*
     * Code simply stops all couroutines and changes the level
     */
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(LevelName);
        StopAllCoroutines();
    }
}
