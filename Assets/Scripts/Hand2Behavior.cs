using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hand2Behavior : MonoBehaviour
{
    private GameObject player2;
    private Vector2 handPos;//Hand position in screen coordinates
    private Vector2 directionVector;//Direction hand should point to from the character body
    private Vector2 playerPos;//Position of player relative to screen coords
    private float diff;//for rotation of hand
    private float tempAngle;//for rotation of hand
    private float bottom_Angle;//For rotating hand
    private float currentHandAngle;//Current rotation of the hand
    private float handBodyDistance;//distance from hand to player

    private float handRadius = 2f;
    private float vectorRatio;

    private Vector2 tempPoint;

    // Use this for initialization
    void Start()
    {
        //Move to the position of parent object which is player2
        transform.localPosition = new Vector2(0.0f, 0.0f);
        currentHandAngle = 0.0f;
        player2 = GameObject.FindWithTag("Player2");
    }



    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("HandHorizontal") * handRadius;
        float y = Input.GetAxis("HandVertical") * handRadius * (-1);

        Vector2 joystickPos = new Vector2(x, y);
        playerPos = player2.transform.position;

        handBodyDistance = joystickPos.magnitude;


        if (handBodyDistance > handRadius)
        {
            vectorRatio = handRadius / handBodyDistance;
            joystickPos = joystickPos * vectorRatio;
        }

        handPos = playerPos + joystickPos;

        transform.position = handPos;

        bottom_Angle = Vector2.Angle(joystickPos, new Vector2(0.0f, -1.0f));

        //Debug.Log(bottom_Angle);

        if (transform.position.x < playerPos.x)
        {
            tempAngle = -bottom_Angle;
            if (currentHandAngle < 0)
            {
                //Debug.Log("1");
                diff = tempAngle - currentHandAngle;
                transform.Rotate(0f, 0f, diff);
                currentHandAngle += diff;
                //Debug.Log(currentHandAngle);
            }
            else
            {
                //Debug.Log("2");
                diff = tempAngle - currentHandAngle;
                transform.Rotate(0f, 0f, diff);
                currentHandAngle += diff;
            }
        }
        else
        {
            tempAngle = bottom_Angle;
            if (currentHandAngle < 0)
            {
                //Debug.Log("3");
                diff = tempAngle - currentHandAngle;
                transform.Rotate(0f, 0f, diff);
                currentHandAngle += diff;
            }
            else
            {
                //Debug.Log("4");
                diff = tempAngle - currentHandAngle;
                transform.Rotate(0f, 0f, diff);
                currentHandAngle += diff;
            }

        }

    }

}
