using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;
    public float runSpeed = 40f;

    public float climbSpeed = 10f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    float verticalMove;
    //used to adjust gravity when climbing ladder
    public Rigidbody2D rb;

    //ladder vaiables 
    public float distance;
    public LayerMask whatIsLadder;
    private bool isClimbing;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update () {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        Debug.Log("Player1 move :" + horizontalMove);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        
    }

    void FixedUpdate()
    {
        // Move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,Vector2.up,distance,whatIsLadder);
        if(hitInfo.collider != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
              
            }
            else if ((Input.GetButtonDown("Jump")) || (hitInfo.collider == null))
            {
                isClimbing = false;
                rb.gravityScale = 3;
            }

            if(isClimbing)
            {
                verticalMove = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(rb.velocity.x, verticalMove * climbSpeed);

                
                rb.gravityScale = 0;   //so the player doesn't fall down when on ladder
                rb.velocity.Set(rb.velocity.x, 0); // gets rid of residual effects from gravity 
            }
        }
    }
}
