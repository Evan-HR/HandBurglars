using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump2 : MonoBehaviour
{
    public float fallMultiplier;
    public float lowJumpMultiplier;

    Rigidbody2D rb;


    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Joystick1Button3))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }
}
