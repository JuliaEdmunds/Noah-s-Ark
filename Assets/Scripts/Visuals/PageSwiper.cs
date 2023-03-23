using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float PercentThreshold = 0.2f;
    public float Easing = 0.5f;
    public int TotalPages = 3;

    private int m_CurrentPage = 1;
    private Vector3 m_PanelLocation;

    private void Start()
    {
        m_PanelLocation = transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = m_PanelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;

        if (Mathf.Abs(percentage) >= PercentThreshold)
        {
            Vector3 newLocation = m_PanelLocation;

            if (percentage > 0 && m_CurrentPage < TotalPages)
            {
                m_CurrentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if (percentage < 0 && m_CurrentPage > 1)
            {
                m_CurrentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
            }

            StartCoroutine(SmoothMove(transform.position, newLocation, Easing));
            m_PanelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, m_PanelLocation, Easing));
        }
    }

    public IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;

        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
