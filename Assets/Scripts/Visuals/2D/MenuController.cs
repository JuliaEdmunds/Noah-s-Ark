using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider m_NumAnimals;
    [SerializeField] private Slider m_NumLifelines;
    [SerializeField] private TextMeshProUGUI m_NumAnimalsText;
    [SerializeField] private TextMeshProUGUI m_NumLifelinesText;

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
    }

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
