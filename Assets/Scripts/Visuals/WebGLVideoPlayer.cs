using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoPlayer : MonoBehaviour
{
    [SerializeField] private string m_VideoName;
    [SerializeField] private VideoPlayer m_VideoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_VideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Video/" + m_VideoName + ".mov");
        m_VideoPlayer.Play();
    }
}
