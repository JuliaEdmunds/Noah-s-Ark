using System;
using System.Collections.Generic;
using UnityEngine;

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

    private const int MAX_PANEL_INDEX = 2;
    private int m_CurrentPanelIndex = 0;

    public void ForwardClick()
    {
        if (m_CurrentPanelIndex >= MAX_PANEL_INDEX)
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
