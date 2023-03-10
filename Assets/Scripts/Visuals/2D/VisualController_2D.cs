using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualController_2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_AnimalText;
    [SerializeField] Image m_AnimalSprite;
    [SerializeField] Animal2DDictionary m_Animal2DDictionary = new();

    private GameLogic m_GameLogic = new();
    public EDifficulty m_Difficulty;

    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;

        m_GameLogic.StartGame(m_Difficulty);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
            Debug.Log("New game");
        }
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

    private void OnNewAnimalAppears(AnimalData animal)
    {
        m_AnimalText.text = $"{animal.Gender} {animal.AnimalType}";

        Animal2D currentAnimal = m_Animal2DDictionary[animal];
        Sprite currentAnimalSprite = currentAnimal.Sprite;
        m_AnimalSprite.sprite = currentAnimalSprite;
    }

    private void OnAnimalCorrect(AnimalData animal)
    {
        Debug.Log($"New animal on board: {animal.Gender} {animal.AnimalType}");
    }

    private void OnGameOver(AnimalData animal)
    {
        // TODO: implement game over
        Debug.Log($"{animal.Gender} {animal.AnimalType} was already on board - you sink.");
    }

    private void OnGameWon()
    {
        // TODO: implement game won
        Debug.Log($"All animals on board.");
    }
}
