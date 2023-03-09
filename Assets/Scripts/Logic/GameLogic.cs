using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic
{
    // Game over, game won should also be events
    public event Action<EAnimal, EGender> OnNewAnimalAppears;

    public void StartGame()
    {
        // Needs to initilize the data it's looking for (hashset)
        PickNewAnimal();
    }

    public void AcceptAnimal()
    {
        Debug.Log("Accept");

        // Work out if correct and the call PickNewAnimal
        PickNewAnimal();
    }

    public void DeclineAnimal()
    {
        Debug.Log("Decline");
        // Work out if correct and the call PickNewAnimal
        PickNewAnimal();
    }

    public void PickNewAnimal()
    {
        System.Random random = new();

        Array animalValues = Enum.GetValues(typeof(EAnimal));
        EAnimal randomAnimal = (EAnimal)animalValues.GetValue(random.Next(animalValues.Length));

        Array genderValues = Enum.GetValues(typeof(EGender));
        EGender randomGender = (EGender)genderValues.GetValue(random.Next(genderValues.Length));

        OnNewAnimalAppears(randomAnimal, randomGender);
    }

    
}
