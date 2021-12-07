using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PinkPlayerController : MonoBehaviour
{
    private enum State { IDLE, RUN, JUMP, FALL, HURT };
    private State state = State.IDLE;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpSpeed = 18f;
    private float direction = 0f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isTouchingGround;

    private Rigidbody2D player;
    private Animator playerAnimation;
    private CapsuleCollider2D playerHitBox;
    //private AudioSource footsteps;

    [SerializeField] private Vector3 respawnPoint;
    public GameObject fallDetector;

    public Text scoreText;
    public HealthBar healthBar; // access the public health script

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerHitBox = GetComponent<CapsuleCollider2D>(); //could just use generic Collider2D 
        //footsteps = GetComponent<AudioSource>();
        respawnPoint = transform.position; // stores players initial position to respawn to
        scoreText.text = "SCORE: " + ScoreController.totalScore;

    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (state != State.HURT)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        
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
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        //for jump movement
        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            state = State.JUMP;
        }

        // to change speed parameter in animator
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);          

        // fall detection - follow the player on the x axis
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.transform.position.y);
        // --- ///
        AnimationState();
        playerAnimation.SetInteger("State", (int)state);
    }

    private void AnimationState()
    {
        if (state == State.JUMP)
        {
            if (player.velocity.y < 0.1f)
            {
                state = State.FALL;
            }
        }
        else if (state == State.FALL)
        {
            if (isTouchingGround)
            {
                state = State.IDLE;
            }
        }
        //else if (Mathf.Abs(player.velocity.x) > 2f) // 2f gives player a small slide in movements
        else if (Mathf.Abs(player.velocity.x) > Mathf.Epsilon) // epsilon prevents the slide

        {
            state = State.RUN;
        }
        else
        {
            state = State.IDLE;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position; // set to the new player position at checkpoint
        }
        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position; // reset spawn position
        }
        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position; // reset spawn position
        }
        else if (collision.tag == "Crystal")
        {
            ScoreController.totalScore += 1;
            scoreText.text = "SCORE: " + ScoreController.totalScore;
            Debug.Log(ScoreController.totalScore); ////// ?ABSTRACTION OF SCORE? ////
            collision.gameObject.SetActive(false); // disable object
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Spikes")
        {
            healthBar.Damage(0.002f);
        }
    }

    //private void FootSounds()
    //{
    //    footsteps.Play();
    //}
}
