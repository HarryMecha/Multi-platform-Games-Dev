using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyHandler : MonoBehaviour
{

    private string SceneName;
    public Difficulty difficulty;

    public enum Difficulty
    {
        Easy,
        Hard
    }

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
