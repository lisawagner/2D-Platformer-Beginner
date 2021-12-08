using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    protected Animator enemyAnimator; /// PROTECTED VARIABLE ///

    protected virtual void Start()
    /// PROTECTED ACCESS: Only children can access the animator ///
    /// VIRTUAL:  ///
    {
        enemyAnimator = GetComponent<Animator>();
    }


    public void HeadSmashedIn()
    {
        
        enemyAnimator.SetTrigger("Death");
        
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
