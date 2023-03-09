using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic
{
    // Game over, game won should also be events
    public event Action<EGender, EAnimal> OnNewAnimalAppears;

    public event Action<EGender, EAnimal> OnAnimalCorrect;

    public event Action<EGender, EAnimal> OnGameOver;

    public event Action OnGameWon;

    private HashSet<KeyValuePair<EGender, EAnimal>> m_AnimalsOnBoard;

    private KeyValuePair<EGender, EAnimal> m_CurrentAnimal;

    private int m_NumAnimalsToCollect;
    private Array m_AnimalValues;
    private Array m_GenderValues;

    public void StartGame()
    {
        // Needs to initilize the data it's looking for (hashset)
        m_AnimalsOnBoard = new();

        m_AnimalValues = Enum.GetValues(typeof(EAnimal));
        m_GenderValues = Enum.GetValues(typeof(EGender));
        m_NumAnimalsToCollect = m_AnimalValues.Length * m_GenderValues.Length;

        PickNewAnimal();
    }

    public void PickNewAnimal()
    {
        System.Random random = new();

        EAnimal randomAnimal = (EAnimal)m_AnimalValues.GetValue(random.Next(m_AnimalValues.Length));
        EGender randomGender = (EGender)m_GenderValues.GetValue(random.Next(m_GenderValues.Length));

        m_CurrentAnimal = KeyValuePair.Create(randomGender, randomAnimal);

        OnNewAnimalAppears(randomGender, randomAnimal);
    }

    public void AcceptAnimal()
    {
        Debug.Log("Accept");

        if (IsAnimalCorrect(m_CurrentAnimal))
        {
            m_AnimalsOnBoard.Add(m_CurrentAnimal);
            OnAnimalCorrect(m_CurrentAnimal.Key, m_CurrentAnimal.Value);

            if (m_AnimalsOnBoard.Count == m_NumAnimalsToCollect)
            {
                OnGameWon();
            }
            else
            {
                PickNewAnimal();
            }
        }  
    }

    public void DeclineAnimal()
    {
        Debug.Log("Decline");
        PickNewAnimal();
    }

    private bool IsAnimalCorrect(KeyValuePair<EGender, EAnimal> animal)
    {
        if (m_AnimalsOnBoard.Contains(animal))
        {
            OnGameOver(animal.Key, animal.Value);
            return false;
        }

        return true;
    }
}
