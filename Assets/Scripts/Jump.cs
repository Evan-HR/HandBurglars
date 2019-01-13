using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    //num jumps
    private int extraJumps;
    public int extraJumpsValue;
    private Rigidbody2D rb;
    public float jumpVelocity;
    private PlayerController player;

    private void Awake()
    {
        //instantiate
        player = GetComponent<PlayerController>();
    }

    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();

    }
    
	// Update is called once per frame
	void FixedUpdate () {
        if (player.isGrounded == true || player.isLadder || player.isLadderTop)
        {
            extraJumps = extraJumpsValue;
        } 

        
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            soundManagerScript.PlaySound("jump");
            rb.velocity = Vector2.up * jumpVelocity;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && (player.isGrounded == true || player.isLadder || player.isLadderTop))
        {
            soundManagerScript.PlaySound("jump");
            rb.velocity = Vector2.up * jumpVelocity;
        }
       
    }
}
