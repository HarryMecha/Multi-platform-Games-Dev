using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager instance;

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this GameObject
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign the current instance and make it persist across scenes
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this GameObject is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find all instances of the SingletonManager in the scene
        var instances = Object.FindObjectsByType<SingletonManager>(FindObjectsSortMode.None);

        // Destroy any duplicates (instances other than this one)
        foreach (var obj in instances)
        {
            if (obj != this)
            {
                Destroy(obj.gameObject);
            }
        }
    }
}
