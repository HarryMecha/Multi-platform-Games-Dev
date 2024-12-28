using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    /*
     * LevelChanger manages the fade screen transition when the scene is changed.
     */

    #region Fields
    public Animator animator;
    private string targetScene;
    #endregion

    /*
     * ScreenTransition() takes a sceneName as input, runs the animation taht fades the screen to black and then loads the new scene.
     */
    public void ScreenTransition(string sceneName)
    {
        targetScene = sceneName;
        animator.SetTrigger("Fade Out");
    }

    /*
     * OnFadeComplete() is a method used as an event in the animtor that runs when the Fade Out animation is complete.
     */
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(targetScene);
    }

}