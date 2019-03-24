﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Jump2 : MonoBehaviour
{
    //num jumps
    private int extraJumps;
    public int extraJumpsValue;
    private Rigidbody2D rb;
    public float jumpVelocity;
    private PlayerController2 player2;

    private void Awake()
    {
        //instantiate
        player2 = GetComponent<PlayerController2>();
    }

    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player2.isGrounded == true || player2.isLadder || player2.isLadderTop)
        {
            extraJumps = extraJumpsValue;
        }


        if (InputManager.ActiveDevice.Action1.WasPressed && extraJumps > 0)
        {
            FindObjectOfType<AudioManager>().Play("jump");
            rb.velocity = Vector2.up * jumpVelocity;
            extraJumps--;
        }
        else if (InputManager.ActiveDevice.Action1.WasPressed && extraJumps == 0 && (player2.isGrounded == true || player2.isLadder || player2.isLadderTop))
        {
            FindObjectOfType<AudioManager>().Play("jump");
            rb.velocity = Vector2.up * jumpVelocity;
        }

    }
}
