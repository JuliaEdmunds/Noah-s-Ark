using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal3D : MonoBehaviour
{
    [SerializeField] private GameObject m_AnimalPrefab;
    [SerializeField] private AAnimalAnimationController m_AnimationController;

    // Common pattern to protect against setting serialized data
    // use short => instead of get { ... }
    public GameObject AnimalPrefab => m_AnimalPrefab;

    // TODO: Call this script from VisualController
    public void StartMoving()
    {
        m_AnimationController.StartMoving();
    }

    // TODO: Call this script from VisualController
    public void StopMoving()
    {
        m_AnimationController.StopMoving();
    }
}
