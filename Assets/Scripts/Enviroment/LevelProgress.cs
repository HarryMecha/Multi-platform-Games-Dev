using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private string LevelName;
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(LevelName);
        StopAllCoroutines();
    }
}
