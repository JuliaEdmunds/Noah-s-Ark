using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private EVolume m_VolumeLevel;
    public EVolume VolumeLevel => m_VolumeLevel;
}
