using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private VisualController_2D m_VisualControler;
    private UnityEngine.UI.Button button;
    public EDifficulty Difficulty;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    void SetDifficulty()
    {
        m_VisualControler.m_Difficulty = Difficulty;
        SceneManager.LoadScene(1);
    }
}

