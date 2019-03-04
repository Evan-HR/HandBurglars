using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PlayerController2 : MonoBehaviour
{
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
    private Health PlayerHealth;
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


    //Get player hide status
    public bool getHideStatus()
    {
        return hide;
    }

    //Set hide status to true
    public void setHideStatusTrue()
    {
        hide = true;
    }

    //Set hide status to false
    public void setHideStatusFalse()
    {
        hide = false;
    }

    //camera shaking
    public float camShakeAmt = 0.1f;
    CameraShake camShake;

    private PlayerController2 instance;


    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {

        PlayerHealth = GetComponent<Health>();

        FindObjectOfType<AudioManager>().Play("bossBattle");

        //Get rigid body component
        rb = GetComponent<Rigidbody2D>();

        dashTime = startDashTime;
        //set hitTime cooldown to adjust in inspector
        hitCooldown = startHitTime;
        crouchSpeed = crouchSpeedAdjust;
        //camera shaker
        camShake = GetComponent<CameraShake>();

        //get CannonShoot
        cannonShoot = GameObject.FindObjectOfType<CannonShoot>();

        print("initialization of player 2 is done");
        //print("at the start, the global health is:" + Health.sharedLives);
        //print("at the start, player health is " + PlayerHealth.health);
    }

    //collision with monster hand
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("BossSmashHand") && canBeHit == true)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("smash");
            camShake.Shake(camShakeAmt, 0.2f);

            FindObjectOfType<AudioManager>().Play("lostHealth");

            PlayerHealth.health--;

            //hit cooldown
            canBeHit = false;
            Invoke("getHit", hitCooldown);

        } else if (collision.gameObject.tag.Equals("Critter") && canBeHit == true && PlayerHealth.health >= 1)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("lostHealth");
            PlayerHealth.health--;

            //hit cooldown
            canBeHit = false;
            Invoke("getHit", hitCooldown);
        }

        // Death Condition
        if (PlayerHealth.health == 0)
        {
            print("PlayerController DEATH player HEALTH (should be 0!): " + PlayerHealth.health);
            //decrement global health
            PlayerHealth.Death();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        //print("Moving status for player 2: " + moving);

        //Debug.Log("Status of hiding: " + getHideStatus() + " and key H press down:" + Input.GetKeyDown(KeyCode.H));

        //isGrounded means you are on ground, or ladder, or top of ladder, thus will reset your # of jumps
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isLadder = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsLadder);
        isLadderTop = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsLadderTop);

        moveInput = Input.GetAxisRaw("LeftJoystickHorizontal");
        verticalMove = Input.GetAxisRaw("LeftJoystickVertical");

        //crouching C, slow speed by crouchSpeed (adjust in inspector)
        if (moving && Input.GetKey(KeyCode.C))
        {
            rb.velocity = new Vector2(moveInput * crouchSpeed, rb.velocity.y);
            //Debug.Log("The move input2 when player is not hide is: " + moveInput);
        }
        else if (moving)
        {
            //When joystick is not used, moveInput still hold a positive value close to zero
            //And the 2D box which is closest to ground can't be slippery material(There should be better approach)
            if (Math.Abs(moveInput) > 0.2)
            {
                print("Move input for player 2 is " + moveInput);
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            }
        }

        //Player has the ability to hide themselves
        //If player is not hidden and then the button is pressed
        //Works for player 1 not player 2
        //Player 2 can hide but  critter can find player
        if (getHideStatus() == false && Input.GetKeyDown(KeyCode.Joystick1Button2))
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
        else if (getHideStatus() == true && Input.GetKeyUp(KeyCode.Joystick1Button2))
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
        else if (facingRight == true && moveInput < 0)
        {

            mySpriteRenderer.flipX = true;
            facingRight = false;


        }
        //ladder stuff Input.GetKey(KeyCode.C))
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        if (moving)
        {
            if (hitInfo.collider != null) // means we're on/in the ladder hitbox
            {
                //Fail to catch cross direction control
                if (Input.GetAxisRaw("LeftJoystickVertical")!=0)
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
                //verticalMove = Input.GetAxisRaw("LeftJoystickVertical");
                //print("Vertical move for player 2: " + verticalMove);
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

            // if (Input.GetKeyDown(KeyCode.Joystick1Button2) && (inputTime == Time.time)){
            //    inputTime = Time.time;
            //    cannonShoot.ShootCannon();
            // }
            //Cannon Shoots when M is pressed, isShootCannon prevents cannon from shooting more than once
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) && !isShootCannon && grabCannon.GetIsAttached())
            {
               isShootCannon = true;
               cannonShoot.ShootCannon();
            }
            else if (Input.GetKeyUp(KeyCode.Joystick1Button2) && isShootCannon)
            {
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
