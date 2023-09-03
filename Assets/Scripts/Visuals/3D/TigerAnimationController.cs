public class TigerAnimationController : AAnimalAnimationController
{
    public override void StartMoving()
    {
        m_Animator.SetFloat("Speed_f", 1f);
    }

    public override void StopMoving()
    {
        m_Animator.SetFloat("Speed_f", 0f);
    }
}
