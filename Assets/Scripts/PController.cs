using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PController     : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;

    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;

    public int amountOfJumps = 2 ;
    private int amountOfJumpsLeft;

    private bool isFacingRight;
    private bool isWalking; 
    private bool isGrounded;
    private bool canJump;
    private bool isTouchingWall;
    private bool isWallSliding;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGrounded;
    public PlayerStats ps;
    public GameOverScreen GameOverScreen;

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource powerUpSoundEffect;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    
    void Update()
    {
        if (DialogueManager.isActive == true)
        {
            return;
        }
        CheckInput();
        checkMovementDirection();
        UpdateAnimation();
        checkIfCanJump();
        checkIfWallSliding();
      
    }
    void FixedUpdate()
    {
        ApplyMovement();
        checkSurroundings();
    } 
   
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
           
                Jump();
        }
    }
    private void checkIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection,rb.velocity.y);

        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }
    private void Jump()
    {
        
        if (canJump)
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
            
    }
    private void checkIfCanJump()
    {
       
        if(isGrounded == true && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps; 
        }
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;

        }
        else
        {
            canJump = true;
        }
    }
   
    private void checkMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if(rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    #region Animation
    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }
    #endregion
    
    
    private void checkSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGrounded);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGrounded);
    }
   

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpPowerUp"))
        {
            Debug.Log("PowerUp");
            Destroy(collision.gameObject);
            jumpForce = 32f;
            GetComponent<SpriteRenderer>().color = Color.cyan;
            powerUpSoundEffect.Play();
            StartCoroutine(ResetJumpPowerUp());
           

        }
        if (collision.CompareTag("3JumpPowerUp"))
        {
            Debug.Log("3Jump PowerUp");
            Destroy(collision.gameObject);
            amountOfJumps = 3;
            GetComponent<SpriteRenderer>().color = Color.grey;
            powerUpSoundEffect.Play();
            StartCoroutine(Reset3jumpPowerUp());
        }
        if (collision.CompareTag("GravityPowerUp"))
        {
            Debug.Log("Gravity PowerUp");
            Destroy(collision.gameObject);
            rb.gravityScale = 3;
            GetComponent<SpriteRenderer>().color = Color.grey;
            powerUpSoundEffect.Play();
            StartCoroutine(ResetGravityPowerUp());
        }
        if (collision.CompareTag("Heal"))
        {
            Debug.Log("Healing");
            Destroy(collision.gameObject);
            FindObjectOfType<PlayerStats>().Heal(1);
            powerUpSoundEffect.Play();
        }
       if (collision.CompareTag("VictoryDiamond"))
        {
            Debug.Log("Victory");
            SceneManager.LoadScene("WinScene");
            FindObjectOfType<PlayFabManager>().SendLeaderBord(FindObjectOfType<Score>().lateScore);
            GameOverScreen.Setup(FindObjectOfType<Score>().lateScore);   
        }
    }
    private IEnumerator ResetJumpPowerUp()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 14;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private IEnumerator Reset3jumpPowerUp()
    {
        yield return new WaitForSeconds(15);
        amountOfJumps = 2;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private IEnumerator ResetGravityPowerUp()
    {
        yield return new WaitForSeconds(10);
        rb.gravityScale = 4;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
