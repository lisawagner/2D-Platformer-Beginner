using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Bounds")]
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [Header("Enemy Movement")]
    [SerializeField] private float jumpLength = 2;
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private LayerMask Ground;

    private Animator enemyAnimator;
    private Collider2D enemyHitBox;
    private Rigidbody2D enemy;

    private bool isFacingLeft = true;

    //TODO: Redo enemyAI to idle/walk/run depending on detecting player nearby
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        enemyHitBox = GetComponent<Collider2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovementHandler();
    }

    private void EnemyMovementHandler()
    {
        if (isFacingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                if (enemyHitBox.IsTouchingLayers(Ground))
                {
                    enemy.velocity = new Vector2(-jumpLength, jumpHeight);
                }
            }
            else
            {
                isFacingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                if (enemyHitBox.IsTouchingLayers(Ground))
                {
                    enemy.velocity = new Vector2(jumpLength, jumpHeight);
                }
            }
            else
            {
                isFacingLeft = true;
            }
        }
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
