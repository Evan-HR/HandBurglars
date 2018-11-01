using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandBehavior : MonoBehaviour {

    //public Transform hand;

    private GameObject player;
    private GameObject baseInput;
    private Vector2 handPos;
    private Vector2 directionVector;
    private Vector2 playerPos;
    private Vector2 mousePos;
    private float diff;//for rotation of hand
    private float tempAngle;//for rotation of hand
    private float bottom_Angle;
    private float top_Angle;
    private float currentHandAngle;
    private float handBodyDistance;
    private float mouseBodyDistance;

    private float handRadius = 100.0f;
    private float mouseRadRatio;

    private Vector2 tempPoint;

    public float speed = 0.01f;

    // Use this for initialization
    void Start () {

        transform.localPosition = new Vector2(0.0f, 0.0f);
        currentHandAngle = 0.0f;
        baseInput = GameObject.Find("BaseInputController");
        player = GameObject.FindWithTag("Player");
       


    }



    // Update is called once per frame
    void Update()
    {
        //position = gameObject.transform.position;
        //print("Coords" + Input.mousePosition.x +" "+ Input.mousePosition.y);
        //baseInput.GetComponent<MouseInput>.GetComponent<mouse>

        //handPos = (Vector2)Camera.main.WorldToScreenPoint(gameObject.transform.position);
        handPos = (Vector2)Camera.main.WorldToScreenPoint(gameObject.transform.position);


        playerPos = (Vector2)Camera.main.WorldToScreenPoint(player.transform.position);

        mousePos = (Vector2)Input.mousePosition;

        handBodyDistance = Vector2.Distance(playerPos, handPos);
        mouseBodyDistance = Vector2.Distance(playerPos, mousePos);
        //print("distance " +mouseBodyDistance);
        //float xVal = baseInput.GetComponent<MouseInput>().GetHorizontal();
        //float yVal = baseInput.GetComponent<MouseInput>().GetVertical();

        //Vector2 mousePos = new Vector2(xVal, yVal);

        directionVector = mousePos - playerPos;




        //m_Angle = Vector2.Angle(playerPos, mousePos);
        //print("Angle " + m_Angle);
        float step = speed*Time.deltaTime;

        //Vector2 worldHandPos = Camera.main.ScreenToWorldPoint(handPos);
        //Vector2 worldPlayerPos = Camera.main.ScreenToWorldPoint(playerPos);

        if (mouseBodyDistance > handRadius) {

            mouseRadRatio = handRadius / directionVector.magnitude;
            //print("Ratio: " +mouseRadRatio);
            directionVector = directionVector * mouseRadRatio;
            /*if (mouseBodyDistance > 100){
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)player.transform.position + directionVector, step);
            }*/

            //transform.position = Vector2.MoveTowards(transform.position, (Vector2)player.transform.position + directionVector, step);
            bottom_Angle = Vector2.Angle(directionVector, new Vector2(0.0f,-1.0f));
            top_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, 1.0f));


            tempPoint = Camera.main.ScreenToWorldPoint(playerPos +  directionVector);
            transform.localPosition = Camera.main.transform.InverseTransformPoint(tempPoint);

            if (bottom_Angle < 0){//To the left of player object

                tempAngle = bottom_Angle;
                if (currentHandAngle < tempAngle){
                    diff = tempAngle - currentHandAngle;
                    transform.Rotate(0f, 0f, diff);
                    currentHandAngle += diff;
                }
                else{
                    diff = currentHandAngle - tempAngle;
                    transform.Rotate(0f, 0f, -diff);
                    currentHandAngle -= diff;
                }

            }
            else{
                tempAngle = bottom_Angle;
                if (currentHandAngle > tempAngle)
                {
                    diff = currentHandAngle - tempAngle;
                    transform.Rotate(0f, 0f, -diff);
                    currentHandAngle -= diff;
                }
                else
                {
                    diff = tempAngle - currentHandAngle;
                    transform.Rotate(0f, 0f, diff);
                    currentHandAngle += diff;
                }


            }


        } else{
            tempPoint = Camera.main.ScreenToWorldPoint(mousePos);
            transform.localPosition = Camera.main.transform.InverseTransformPoint(tempPoint);
        }


    }
}
