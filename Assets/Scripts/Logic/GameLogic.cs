using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class GameLogic
{
    // Game over, game won should also be events
    public event Action<AnimalData> OnNewAnimalAppears;
    public event Action<AnimalData> OnAnimalCorrect;
    public event Action<AnimalData> OnGameOver;
    public event Action OnGameWon;

    private HashSet<AnimalData> m_AnimalsOnBoard;

    private AnimalData m_CurrentAnimal;

    private int m_NumAnimalsToCollect;
    private Array m_GenderValues;
    private int m_NumAnimalTypes;
    private List<EAnimal> m_AnimalTypesInGame;

    public void StartGame(EDifficulty difficulty)
    {
        // Needs to initilize the data it's looking for (hashset)
        m_AnimalsOnBoard = new();

        m_AnimalTypesInGame = new();
        m_AnimalTypesInGame.AddRange(Enum.GetValues(typeof(EAnimal)));
        m_GenderValues = Enum.GetValues(typeof(EGender));

        switch (difficulty)
        {
            default:
            case EDifficulty.Easy:
                m_NumAnimalTypes = 3;
                m_NumAnimalsToCollect = m_NumAnimalTypes * m_GenderValues.Length;
                break;
            case EDifficulty.Medium:
                m_NumAnimalTypes = 5;
                m_NumAnimalsToCollect = m_NumAnimalTypes * m_GenderValues.Length;
                break;
            case EDifficulty.Hard:
                m_NumAnimalTypes = 8;
                m_NumAnimalsToCollect = m_NumAnimalTypes * m_GenderValues.Length;
                break;
        }

        System.Random rnd = new();
        m_AnimalTypesInGame = (List<EAnimal>)m_AnimalTypesInGame.OrderBy(item => rnd.Next());

        while (m_AnimalTypesInGame.Count > m_NumAnimalTypes)
        {
            m_AnimalTypesInGame.RemoveAt(0);
        }


        PickNewAnimal();
    }

    public void PickNewAnimal()
    {
        System.Random random = new();
        int index = random.Next(0, m_AnimalTypesInGame.Count);

        EAnimal randomAnimal = m_AnimalTypesInGame[index];
        EGender randomGender = (EGender)m_GenderValues.GetValue(random.Next(m_GenderValues.Length));

        m_CurrentAnimal = new AnimalData(randomGender, randomAnimal);

        OnNewAnimalAppears(m_CurrentAnimal);
    }

    public void AcceptAnimal()
    {
        // TODO: Add accept VFX
        Debug.Log("Accept");

        if (IsAnimalCorrect(m_CurrentAnimal))
        {
            m_AnimalsOnBoard.Add(m_CurrentAnimal);
            OnAnimalCorrect(m_CurrentAnimal);

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
        // TODO: Add decline VFX
        Debug.Log("Decline");
        PickNewAnimal();
    }

    private bool IsAnimalCorrect(AnimalData animal)
    {
        if (m_AnimalsOnBoard.Contains(animal))
        {
            OnGameOver(animal);
            return false;
        }
            
        return true;
    }
}
