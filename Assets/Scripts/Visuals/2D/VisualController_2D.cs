using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualController_2D : MonoBehaviour
{
    [Header("Animal")]
    [SerializeField] private TextMeshProUGUI m_AnimalText;
    [SerializeField] Image m_AnimalSprite;
    [SerializeField] Animal2DDictionary m_Animal2DDictionary = new();

    [Header("Game State")]
    [SerializeField] private List<GameObject> m_GameOnButtons;
    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [SerializeField] private Color m_WinColor;

    [Header("Lifeline")]
    [SerializeField] private TextMeshProUGUI m_LifelineText;
    [SerializeField] private Button m_LifelineButton;
    [SerializeField] private GameObject m_LifelineImage;
    [SerializeField] private GameObject m_AnimalsOnBoardScreen;
    [SerializeField] private List<AnimalSlotsUI> m_AnimalPairsList;

    [Header("Audio")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_AcceptSound;
    [SerializeField] private AudioClip m_GameLostSound;
    [SerializeField] private AudioClip m_GameWonSound;

    private GameLogic m_GameLogic = new();
    private Dictionary<EAnimal, AnimalSlotsUI> m_AnimalPairsDict = new();
    private int m_NumLifelinesLeft;

    private const string MAIN_2D_SCENE = "Main_2D";
    private const string MENU_SCENE = "Menu";

    void Start()
    {
        m_AudioSource.Play();
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;

        m_GameLogic.StartGame(GameSettings.NumAnimals, GameSettings.NumLifelines);

        m_NumLifelinesLeft = GameSettings.NumLifelines;

        if (m_NumLifelinesLeft < 1)
        {
            m_LifelineButton.interactable = false;
            m_LifelineImage.SetActive(false);
        }

        ShowNumLifelines();

        // Generate slots for animals in game
        IReadOnlyList<EAnimal> animalsInGame = m_GameLogic.AnimalTypesInGame;
        int numAnimalsInGame = animalsInGame.Count;

        for (int i = 0; i < m_AnimalPairsList.Count; i++)
        {
            AnimalSlotsUI currentPair = m_AnimalPairsList[i];

            if (i < numAnimalsInGame)
            {
                EAnimal currentType = animalsInGame[i];
                AnimalData currentTypeMale = new(EGender.Male, currentType);
                AnimalData currentTypeFemale = new(EGender.Female, currentType);
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
        if (m_NumLifelinesLeft < 1)
        {
            return;
        }

        m_NumLifelinesLeft--;

        ShowNumLifelines();

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

        m_AudioSource.PlayOneShot(m_AcceptSound, 1);
    }

    private IEnumerator ShowAnimalsOnBoard()
    {
        m_AnimalsOnBoardScreen.SetActive(true);

        foreach (var button in m_GameOnButtons)
        {
            button.SetActive(false);
        }

        yield return new WaitForSeconds(5);

        m_AnimalsOnBoardScreen.SetActive(false);

        foreach (var button in m_GameOnButtons)
        {
            button.SetActive(true);
        }
    }

    private void OnGameOver(AnimalData animal)
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_GameLostSound, 1);
        m_GameOverText.text = $"{animal.Gender} {animal.AnimalType} was already on board - you sink.";

        foreach (var button in m_GameOnButtons)
        {
            button.SetActive(false);
        }

        m_GameOverScreen.SetActive(true);
        m_AnimalsOnBoardScreen.SetActive(true);
    }

    private void ShowNumLifelines()
    {
        if (m_NumLifelinesLeft < 1)
        {
            m_LifelineImage.SetActive(false);
            m_LifelineButton.interactable = false;
            m_LifelineText.text = "No lifelines";
        }
        else if (m_NumLifelinesLeft == 1)
        {
            m_LifelineText.text = $"{m_NumLifelinesLeft} lifeline available";
        }
        else
        {
            m_LifelineText.text = $"{m_NumLifelinesLeft} lifelines available";
        }
    }

    private void OnGameWon()
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_GameWonSound, 1);

        m_GameOverText.text = "All animals on board. Congrats!";
        m_GameOverText.color = m_WinColor;

        foreach (var button in m_GameOnButtons)
        {
            button.SetActive(false);
        }

        m_GameOverScreen.SetActive(true);
        m_AnimalsOnBoardScreen.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(MAIN_2D_SCENE);
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
