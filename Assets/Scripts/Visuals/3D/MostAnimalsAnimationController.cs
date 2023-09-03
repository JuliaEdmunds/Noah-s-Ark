public class MostAnimalsAnimationController : AAnimalAnimationController
{
    public override void StartMoving()
    {
        m_Animator.SetFloat("Speed_f", 1f);
        m_Animator.SetBool("Eat_b", false);
    }

    public override void StopMoving()
    {
        m_Animator.SetFloat("Speed_f", 0);
        m_Animator.SetBool("Eat_b", true);
    }
}
