using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName; // Name of the scene to load
    [SerializeField] private float delay;      // Delay in seconds before changing the scene

    void Start()
    {
        // Invoke the method to change the scene after the specified delay
        Invoke("ChangeScene", delay);
    }

    void ChangeScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}