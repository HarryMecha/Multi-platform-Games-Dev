using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyHandler : MonoBehaviour
{
    #region Fields
    private string SceneName;
    public Difficulty difficulty;

    public enum Difficulty
    {
        Easy,
        Hard
    }
    #endregion


    /*
     * This script is just a getter for the difficulty in menu and then returns that value to the EnviromentManager which will dictate specific conditions in switch cases within other scipts
     */
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void setDifficultyEasy()
    {
        difficulty = Difficulty.Easy;
    }

    public void setDifficultyHard()
    {
        difficulty = Difficulty.Hard;
    }

    public void setSceneName(string Name)
    {
        SceneName = Name;
    }

    public Difficulty getDifficulty()
    {
        return difficulty;
    }

    public void changeScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
