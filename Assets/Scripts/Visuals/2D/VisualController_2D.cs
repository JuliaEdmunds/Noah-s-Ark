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
    [SerializeField, Header("Animal")] private TextMeshProUGUI m_AnimalText;
    [SerializeField] Image m_AnimalSprite;
    [SerializeField] Animal2DDictionary m_Animal2DDictionary = new();
    [SerializeField, Header("Game State")] GameObject m_GameOnButtons;
    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [SerializeField, Header("Lifeline")] private TextMeshProUGUI m_LifelineText;
    [SerializeField] private Button m_LifelineButton;
    [SerializeField] private GameObject m_AnimalsOnBoardScreen;
    [SerializeField] private List<AnimalSlotsUI> m_AnimalPairsList;

    private GameLogic m_GameLogic = new();
    public EDifficulty m_Difficulty;

    private Dictionary<EAnimal, AnimalSlotsUI> m_AnimalPairsDict = new();

    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;
        // m_GameLogic.ShowAnimalsOnBoard += ShowAnimalsOnBoard;

        m_GameLogic.StartGame(GameSettings.Difficulty);

        // Generate slots for animals in game
        IReadOnlyList<EAnimal> animalsInGame = m_GameLogic.AnimalTypesInGame;
        int numAnimalsInGame = animalsInGame.Count;

        for (int i = 0; i < m_AnimalPairsList.Count; i++)
        {
            AnimalSlotsUI currentPair = m_AnimalPairsList[i];

            if (i < numAnimalsInGame)
            {
                EAnimal currentType = animalsInGame[i];
                AnimalData currentTypeMale = new AnimalData(EGender.Male, currentType);
                AnimalData currentTypeFemale = new AnimalData(EGender.Female, currentType);
                Sprite male = m_Animal2DDictionary[currentTypeMale].Sprite;
                Sprite female = m_Animal2DDictionary[currentTypeFemale].Sprite;
                currentPair.Init(male, female);

                m_AnimalPairsDict.Add(currentType, currentPair);
            }
            else
            {
                currentPair.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        m_GameLogic.OnNewAnimalAppears -= OnNewAnimalAppears;
        m_GameLogic.OnAnimalCorrect -= OnAnimalCorrect;
        m_GameLogic.OnGameWon -= OnGameWon;
        m_GameLogic.OnGameOver -= OnGameOver;
        // m_GameLogic.ShowAnimalsOnBoard -= ShowAnimalsOnBoard;
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
        m_LifelineButton.interactable = false;
        m_LifelineText.text = "Lifeline (none left)";
        StartCoroutine(ShowAnimalsOnBoard());
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
        AnimalSlotsUI animalSlot = m_AnimalPairsDict[animal.AnimalType];

        animalSlot.BoardedAnimal(animal.Gender);
    }

    private IEnumerator ShowAnimalsOnBoard()
    {
        m_AnimalsOnBoardScreen.SetActive(true);
        m_GameOnButtons.SetActive(false);

        yield return new WaitForSeconds(5);

        m_AnimalsOnBoardScreen.SetActive(false);
        m_GameOnButtons.SetActive(true);
    }

    private void OnGameOver(AnimalData animal)
    {
        m_GameOverText.text = $"{animal.Gender} {animal.AnimalType} was already on board - you sink.";
        m_GameOnButtons.SetActive(false);
        m_GameOverScreen.SetActive(true);
        m_AnimalsOnBoardScreen.SetActive(true);
    }

    private void OnGameWon()
    {
        m_GameOverText.text = "All animals on board. Congrats!";
        m_GameOnButtons.SetActive(false);
        m_GameOverScreen.SetActive(true);
        m_AnimalsOnBoardScreen.SetActive(true);
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
