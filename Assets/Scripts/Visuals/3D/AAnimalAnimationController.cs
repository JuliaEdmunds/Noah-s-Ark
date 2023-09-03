using UnityEngine;

public abstract class AAnimalAnimationController : MonoBehaviour
{
    [SerializeField] protected Animator m_Animator;

    public abstract void StartMoving();

    public abstract void StopMoving();
}
