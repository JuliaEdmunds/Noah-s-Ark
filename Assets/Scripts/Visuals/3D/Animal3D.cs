using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal3D : MonoBehaviour
{
    [SerializeField] private GameObject m_AnimalPrefab;

    // Common pattern to protect against setting serialized data
    // use short => instead of get { ... }
    public GameObject AnimalPrefab => m_AnimalPrefab;

    // TODO: Call this script from VisualController
    public void StartMoving()
    {
        // TODO: Call AAnimalAnimationController.StartMoving()
    }

    // TODO: Call this script from VisualController
    public void StopMoving()
    {
        // TODO: Call AAnimalAnimationController.StopMoving()
    }
}
