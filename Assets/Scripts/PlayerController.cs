using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour {

    //Player moving speed
    public float speed;

    private float moveInput;

    private Rigidbody2D rb;
    private bool facingRight = true;
    //for flipping the sprite
    private SpriteRenderer mySpriteRenderer;

    //if standing on ground
    public bool isGrounded;
    public bool isLadder;
    public bool isLadderTop;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //Player can dash horizontally
    //dash speed
    public float dashSpeed;
    //how long dash lasts
    private float dashTime;
    public float startDashTime;

    public bool canDash = true;
    public float dashCooldown;

    public SpriteRenderer playerSprite;

    //ladder variables
    float verticalMove;
    public float distance;
    public LayerMask whatIsLadder;
    public LayerMask whatIsLadderTop;
    private bool isClimbing;
    public float climbSpeed = 10f;
    public float gravity;

    //hittable variables
    public bool canBeHit = true;
    private static Health PlayerHealth;
    private float hitCooldown;
    public float startHitTime;


    //The variable to flag player is hiding or not
    private bool hide = true;

    public bool getHideStatus()
    {
        return hide;
    }

    //camera shaking
    public float camShakeAmt = 0.1f;
    CameraShake camShake;

    //used to get position of character for the ghost effect
    private static PlayerController instance;
    //gameObject named ghost
    [SerializeField]
    GameObject PlayerGhost;

    public static PlayerController Instance{ 
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }
   


    private void Awake()
    {
        PlayerHealth = GameObject.FindObjectOfType<Health>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        //set hitTime cooldown to adjust in inspector
        hitCooldown = startHitTime;
        //get sprite for ghost effect
        playerSprite = GetComponent<SpriteRenderer>();

        //camera shaker
        camShake = GetComponent<CameraShake>();
            }

    //collision with monster hand 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("BossSmashHand") && canBeHit == true && PlayerHealth.health > 1)
        {

            camShake.Shake(camShakeAmt, 0.2f);
            PlayerHealth.health--;
                canBeHit = false;
                Invoke("getHit", hitCooldown);

        }
        else if (collision.gameObject.tag.Equals("BossSmashHand") && canBeHit == true && PlayerHealth.health == 1)
        {
           
            PlayerHealth.health--;
            canBeHit = false;
            //placeholder "death" by freezing 
            //set Z axis to off, then freeze X and Y
            rb.constraints = RigidbodyConstraints2D.None;
       
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        }
    }


    // Update is called once per frame
    void FixedUpdate () {

        //isGrounded means you are on ground, or ladder, or top of ladder, thus will reset your # of jumps
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,checkRadius,whatIsGround);
        isLadder = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsLadder);
        isLadderTop = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsLadderTop);


        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);


        if (facingRight == false && moveInput > 0)
        {
            mySpriteRenderer.flipX = true;
        }
        else if(facingRight == true && moveInput < 0)
        {
            mySpriteRenderer.flipX = true;
        }
        //ladder stuff
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        if (hitInfo.collider != null) // means we're on/in the ladder hitbox
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                isClimbing = true;

            }
            else if ((Input.GetButtonDown("Jump")))
            {
                isClimbing = false;
            }
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {

            verticalMove = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalMove * climbSpeed);
            rb.gravityScale = 0;   //so the player doesn't fall down when on ladder
            rb.velocity.Set(rb.velocity.x, 0); // gets rid of residual effects from gravity 
        }
        else
        {
            //if not climbing, set gravity to normal (via inspector)
            rb.gravityScale = gravity;

        }



        //dashing
        if (dashTime <= 0)
            {
                dashTime = startDashTime;
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.F) && canDash == true)
                {
                GameObject dashEffect = Instantiate(PlayerGhost, transform.position, transform.rotation);
                    rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
                canDash = false;
                Invoke("DashCooldown", dashCooldown);

                }

            }


	}
    void DashCooldown()
    {
        canDash = true;
    }

    void getHit()
    {
        canBeHit = true;
    }

}
