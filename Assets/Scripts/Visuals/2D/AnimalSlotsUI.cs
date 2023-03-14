using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalSlotsUI : MonoBehaviour
{
    [SerializeField] private Image m_Male;
    [SerializeField] private Image m_Female;

    public void Init(Sprite male, Sprite female)
    {
        m_Male.sprite = male;
        m_Female.sprite = female;

        gameObject.SetActive(false);
        m_Male.gameObject.SetActive(false);
        m_Female.gameObject.SetActive(false);
    }

    public void BoardedAnimal(EGender gender)
    {
        gameObject.SetActive(true);

        if (gender == EGender.Male)
        {
            m_Male.gameObject.SetActive(true);
        }
        else
        {
            m_Female.gameObject.SetActive(true);
        }
    }
}
