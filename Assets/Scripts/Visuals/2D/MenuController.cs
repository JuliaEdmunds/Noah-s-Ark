using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField, Header("Sliders")] private Slider m_NumAnimals;
    [SerializeField] private Slider m_NumLifelines;
    [SerializeField] private TextMeshProUGUI m_NumAnimalsText;
    [SerializeField] private TextMeshProUGUI m_NumLifelinesText;
    [SerializeField, Header("Game Mode")] private Toggle m_2DToggle;
    [SerializeField] private Toggle m_3DToggle;

    private void Start()
    {
        LoadDifficulty();
    }

    public void OnAnimalSliderChanged(float value)
    {
        GameSettings.NumAnimals = (int)value;

        m_NumAnimalsText.text = value.ToString();

        Debug.Log("Num animals changed");
    }

    public void OnLifelineSliderChanged(float value)
    {
        GameSettings.NumLifelines = (int)value;

        m_NumLifelinesText.text = value.ToString();

        Debug.Log("Num lifelines changed");
    }

    private void LoadDifficulty()
    {
        m_NumAnimals.value = GameSettings.NumAnimals;

        m_NumLifelines.value = GameSettings.NumLifelines;

        if (GameSettings.GameMode == EGameMode._2D)
        {
            m_2DToggle.isOn = true;
        }
        else
        {
            m_3DToggle.isOn = true;
        }
    }

    private void SetGameMode(EGameMode gameMode)
    {
        GameSettings.GameMode = gameMode;
    }

    public void Set2DGameMode() => SetGameMode(EGameMode._2D);

    public void Set3DGameMode() => SetGameMode(EGameMode._3D);

    public void LoadGame()
    {
        if (GameSettings.GameMode == EGameMode._2D)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
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
