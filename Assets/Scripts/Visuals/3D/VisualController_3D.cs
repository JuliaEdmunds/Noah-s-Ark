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
    [SerializeField] private Animal3DDictionary m_AnimalDataPrefabDict = new();

    [SerializeField, Space] private Vector3 m_SpawnPos;

    [SerializeField, Header("UI")] private TextMeshProUGUI m_AnimalText;
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_GameOverText;

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
        StartCoroutine(RemoveAnimal(m_CurrentAnimal));
    }

    public void DeclineAnimal()
    {
        m_GameLogic.DeclineAnimal();
        StartCoroutine(RemoveAnimal(m_CurrentAnimal));
    }

    public void GetHelp()
    {

    }

    private void OnNewAnimalAppears(AnimalData animal)
    {
        StartCoroutine(SpawnAnimal(animal));
    }

    private void OnAnimalCorrect(AnimalData animal)
    { 

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
