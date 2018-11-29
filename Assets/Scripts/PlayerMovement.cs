using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public float inputVertical;
    //used to adjust gravity when climbing ladder
    public Rigidbody2D rb;

    //ladder vaiables 
    public float distance;
    public LayerMask whatIsLadder;
    private bool isClimbing;


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
            else
            {
                isClimbing = false;
            }

            if(isClimbing == true)
            {
                inputVertical = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(0, inputVertical * runSpeed);

                //doesn't fall down when on ladder
                rb.gravityScale = 0;
                
            }
            else
            {
                //will fall, set gravity back to normal  
                rb.gravityScale = 3;
            }
        }
    }
}
