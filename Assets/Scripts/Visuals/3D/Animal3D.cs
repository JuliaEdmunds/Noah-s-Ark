using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal3D : MonoBehaviour
{
    [SerializeField] private GameObject m_AnimalPrefab;

    // Common pattern to protect against setting serialized data
    // use short => instead of get { ... }
    public GameObject AnimalPrefab => m_AnimalPrefab;
}
