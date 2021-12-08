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
    [SerializeField] private float hurtForce = 0.02f; //rebound/kickback from bumping enemy
    private float direction = 0f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isTouchingGround;

    private Rigidbody2D player;
    private Animator playerAnimation;
    private CapsuleCollider2D playerHitBox; // for better re-use on different players, use Collider2D instead
    // CapsuleCollider2D is a child of Collider2D, as is BoxCollider2D, etc. // Inheritance?
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
                                                          //playerHitBox unused atm; using alternate ground check method
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
            MovementHandler(); // Abstraction Principle
        }
        // --- ///
        AnimationStateHandler(); // Abstraction Principle
        playerAnimation.SetInteger("State", (int)state);//updates animation based on Enumerator state

        //TODO: Fix so that anims reset after fall caused by hurt knockback
        // fall detection - follow the player on the x axis
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.transform.position.y);
    }

    private void MovementHandler()
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
        // could use (Input.GetButtonDown("Jump") && playerHitBox.isTouchingGround(groundLayer))
        {
            JumpHandler();
        }

        // to change speed parameter in animator
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);          

    }

    private void JumpHandler()
    {
        player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        state = State.JUMP;
    }

    private void AnimationStateHandler()
        // TODO: State.FALL doesn't detect the player falling off of a floating platform, only "falls" after jump. Add from platforms
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
        else if (state == State.HURT)
        {
            // if we're hurt and no longer moving, reset to IDLE so that we can move again
            if (Mathf.Abs(player.velocity.x) < 0.1f)
            {
                state = State.IDLE;
            }
            //else if (!isTouchingGround)
            //{
            //    state = State.FALL;
            //}
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
            state = State.IDLE;
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
            ////// ABSTRACTION OF SCORE: ScoreController ////
            ScoreController.totalScore += 1;
            scoreText.text = "SCORE: " + ScoreController.totalScore; 
            Debug.Log(ScoreController.totalScore); 
            collision.gameObject.SetActive(false); // disable object
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyController monster = other.gameObject.GetComponent<EnemyController>();
            
            if (state == State.FALL)
            {
                monster.HeadSmashedIn();
                //Destroy(other.gameObject);
                JumpHandler();
            }
            else
            {
                //TODO: consider coroutine to time the knockback animation
                state = State.HURT;
                HandleHealth();
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy is to the right
                    player.velocity = new Vector2(-hurtForce, player.velocity.y);
                }
                else
                {
                    //enemy is to the left
                    player.velocity = new Vector2(hurtForce, player.velocity.y);
                }
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //TODO: Fix - It doesn't run continuously while player stays on spikes
        if (collision.tag == "Spikes")
        {
            healthBar.Damage(0.002f);
        }
    }

    private void HandleHealth()
    {
        healthBar.Damage(0.1f);


    }

    //private void FootSounds()
    //{
    //    footsteps.Play();
    //}
}
