using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;

public class VisualController_2D : MonoBehaviour
{
    private GameLogic m_GameLogic = new();

    // Start is called before the first frame update
    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
        m_GameLogic.OnGameOver += OnGameOver;
        m_GameLogic.OnAnimalCorrect += OnAnimalCorrect;
        m_GameLogic.OnGameWon += OnGameWon;

        m_GameLogic.StartGame();
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

    private void OnNewAnimalAppears(EGender gender, EAnimal species)
    {
        Debug.Log($"New animal: {gender} {species}");
    }

    private void OnAnimalCorrect(EGender gender, EAnimal species)
    {
        Debug.Log($"New animal on board: {gender} {species}");
    }

    private void OnGameOver(EGender gender, EAnimal species)
    {
        // TODO: implement game over
        Debug.Log($"{gender} {species} was already on board - you sink.");
    }

    private void OnGameWon()
    {
        // TODO: implement game won
        Debug.Log($"All animals on board.");
    }
}
