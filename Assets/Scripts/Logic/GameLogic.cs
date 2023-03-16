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
    private AnimalData m_LastAnimal;

    private int m_NumAnimalsToCollect;
    private Array m_GenderValues;
    private int m_NumAnimalTypes;
    private List<EAnimal> m_AnimalTypesInGame;
    public IReadOnlyList<EAnimal> AnimalTypesInGame => m_AnimalTypesInGame;
    public IReadOnlyCollection<AnimalData> AnimalsOnBoard => m_AnimalsOnBoard;

    public void StartGame(int numAnimals, int numLifelines)
    {
        // Needs to initilize the data it's looking for (hashset)
        m_AnimalsOnBoard = new();

        m_AnimalTypesInGame = new();
        m_AnimalTypesInGame.AddRange(Enum.GetValues(typeof(EAnimal)));
        m_GenderValues = Enum.GetValues(typeof(EGender));

        m_NumAnimalTypes = numAnimals;
        m_NumAnimalsToCollect = m_NumAnimalTypes * m_GenderValues.Length;

        System.Random rnd = new();
        m_AnimalTypesInGame = m_AnimalTypesInGame.OrderBy(item => rnd.Next()).ToList();

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

        if (m_LastAnimal == m_CurrentAnimal)
        {
            PickNewAnimal();
        }
        else
        {
            m_LastAnimal = m_CurrentAnimal;
            OnNewAnimalAppears(m_CurrentAnimal);
        }
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
