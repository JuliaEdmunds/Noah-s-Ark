using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

