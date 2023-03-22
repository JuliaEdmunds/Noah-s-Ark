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
    [SerializeField] private Image m_SliderFill; //connected the Image Fill from the slider
    [SerializeField] private Color m_MinLevelColour;
    [SerializeField] private Color m_MaxLevelColour;

    [Header("Level Grapghs")]
    [SerializeField] private AnimationCurve m_0LifelinesCurve;
    [SerializeField] private AnimationCurve m_1LifelinesCurve;
    [SerializeField] private AnimationCurve m_2LifelinesCurve;
    [SerializeField] private AnimationCurve m_3LifelinesCurve;

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

        m_NumAnimalsText.text = $"Number of animals: {GameSettings.NumAnimals}";
        m_NumLifelinesText.text = $"Number of lifelines: {GameSettings.NumLifelines}";

        UpdateDifficulty(GameSettings.NumAnimals, GameSettings.NumLifelines);
        m_AudioSource.Play();
    }

    public void OnAnimalSliderChanged(float value)
    {
        GameSettings.NumAnimals = (int)value;

        m_NumAnimalsText.text = $"Number of animals: {value}";

        UpdateDifficulty(GameSettings.NumAnimals, GameSettings.NumLifelines);
    }

    public void OnLifelineSliderChanged(float value)
    {
        GameSettings.NumLifelines = (int)value;

        m_NumLifelinesText.text = $"Number of lifelines: {value}";

        UpdateDifficulty(GameSettings.NumAnimals, GameSettings.NumLifelines);
    }

    private void UpdateDifficulty(int numAnimals, int numLifelines)
    {
        float normalizedAnimals = (float)(numAnimals - m_MinAnimals) / (float)(m_MaxAnimals - m_MinAnimals);

        AnimationCurve curve = m_0LifelinesCurve;

        if (numLifelines == 1)
        {
            curve = m_1LifelinesCurve;
        }
        else if (numLifelines == 2)
        {
            curve = m_2LifelinesCurve;
        }
        else if (numLifelines == 3)
        {
            curve = m_3LifelinesCurve;
        }

        m_Difficulty = Mathf.RoundToInt(curve.Evaluate(normalizedAnimals) * 100);

        m_SliderFill.color = Color.Lerp(m_MinLevelColour, m_MaxLevelColour, (float)m_Difficulty / 100);

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
