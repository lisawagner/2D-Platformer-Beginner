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

    private Collider2D coll;
    private Rigidbody2D rb;

    private bool isFacingLeft = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFacingLeft)
        {
            if(transform.position.x > leftCap)
            {
                if(transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                if(coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
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


                if (coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                }


            }
            else
            {
                isFacingLeft = true;
            }
        }
    }
}
