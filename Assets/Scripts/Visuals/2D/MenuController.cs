using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    private void Start()
    {
        LoadDifficulty();
    }

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

    private void LoadDifficulty()
    {
        switch (GameSettings.Difficulty) 
        {
            default:
            case EDifficulty.Easy:
                easyButton.Select();
                break;
            case EDifficulty.Medium:
                mediumButton.Select(); 
                break;
            case EDifficulty.Hard: 
                hardButton.Select();
                break;
        }
    }
}
