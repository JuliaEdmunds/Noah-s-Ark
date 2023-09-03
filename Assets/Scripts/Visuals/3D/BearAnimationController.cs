public class BearAnimationController : AAnimalAnimationController
{
    public override void StartMoving()
    {
        m_Animator.SetBool("Idle", false);
        m_Animator.SetBool("Run Forward", true);
    }

    public override void StopMoving()
    {
        m_Animator.SetBool("Run Forward", false);
        m_Animator.SetBool("Idle", true);
    }
}
