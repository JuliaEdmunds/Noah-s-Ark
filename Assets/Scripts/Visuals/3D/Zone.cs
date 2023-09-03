using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
    [SerializeField] private EActionType m_ActionType;
    [SerializeField] private UnityEvent m_Event;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Event.Invoke();
        }
    }
}
