using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualController_2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_AnimalText;
    [SerializeField] Image m_AnimalSprite;
    [SerializeField] Animal2DDictionary m_Animal2DDictionary = new();
    [SerializeField] GameObject m_GameOnButtons;
    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [SerializeField] private TextMeshProUGUI m_LifelineText;
    [SerializeField] private Button m_LifelineButton;

    private GameLogic m_GameLogic = new();
    public EDifficulty m_Difficulty;

    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;
        m_GameLogic.ShowAnimalsOnBoard += ShowAnimalsOnBoard;

        m_GameLogic.StartGame(GameSettings.Difficulty);
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

    public void GetHelp()
    {
        m_GameLogic.GetHelp();
        m_LifelineButton.interactable = false;
        m_LifelineText.text = "Lifeline (none left)";
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

    private void ShowAnimalsOnBoard(List<AnimalData> animalsOnBoard)
    {
        string allAnimals = "";
        for (int i = 0; i < animalsOnBoard.Count; i++)
        {
            AnimalData currentAnimal = animalsOnBoard[i];
            allAnimals += currentAnimal + " ";
        }

        Debug.Log($"{allAnimals}");
    }

    private void OnGameOver(AnimalData animal)
    {
        m_GameOverText.text = $"{animal.Gender} {animal.AnimalType} was already on board - you sink.";
        m_GameOnButtons.SetActive(false);
        m_GameOverScreen.SetActive(true);
    }

    private void OnGameWon()
    {
        // TODO: implement game won
        Debug.Log($"All animals on board.");

        m_GameOverText.text = "All animals on board. Congrats!";
        m_GameOnButtons.SetActive(false);
        m_GameOverScreen.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
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
