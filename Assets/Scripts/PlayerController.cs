using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour {
    //import gameManager
    public GameManager gameManager;

    //input Time used for button
    private float inputTime;

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
    private  Health PlayerHealth;
    private float hitCooldown;
    public float startHitTime;

    //The variable to flag player is hiding or not
    private bool hide = false;
    private float hideCoolDown = 2.0f;
    private bool moving = true;
    private int HIDING_LAYER_ORDER = -1;
    private int EXPOSING_LAYER_ORDER = 5;

    //crouch
    private bool crouch = false;
    private float crouchSpeed;
    public float crouchSpeedAdjust;

    //blood
    public GameObject blood;

    //cannon
    CannonShoot cannonShoot;
    GrabCannon grabCannon;
    bool isShootCannon = false;



    public bool getHideStatus()
    {
        return hide;
    }

    public void setHideStatusTrue()
    {
        hide = true;
    }

    public void setHideStatusFalse()
    {
        hide = false;
    }

    //camera shaking
    public float camShakeAmt = 0.1f;
    CameraShake camShake;

    private  PlayerController instance;

    public  PlayerController Instance{ 
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
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start() {
        
        PlayerHealth = GetComponent<Health>();
        //FindObjectOfType<AudioManager>().Play("bossBattle");
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        //set hitTime cooldown to adjust in inspector
        hitCooldown = startHitTime;
 
        crouchSpeed = crouchSpeedAdjust;

        //camera shaker
        camShake = GetComponent<CameraShake>();

        //get CannonShoot
        cannonShoot = GameObject.FindObjectOfType<CannonShoot>();
        grabCannon = GameObject.FindObjectOfType<GrabCannon>();
        print("at the start, the global health is:" +Health.sharedLives);
        print("at the start, player health is " + PlayerHealth.health);
    }

    //collision with monster hand 
    void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            //Debug.Log("collision: " + collision.gameObject.tag);

            if (collision.gameObject.tag.Equals("BossSmashHand") && canBeHit == true && PlayerHealth.health >= 1)
            {
                Instantiate(blood, transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("lostHealth");
                FindObjectOfType<AudioManager>().Play("smash");
                camShake.Shake(camShakeAmt, 0.2f);

                PlayerHealth.health--;

                //hit cooldown
                canBeHit = false;
                Invoke("getHit", hitCooldown);
                if (PlayerHealth.health == 0)
                {
                    print("PlayerController DEATH player HEALTH (should be 0!): " + PlayerHealth.health);
                    //decrement global health
                    PlayerHealth.Death();
                }
            }
            else if (collision.gameObject.tag.Equals("Critter") && canBeHit == true && PlayerHealth.health >= 1)
            {
                Instantiate(blood, transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("lostHealth");
                PlayerHealth.health--;

                //hit cooldown
                canBeHit = false;
                Invoke("getHit", hitCooldown);
            }

            // Death Condition
            Debug.Log("Player heath: " + PlayerHealth.health);

            if (PlayerHealth.health == 0)
            {
                print("PlayerController DEATH player HEALTH (should be 0!): " + PlayerHealth.health);
                //decrement global health
                PlayerHealth.Death();
            }
        } catch { }
    }


    // Update is called once per frame
    void FixedUpdate () {

        //Debug.Log("Status of hiding: " + getHideStatus() + " and key H press down:" + Input.GetKeyDown(KeyCode.H));

        //isGrounded means you are on ground, or ladder, or top of ladder, thus will reset your # of jumps
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,checkRadius,whatIsGround);
        isLadder = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsLadder);
        isLadderTop = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsLadderTop);

        moveInput = Input.GetAxisRaw("Horizontal");
        //crouching C, slow speed by crouchSpeed (adjust in inspector)
        if (moving && Input.GetKey(KeyCode.C))
        {  
            rb.velocity = new Vector2(moveInput * crouchSpeed, rb.velocity.y);
            //Debug.Log("The move input2 when player is not hide is: " + moveInput);
        }
        else if (moving)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }

        //Player has the ability to hide themselves
        //If player is not hidden and then the button is pressed
            if (getHideStatus() == false && Input.GetKeyDown(KeyCode.Q))
        {
            setHideStatusTrue();
            gameObject.layer = LayerMask.NameToLayer("HiddenPlayerBody");
            //Player can't move when they hide(works in later frame check condition)
            moving = false;
            //Velocity need to set zero to overwrite velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;

            //Change the layer of player to hide from bushes
            if (mySpriteRenderer)
            {
                mySpriteRenderer.sortingOrder = HIDING_LAYER_ORDER;
            }

            //Debug.Log("1, hidden status of player now is " + getHideStatus());


        }
        //After hide button is released, player can move again
        else if (getHideStatus() == true && Input.GetKeyUp(KeyCode.Q))
        {
            setHideStatusFalse();
            gameObject.layer = LayerMask.NameToLayer("PlayerBody");
            moving = true;

            //Chnage the layer of plyaer here
            if (mySpriteRenderer)
            {
                mySpriteRenderer.sortingOrder = EXPOSING_LAYER_ORDER;
            }

            //Debug.Log("2, hidden status of player now is " + getHideStatus());

        }

        if (facingRight == false && moveInput > 0)
        {
            mySpriteRenderer.flipX = false;
            facingRight = true;
            
        }
        else if(facingRight == true && moveInput < 0)
        {
          
            mySpriteRenderer.flipX = true;
            facingRight = false; 


        }
        //ladder stuff
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        if (moving)
        {
            if (hitInfo.collider != null) // means we're on/in the ladder hitbox
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    isClimbing = true;

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
                   
                    
                    FindObjectOfType<AudioManager>().Play("dash");
                    rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
                    canDash = false;
                    Invoke("DashCooldown", dashCooldown);

                }

            }

            //if (Input.GetKeyDown(KeyCode.M) && (inputTime == Time.time)){
            //    inputTime = Time.time;
            //    cannonShoot.ShootCannon();
            //}
            //Cannon Shoots when E is pressed, isShootCannon prevents cannon from shooting more than once
            if (Input.GetKeyDown(KeyCode.E) && !isShootCannon && grabCannon.GetIsAttached())
            {
                if (grabCannon.GetPlayer() == gameObject)
                {
                    print("READY TO SHOOT POTENTIALLY!");
                    isShootCannon = true;
                    cannonShoot.ShootCannon();
                }

            } else if (Input.GetKeyUp(KeyCode.E) && isShootCannon){
                isShootCannon = false;
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
