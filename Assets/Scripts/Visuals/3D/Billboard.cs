using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(m_Camera.transform, Vector3.up);
    }
}
