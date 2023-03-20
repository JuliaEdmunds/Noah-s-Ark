using Cinemachine;
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
    [Header("Main Objects")]
    [SerializeField] private GameObject m_Forest;
    [SerializeField] private GameObject m_ForestEntry;
    [SerializeField] private GameObject m_Ship;
    [SerializeField] private GameObject m_ShipEntry;
    [SerializeField] private GameObject m_AnimalShowPos;
    [SerializeField] private Material m_SunnySkybox;
    [SerializeField] private Animal3DDictionary m_AnimalDataPrefabDict = new();

    [Header("Spawn Controls")]
    [SerializeField] private Vector3 m_SpawnPos;
    [SerializeField, Range(0.1f, 5f)] private float m_MoveSpeed;

    [SerializeField, Header("UI")] private TextMeshPro m_AnimalText;
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;

    [Header("Lifeline")]
    [SerializeField] private GameObject m_AnimalsOnBoardScreen;
    [SerializeField] private Animal3DDictionary m_AllAnimalsOnBoard;
    [SerializeField] private GameObject m_Lifeline;
    [SerializeField] private TextMeshPro m_LifelineText;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera m_MainCamera;
    [SerializeField] private CinemachineVirtualCamera m_ShipCamera;

    private GameLogic m_GameLogic = new();
    private int m_NumLifelinesLeft;
    private Animal3D m_CurrentAnimal;


    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;

        m_GameLogic.StartGame(GameSettings.NumAnimals, GameSettings.NumLifelines);

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
        bool shouldDestroy = true;

        if (m_CurrentAnimal == null)
        {
            return;
        }

        m_GameLogic.AcceptAnimal();
        Vector3 targetPos = m_ShipEntry.transform.position;
        StartCoroutine(MoveAnimal(m_CurrentAnimal, targetPos, shouldDestroy));
    }

    public void DeclineAnimal()
    {
        bool shouldDestroy = true;

        if (m_CurrentAnimal == null)
        {
            return;
        }

        m_GameLogic.DeclineAnimal();
        Vector3 targetPos = m_ForestEntry.transform.position;
        StartCoroutine(MoveAnimal(m_CurrentAnimal, targetPos, shouldDestroy));
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
        TurnOffClickables();
        m_GameOverText.text = $"{animal.Gender} {animal.AnimalType} was already on board - you sink.";
        m_GameOverScreen.SetActive(true);
    }


    private void OnGameWon()
    {
        TurnOffClickables();
        RenderSettings.skybox = m_SunnySkybox;
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
        GameObject animalGameObject = Instantiate(currentPrefab, m_SpawnPos, Quaternion.LookRotation(currentPrefab.transform.forward, Vector3.up));
        m_CurrentAnimal = animalGameObject.GetComponent<Animal3D>();

        Vector3 targetPos = m_AnimalShowPos.transform.position;
        MoveAnimal(m_CurrentAnimal, targetPos, false);
    }

    private IEnumerator MoveAnimal(Animal3D animal, Vector3 targetPos, bool shouldDestroy)
    {
        float duration = 0.5f;
        float timeElapsed = 0;
        Vector3 startPos = animal.transform.position;

        animal.transform.LookAt(targetPos);
        animal.StartMoving();

        while (timeElapsed < duration)
        {
            m_AnimalText.gameObject.SetActive(false);
            animal.transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        animal.StopMoving();

        // TODO: Add VFX of animal disappearing
        if (shouldDestroy)
        {
            Destroy(animal.gameObject);
        }
        
        m_CurrentAnimal = null;
    }

    private IEnumerator ShowAnimalsOnBoard()
    {
        m_AnimalsOnBoardScreen.SetActive(true);
        m_MainCamera.Priority = 1;
        m_ShipCamera.Priority = 10;

        yield return new WaitForSeconds(5);

        m_MainCamera.Priority = 10;
        m_ShipCamera.Priority = 1;

        yield return new WaitForSeconds(5);

        m_AnimalsOnBoardScreen.SetActive(false);
    }

    private void TurnOffClickables()
    {
        m_Lifeline.SetActive(false);
        m_Ship.SetActive(false);
        m_Forest.SetActive(false);
        StopAllCoroutines();
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
