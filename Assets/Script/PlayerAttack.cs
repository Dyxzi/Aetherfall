using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;

    public void Attack()
    {
        anim.SetTrigger("isAttack");
    }

}
