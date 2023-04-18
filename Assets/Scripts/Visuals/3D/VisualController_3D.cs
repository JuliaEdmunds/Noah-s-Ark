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
    [SerializeField] private GameObject m_ForestZone;
    [SerializeField] private GameObject m_ForestEntry;
    [SerializeField] private GameObject m_ShipObject;
    [SerializeField] private GameObject m_ShipZone;
    [SerializeField] private GameObject m_ShipEntry;
    [SerializeField] private GameObject m_ShipExit;
    [SerializeField] private GameObject m_AnimalShowPos;
    [SerializeField] private Material m_SunnySkybox;
    [SerializeField] private ParticleSystem m_ShipParticles;
    [SerializeField] private ParticleSystem m_ForestParticles;
    [SerializeField] private Animal3DDictionary m_AnimalDataPrefabDict = new();

    [Header("Spawn Controls")]
    [SerializeField] private Vector3 m_SpawnPos;
    [SerializeField, Range(0.1f, 5f)] private float m_MoveSpeed;

    [Header("UI")]
    [SerializeField] private GameObject m_BackToMenuButton;
    [SerializeField] private TextMeshPro m_AnimalText;
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [SerializeField] private Color m_WinColor;

    [Header("Lifeline")]
    [SerializeField] private GameObject m_AnimalsOnBoardScreen;
    [SerializeField] private Animal3DDictionary m_AllAnimalsOnBoard;
    [SerializeField] private GameObject m_Lifeline;
    [SerializeField] private TextMeshPro m_LifelineText;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera m_MainCamera;
    [SerializeField] private CinemachineVirtualCamera m_ShipCameraOne;
    [SerializeField] private CinemachineVirtualCamera m_ShipCameraTwo;
    [SerializeField] private CinemachineVirtualCamera m_ShipCameraThree;

    [Header("Audio")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_WonGameSound;
    [SerializeField] private AudioClip m_LostGameSound;
    [SerializeField] private AudioClip m_AnimalCorrectSound;

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
        m_AudioSource.Play();

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
        StartCoroutine(MoveAnimal(m_CurrentAnimal, m_ShipEntry, shouldDestroy));
    }

    public void DeclineAnimal()
    {
        bool shouldDestroy = true;

        if (m_CurrentAnimal == null)
        {
            return;
        }

        m_GameLogic.DeclineAnimal();
        StartCoroutine(MoveAnimal(m_CurrentAnimal, m_ForestEntry, shouldDestroy));
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
        m_AudioSource.PlayOneShot(m_AnimalCorrectSound, 1);

        Animal3D currentAnimal = m_AllAnimalsOnBoard[animal];

        GameObject currentAnimalGameOject = currentAnimal.AnimalPrefab;

        currentAnimalGameOject.SetActive(true);
    }

    private void OnGameOver(AnimalData animal)
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_LostGameSound, 1);
        TurnOffClickables();
        m_GameOverText.text = $"{animal.Gender} {animal.AnimalType} was already on board - you sink.";
        m_GameOverScreen.SetActive(true);
    }


    private void OnGameWon()
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_WonGameSound, 1);
        TurnOffClickables();
        StartCoroutine(MoveShip());
        RenderSettings.skybox = m_SunnySkybox;
        m_GameOverText.text = "All animals on board. Congrats!";
        m_GameOverText.color = m_WinColor;
        m_GameOverScreen.SetActive(true);
    }

    private IEnumerator MoveShip()
    {
        float duration = 2.5f;
        float timeElapsed = 0;

        Vector3 startPos = m_ShipObject.transform.position;
        Vector3 targetPos = m_ShipExit.transform.position;

        m_ShipObject.transform.LookAt(targetPos);

        while (timeElapsed < duration)
        {
            m_ShipObject.transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;  
        }
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

        StartCoroutine(MoveAnimal(m_CurrentAnimal, m_AnimalShowPos, false));
    }

    private IEnumerator MoveAnimal(Animal3D animal, GameObject entryPoint, bool shouldDestroy)
    {
        float duration = 0.5f;
        float timeElapsed = 0;

        Vector3 startPos = animal.transform.position;
        Vector3 targetPos = entryPoint.transform.position;

        animal.transform.LookAt(targetPos);
        animal.StartMoving();

        while (timeElapsed < duration)
        {
            animal.transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        animal.StopMoving();

        if (shouldDestroy)
        {
            if (entryPoint == m_ForestEntry)
            {
                m_ForestParticles.Play();
            }
            else
            {
                m_ShipParticles.Play();
            }

            m_AnimalText.gameObject.SetActive(false);
            Destroy(animal.gameObject);
            m_CurrentAnimal = null;
        }        
    }

    private IEnumerator ShowAnimalsOnBoard()
    {
        m_ShipZone.SetActive(false);
        m_AnimalsOnBoardScreen.SetActive(true);

        foreach (var animal in m_AllAnimalsOnBoard)
        {
            if (animal.Value.gameObject.activeSelf)
            {
                animal.Value.StopMoving();
            }
        }

        m_MainCamera.Priority = 1;
        m_ShipCameraOne.Priority = 10;

        yield return new WaitForSeconds(2f);

        m_ShipCameraOne.Priority = 1;
        m_ShipCameraTwo.Priority = 10;

        yield return new WaitForSeconds(2f);

        m_ShipCameraTwo.Priority = 1;
        m_ShipCameraThree.Priority = 10;

        yield return new WaitForSeconds(2.5f);

        m_MainCamera.Priority = 10;
        m_ShipCameraThree.Priority = 1;

        yield return new WaitForSeconds(2.5f);

        m_ShipZone.SetActive(true);

        m_AnimalsOnBoardScreen.SetActive(false);
    }

    private void TurnOffClickables()
    {
        m_Lifeline.SetActive(false);
        m_ShipZone.SetActive(false);
        m_ForestZone.SetActive(false);
        m_BackToMenuButton.SetActive(false);

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
