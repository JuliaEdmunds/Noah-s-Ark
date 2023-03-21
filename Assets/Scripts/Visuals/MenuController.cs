using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider m_NumAnimals;
    [SerializeField] private Slider m_NumLifelines;
    [SerializeField] private TextMeshProUGUI m_NumAnimalsText;
    [SerializeField] private TextMeshProUGUI m_NumLifelinesText;

    [Header("Levelometer")]
    [SerializeField] private TextMeshProUGUI m_DifficultyText;
    [SerializeField] private Slider m_DifficultySlider;
    [SerializeField] private int m_MinLifelines = 0;
    [SerializeField] private int m_MaxLifelines = 3;

    [Header("Game Mode")]
    [SerializeField] private Toggle m_2DToggle;
    [SerializeField] private Toggle m_3DToggle;

    [Header("Audio")]
    [SerializeField] private AudioSource m_AudioSource;

    private int m_Difficulty = 0;
    private int m_MinAnimals = 2;
    private int m_MaxAnimals = Enum.GetNames(typeof(EAnimal)).Length;

    private void Start()
    {
        LoadDifficulty();
        UpdateDifficulty(GameSettings.NumAnimals, GameSettings.NumLifelines);
        m_AudioSource.Play();
    }

    public void OnAnimalSliderChanged(float value)
    {
        GameSettings.NumAnimals = (int)value;

        m_NumAnimalsText.text = value.ToString();

        UpdateDifficulty(GameSettings.NumAnimals, GameSettings.NumLifelines);
    }

    public void OnLifelineSliderChanged(float value)
    {
        GameSettings.NumLifelines = (int)value;

        m_NumLifelinesText.text = value.ToString();

        UpdateDifficulty(GameSettings.NumAnimals, GameSettings.NumLifelines);
    }

    private void UpdateDifficulty(int numAnimals, int numLifelines)
    {
        // Increased max values to stop level from showing 0
        int maxAnimals = m_MaxAnimals + 1;
        int maxLifelines = m_MaxLifelines + 1;

        float difficultyAnimals = (float)(numAnimals - maxAnimals) / (float)(m_MinAnimals - maxAnimals);
        float difficultyLifelines = (float)(numLifelines - maxLifelines) / (float)(m_MinLifelines - maxLifelines);

        m_Difficulty = (int)(difficultyAnimals * difficultyLifelines * 100);

        m_DifficultyText.text = $"Level: {m_Difficulty}%";
        m_DifficultySlider.value = m_Difficulty;
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
