using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartANewDive()
    {
        Debug.Log("StartANewDive button clicked!");
        SceneManager.LoadScene("TutorialScene");
    }


    public void Quit()
    {
        Application.Quit();
    }
}
