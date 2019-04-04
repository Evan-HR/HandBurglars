using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Jump : MonoBehaviour {
    //num jumps
    private int extraJumps;
    public int extraJumpsValue;
    private Rigidbody2D rb;
    public float jumpVelocity;
    private PlayerController player;

    //Control scheme depending on connected controllers
    string[] controllerNameArray;
    int numberOfControllers;

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
        //gets number of devices at each fixedupdate
        numberOfControllers = InputManager.Devices.Count;

        if (player.isGrounded == true || player.isLadder || player.isLadderTop)
        {
            extraJumps = extraJumpsValue;
        }

        bool flagJump;

        if (numberOfControllers > 1)
        {
            flagJump = InputManager.Devices[0].Action1.IsPressed;
        }
        else
        {
            flagJump = Input.GetKeyDown(KeyCode.Space);
        }

        if (flagJump && extraJumps > 0)
        {
            FindObjectOfType<AudioManager>().Play("jump");
            rb.velocity = Vector2.up * jumpVelocity;
            extraJumps--;
        }
        else if (flagJump && extraJumps == 0 && (player.isGrounded == true || player.isLadder || player.isLadderTop))
        {
            FindObjectOfType<AudioManager>().Play("jump");
            rb.velocity = Vector2.up * jumpVelocity;
        }
       
    }
}
