using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private EActionType m_ActionType;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_ActionType == EActionType.Accept)
            {
                Debug.Log("Accept");
            }
            else
            {
                Debug.Log("Decline");
            }
        }
    }
}
