using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    protected Animator enemyAnimator; /// PROTECTED VARIABLES ///
    protected Rigidbody2D enemy;
    protected AudioSource deathSound;
    [SerializeField] protected AudioClip impact;

    protected virtual void Start()
    /// PROTECTED ACCESS: Only children can access the animator ///
    /// VIRTUAL:  ///
    {
        enemyAnimator = GetComponent<Animator>();
        enemy = GetComponent<Rigidbody2D>();
        deathSound = GetComponent<AudioSource>();
    }


    public void HeadSmashedIn()
    {       
        enemyAnimator.SetTrigger("Death");
        deathSound.PlayOneShot(impact, 0.7f);
        enemy.velocity = Vector2.zero;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
