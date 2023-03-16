using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Animal3DDictionary m_AnimalDataPrefabDict = new();

    [SerializeField, Space] private Vector3 m_SpawnPos;

    [SerializeField, Header("UI")] private TextMeshProUGUI m_AnimalText;
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;

    [SerializeField, Header("Lifeline")] private GameObject m_AnimalsOnBoardScreen;
    [SerializeField] private Animal3DDictionary m_AllAnimalsOnBoard;
    [SerializeField] private GameObject m_Lifeline;
    [SerializeField] private TextMeshProUGUI m_LifelineText;

    private GameLogic m_GameLogic = new();
    private int m_NumLifelinesLeft;
    private GameObject m_CurrentAnimal;

    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;

        m_GameLogic.StartGame(GameSettings.NumAnimals, GameSettings.NumLifelines);


        // TODO: Find a 3D HELP sign and place it at the front of the screen + fix the postiion of the liflines on screen
        m_NumLifelinesLeft = GameSettings.NumLifelines;

        if (m_NumLifelinesLeft < 1)
        {
            m_Lifeline.SetActive(false);
            m_LifelineText.text = null;
        }
        else
        {
            m_LifelineText.text = $"{m_NumLifelinesLeft}";
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
        StartCoroutine(RemoveAnimal(m_CurrentAnimal));
    }

    public void DeclineAnimal()
    {
        m_GameLogic.DeclineAnimal();
        StartCoroutine(RemoveAnimal(m_CurrentAnimal));
    }

    public void GetHelp()
    {
        if (m_NumLifelinesLeft < 1)
        {
            m_Lifeline.SetActive(false);
            m_LifelineText.text = null;
            return;
        }

        m_NumLifelinesLeft--;
        m_LifelineText.text = $"{m_NumLifelinesLeft}";
        StartCoroutine(ShowAnimalsOnBoard());

        if (m_NumLifelinesLeft < 1)
        {
            m_Lifeline.SetActive(false);
            m_LifelineText.text = null;
        }
    }

    private void OnNewAnimalAppears(AnimalData animal)
    {
        StartCoroutine(SpawnAnimal(animal));
    }

    private void OnAnimalCorrect(AnimalData animal)
    {
        GameObject currentAnimal = m_AllAnimalsOnBoard[animal].AnimalPrefab;
        currentAnimal.SetActive(true);
    }

    private void OnGameOver(AnimalData animal)
    {
        StopAllCoroutines();
        m_GameOverText.text = $"{animal.Gender} {animal.AnimalType} was already on board - you sink.";
        m_GameOverScreen.SetActive(true);
    }


    private void OnGameWon()
    {
        StopAllCoroutines();
        m_GameOverText.text = "All animals on board. Congrats!";
        m_GameOverScreen.SetActive(true);
    }

    private IEnumerator SpawnAnimal(AnimalData animal)
    {
        Animal3D currentAnimalData = m_AnimalDataPrefabDict[animal];
        GameObject currentPrefab = currentAnimalData.AnimalPrefab;

        // Fow nor instantiate straight at presentable position
        yield return new WaitForSeconds(1.5f);
        m_AnimalText.gameObject.SetActive(true);
        m_AnimalText.text = $"{animal.Gender} {animal.AnimalType}";
        m_CurrentAnimal = Instantiate(currentPrefab, m_SpawnPos, currentPrefab.transform.rotation);
    }

    private IEnumerator RemoveAnimal(GameObject animal)
    {
        m_AnimalText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(animal);
    }

    private IEnumerator ShowAnimalsOnBoard()
    {
        m_AnimalsOnBoardScreen.SetActive(true);

        yield return new WaitForSeconds(5);

        m_AnimalsOnBoardScreen.SetActive(false);
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
