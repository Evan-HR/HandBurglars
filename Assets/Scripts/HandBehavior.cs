using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandBehavior : MonoBehaviour {

    //public Transform hand;

    private GameObject player;
    private GameObject baseInput;
    private Vector2 handPos;//Hand position in screen coordinates
    private Vector2 directionVector;//Direction hand should point to from the character body
    private Vector2 playerPos;//Position of player relative to screen coords
    private Vector2 mousePos;//mouse position relative to screen coords
    private float diff;//for rotation of hand
    private float tempAngle;//for rotation of hand
    private float bottom_Angle;//For rotating hand
    private float top_Angle;//For rotating hand
    private float currentHandAngle;//Current rotation of the hand
    private float handBodyDistance;//distance from hand to player
    private float mouseBodyDistance;//distance from mouse to player

    private float handRadius = 100.0f;
    private float mouseRadRatio;

    private Vector2 tempPoint;

    public float speed = 0.01f;

    // Use this for initialization
    void Start () {

        transform.localPosition = new Vector2(0.0f, 0.0f);//localPosition - local to the parent(player)
        currentHandAngle = 0.0f;
        //baseInput = GameObject.Find("BaseInputController");
        player = GameObject.FindWithTag("Player");
       


    }



    // Update is called once per frame
    void Update()
    {
       
        handPos = (Vector2)Camera.main.WorldToScreenPoint(gameObject.transform.position);

        playerPos = (Vector2)Camera.main.WorldToScreenPoint(player.transform.position);

        mousePos = (Vector2)Input.mousePosition;

        handBodyDistance = Vector2.Distance(playerPos, handPos);
        mouseBodyDistance = Vector2.Distance(playerPos, mousePos);
       

        directionVector = mousePos - playerPos;
       
        float step = speed*Time.deltaTime;

        //For when hand is outside of player reach
        if (mouseBodyDistance > handRadius)
        {

            if (directionVector.magnitude != 0)
            {
                mouseRadRatio = handRadius / directionVector.magnitude;
            }


            directionVector = directionVector * mouseRadRatio;

            bottom_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, -1.0f));
            top_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, 1.0f));


            tempPoint = Camera.main.ScreenToWorldPoint(playerPos + directionVector);
            transform.localPosition = Camera.main.transform.InverseTransformPoint(tempPoint);

            //Can restructure the if-else tree to reduce it further!!!
            if (directionVector.x < 0)
            {
                tempAngle = -bottom_Angle;
                if (currentHandAngle < 0)
                {
                    diff = tempAngle - currentHandAngle;
                    transform.Rotate(0f, 0f, diff);
                    currentHandAngle += diff;
                }
                else
                {
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
                    diff = tempAngle - currentHandAngle;
                    transform.Rotate(0f, 0f, diff);
                    currentHandAngle += diff;
                }
                else
                {
                    diff = tempAngle - currentHandAngle;
                    transform.Rotate(0f, 0f, diff);
                    currentHandAngle += diff;
                }

             


            }
        }
        else{
            tempPoint = Camera.main.ScreenToWorldPoint(mousePos);
            transform.localPosition = Camera.main.transform.InverseTransformPoint(tempPoint);
        }


    }
}
