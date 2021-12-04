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

    private Animator playerAnimation;
    private AudioSource footsteps;

    [SerializeField] private Vector3 respawnPoint;
    public GameObject fallDetector;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        footsteps = GetComponent<AudioSource>();
        respawnPoint = transform.position; // stores players initial position to respawn to
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
            transform.localScale = new Vector2(1, 1);
        }
        else if (direction < 0f)
        {
            // if player is on a tilted platform, player can slide when keys not pressed
            // this keeps the player still when not pressing keys
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-1, 1);
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

        // to change speed parameter in animator
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        // fall detection - follow the player on the x axis
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.transform.position.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
    }

    private void FootSounds()
    {
        footsteps.Play();
    }
}
