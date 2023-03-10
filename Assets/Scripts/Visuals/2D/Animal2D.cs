using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Animal2D
{
    [SerializeField] Sprite m_AnimalSprite;

    // Common pattern to protect against setting serialized data
    // use short => instead of get { ... }
    public Sprite Sprite => m_AnimalSprite;
}
