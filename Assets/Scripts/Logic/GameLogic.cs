using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class GameLogic
{
    public event Action<AnimalData> OnNewAnimalAppears;
    public event Action<AnimalData> OnAnimalCorrect;
    public event Action<AnimalData> OnGameOver;
    public event Action OnGameWon;

    private HashSet<AnimalData> m_AnimalsOnBoard;
    private List<AnimalData> m_AnimalsInGame;

    private AnimalData m_CurrentAnimal;
    private AnimalData m_LastAnimal;

    private int m_NumAnimalsToCollect;
    private Array m_GenderValues;
    private int m_NumAnimalTypes;
    private List<EAnimal> m_AnimalTypesInGame;

    private int m_CountFromLastPossibleAnimal;
    private const int MAX_INVALID_ANIMALS = 2;

    public IReadOnlyList<EAnimal> AnimalTypesInGame => m_AnimalTypesInGame;
    public IReadOnlyCollection<AnimalData> AnimalsOnBoard => m_AnimalsOnBoard;

    public void StartGame(int numAnimals, int numLifelines)
    {
        // Needs to initilize the data it's looking for (hashset)
        m_AnimalsOnBoard = new();
        m_AnimalsInGame = new();

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

        for (int i = 0; i < m_AnimalTypesInGame.Count; i++)
        {
            EAnimal currentType = m_AnimalTypesInGame[i];

            AnimalData currentFemaleAnimal = new AnimalData(EGender.Female, currentType);
            AnimalData currentMaleAnimal = new AnimalData(EGender.Male, currentType);

            m_AnimalsInGame.Add(currentFemaleAnimal);
            m_AnimalsInGame.Add(currentMaleAnimal);
        }

        PickNewAnimal();
    }

    public void PickNewAnimal()
    {
        System.Random random = new();

        if (m_CountFromLastPossibleAnimal > MAX_INVALID_ANIMALS)
        {
            List<AnimalData> animalsNotOnBoard = m_AnimalsInGame.Except(m_AnimalsOnBoard).ToList();
            int rndIndex = random.Next(0, animalsNotOnBoard.Count);

            m_CurrentAnimal = animalsNotOnBoard[rndIndex];

            m_CountFromLastPossibleAnimal = 0;

            OnNewAnimalAppears(m_CurrentAnimal);
        }
        else
        {
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

                if (AnimalsOnBoard.Contains(m_CurrentAnimal))
                {
                    m_CountFromLastPossibleAnimal++;
                }
                else
                {
                    m_CountFromLastPossibleAnimal = 0;
                }

                OnNewAnimalAppears(m_CurrentAnimal);
            }
        }        
    }

    public void AcceptAnimal()
    {
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
