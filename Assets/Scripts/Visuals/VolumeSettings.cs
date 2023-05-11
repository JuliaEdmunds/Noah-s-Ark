using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private EVolume m_VolumeLevel;
    public EVolume VolumeLevel => m_VolumeLevel;
}

