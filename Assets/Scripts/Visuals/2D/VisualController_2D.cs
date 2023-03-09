using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController_2D : MonoBehaviour
{
    private GameLogic m_GameLogic = new();

    // Start is called before the first frame update
    void Start()
    {
        m_GameLogic.OnNewAnimalAppears += OnNewAnimalAppears;
    }

    private void OnDestroy()
    {
        m_GameLogic.OnNewAnimalAppears -= OnNewAnimalAppears;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AcceptAnimal()
    {
        m_GameLogic.AcceptAnimal();
    }

    public void DeclineAnimal()
    {
        m_GameLogic.DeclineAnimal();
    }

    private void OnNewAnimalAppears(EAnimal species, EGender gender)
    {
        Debug.Log($"New animal: {gender} {species}");
    }
}
