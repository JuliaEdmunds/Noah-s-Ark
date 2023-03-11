using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void SetDifficulty(EDifficulty difficulty)
    {
        GameSettings.Difficulty = difficulty;
    }

    public void SetDifficultyEasy() => SetDifficulty(EDifficulty.Easy);

    public void SetDifficultyMedium() => SetDifficulty(EDifficulty.Medium);

    public void SetDifficultyHard() => SetDifficulty(EDifficulty.Hard);

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); 
#endif
    }
}
