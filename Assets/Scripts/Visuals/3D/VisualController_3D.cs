using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualController_3D : MonoBehaviour
{
    [SerializeField] private GameObject m_Forest;
    [SerializeField] private GameObject m_Ship;

    private GameLogic m_GameLogic = new();
    private int m_NumLifelinesLeft;

    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;

        m_GameLogic.StartGame(GameSettings.NumAnimals, GameSettings.NumLifelines);

        m_NumLifelinesLeft = GameSettings.NumLifelines;  
    }

    private void OnDestroy()
    {
        m_GameLogic.OnNewAnimalAppears -= OnNewAnimalAppears;
        m_GameLogic.OnAnimalCorrect -= OnAnimalCorrect;
        m_GameLogic.OnGameWon -= OnGameWon;
        m_GameLogic.OnGameOver -= OnGameOver;
    }

    public void AcceptAnimal()
    {
        m_GameLogic.AcceptAnimal();
    }

    public void DeclineAnimal()
    {
        m_GameLogic.DeclineAnimal();
    }

    public void GetHelp()
    {

    }

    private void OnNewAnimalAppears(AnimalData animal)
    {

    }

    private void OnAnimalCorrect(AnimalData animal)
    { 

    }

    private void OnGameOver(AnimalData animal)
    {

    }


    private void OnGameWon()
    {
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
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
