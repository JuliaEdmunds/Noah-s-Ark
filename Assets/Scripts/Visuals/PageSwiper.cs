using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour
{
    public event Action OnTutorialClosed;

    [Serializable]
    private struct PanelData
    {
        public int PanelIndex;
        public GameObject Panel;
    }

    [SerializeField] private List<PanelData> m_PanelData;

    private int m_CurrentPanelIndex = 0;
    private int m_MaxPanelIndex = 2;

    public void ForwardClick()
    {
        if (m_CurrentPanelIndex >= m_MaxPanelIndex)
        {
            return;
        }

        GameObject currentPanel = m_PanelData[m_CurrentPanelIndex].Panel;
        GameObject nextPanel = m_PanelData[m_CurrentPanelIndex + 1].Panel;

        nextPanel.SetActive(true);
        currentPanel.SetActive(false);

        m_CurrentPanelIndex++;
    }

    public void BackClick()
    {
        if (m_CurrentPanelIndex <= 0)
        {
            return;
        }

        GameObject currentPanel = m_PanelData[m_CurrentPanelIndex].Panel;
        GameObject nextPanel = m_PanelData[m_CurrentPanelIndex - 1].Panel;

        nextPanel.SetActive(true);
        currentPanel.SetActive(false);

        m_CurrentPanelIndex--;
    }

    public void CloseTutorial()
    {
        GameObject currentPanel = m_PanelData[m_CurrentPanelIndex].Panel;
        GameObject firstPanel = m_PanelData[0].Panel;

        currentPanel.SetActive(false);
        firstPanel.SetActive(true);

        m_CurrentPanelIndex = 0;

        gameObject.SetActive(false);

        OnTutorialClosed?.Invoke();
    }
}
