using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpSpeed = 18f;
    private float direction = 0f;
    private Rigidbody2D player;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isTouchingGround;


    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        // for left/right movement
        if (direction > 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
        }
        else if (direction < 0f)
        {
            // if player is on a tilted platform, player can slide when keys not pressed
            // this keeps the player still when not pressing keys
            player.velocity = new Vector2(direction * speed, player.velocity.y);

        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        //for jump movement
        if(Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }
    }
}
