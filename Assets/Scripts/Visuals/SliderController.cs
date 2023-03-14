using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Value;

    public void OnSliderChanged(float value)
    {
        m_Value.text = value.ToString();

        if (gameObject.name == "Animal Slider")
        {
            GameSettings.NumAnimals = (int)value;
            Debug.Log("Num animals changed");
        }
        else
        {
            GameSettings.NumLifelines = (int)value;
            Debug.Log("Num lifelines changed");
        }
    }
}
