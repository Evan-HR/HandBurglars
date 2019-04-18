using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;


public class Hand2Behavior : MonoBehaviour
{
    private GameObject player2;
    private GameObject chosenObject;
    private Vector2 handPos;//Hand position in screen coordinates
    private Vector2 directionVector;//Direction hand should point to from the character body
    private Vector2 playerPos;//Position of player relative to screen coords
    private float diff;//for rotation of hand
    private float tempAngle;//for rotation of hand
    private float bottom_Angle;//For rotating hand
    private float currentHandAngle;//Current rotation of the hand
    private float handBodyDistance;//distance from hand to player
    private bool toggleGrabMode = false;
    private bool isHolding = false;
    private List<GameObject> grabbableObjects;
    private int numOfTouchingObjects;
    private Transform tempTransform;//For grabbing objects and making them children of hand object
    private float handRadius = 3.5f;
    bool dragging;
    private float vectorRatio;

    private Vector2 tempPoint;

private void OnTriggerEnter2D(Collider2D other)
    {

        //If other is a child object, ref their root parent in this
        GameObject objectBody;
        Rigidbody2D objectRB = other.transform.GetComponent<Rigidbody2D>();
        if (!(isHolding))
        {
            if (!(objectRB == null))
            {


                //check if collider is on a child object
                if (other.transform.root != other.transform)
                {
                    objectBody = other.transform.root.gameObject;


                }
                else
                {
                    objectBody = other.transform.gameObject;
                }
                if (grabbableObjects.Contains(objectBody))
                {
                    //print("Same Object within reach");
                }
                else
                {
                    grabbableObjects.Add(objectBody);
                    numOfTouchingObjects++;
                    //print("Object within reach");
                }
            }
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        //print("Staying");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        grabbableObjects.Remove(other.gameObject);
        numOfTouchingObjects--;
    }

    // Use this for initialization
    void Start()
    {
        dragging = false;
        grabbableObjects = new List<GameObject>();
        chosenObject = null;
        numOfTouchingObjects = 0;
        //Move to the position of parent object which is player2
        transform.localPosition = new Vector2(0.0f, 0.0f);
        currentHandAngle = 0.0f;
        player2 = GameObject.FindWithTag("Player2");
    }

void checkGrab()
    {
        bool isGrabDown = Input.GetButtonDown("joystickGrab");
        bool isGrabUp = Input.GetButtonUp("joystickGrab");
        int indexOfMin = 0;
        float objectDistance;
        if (isHolding == true)
        {
            if (isGrabUp)
            {

                if (chosenObject.CompareTag("cannon"))
                {
                    chosenObject.SendMessage("disconnectHand");
                }

                //drop current object and open hand


                else
                {
                    chosenObject.transform.parent = tempTransform;
                    chosenObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    //chosenObject.layer = LayerMask.NameToLayer("HandObjects");
                    if (chosenObject.tag == "Draggable")
                    {
                        chosenObject.GetComponent<BossSpikeScript>().isAttached = false;
                    }


                    toggleGrabMode = false;
                }
                chosenObject = null;
                isHolding = false;
                dragging = false;
            }
        }

        if (isGrabDown & !isHolding)
        {
            if (grabbableObjects.Count == 0)
            {
                //print("Nothing to Grab");
            }
            else
            {
                float minDistance = gameObject.GetComponent<CircleCollider2D>().radius;

                for (int i = 0; i < numOfTouchingObjects; i++)
                {
                    objectDistance = Vector2.Distance(player2.transform.position, (Vector2)grabbableObjects[i].transform.position);
                    if (objectDistance < minDistance)
                    {
                        indexOfMin = i;
                        minDistance = objectDistance;
                    }
                }
                chosenObject = grabbableObjects[indexOfMin];
                grabbableObjects.Clear();



                if (chosenObject.CompareTag("Draggable"))
                {

                    //handle = chosenObject.transform.Find("RChainEnd").gameObject;

                    //Add the spring joint to hand. It attaches to the end of the draggable chain

                    //chosenObject.GetComponent<bossSpikeScript>().isAttached = true;

                    chosenObject.SendMessage("SetPlayer", gameObject.transform.parent.gameObject);
                    chosenObject.SendMessage("SetPlayerHand", gameObject);
                    chosenObject.SendMessage("SetIsAttached", true);
                    chosenObject.SendMessage("SetHasHandle", true);
                    chosenObject.SendMessage("AddHandle");

                    dragging = true;
                    isHolding = true;

                }
                else if (chosenObject.CompareTag("cannon"))
                {
                    chosenObject.SendMessage("SetPlayer", gameObject.transform.parent.gameObject);
                    chosenObject.SendMessage("SetPlayerHand", gameObject);
                    chosenObject.SendMessage("SetIsAttached", true);

                    chosenObject.SendMessage("ConnectHand");

                    isHolding = true;
                }
                else
                {

                    tempTransform = chosenObject.transform.parent;
                    chosenObject.transform.parent = gameObject.transform;
                    chosenObject.GetComponent<Rigidbody2D>().isKinematic = true;

                    //UNCOMMENT "chosenObject.layer = LayerMask.NameToLayer("Hand");" when FIXED!
                    //chosenObject.layer = LayerMask.NameToLayer("Hand");

                    //gameObject.GetComponent<SpriteRenderer>().sprite = grabbingHand;


                    isHolding = true;
                }

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        checkGrab();
        float x = InputManager.ActiveDevice.RightStickX * handRadius;
        float y = InputManager.ActiveDevice.RightStickY * handRadius;
        //float x = Input.GetAxis("HandHorizontal") * handRadius;
        //float y = Input.GetAxis("HandVertical") * handRadius * (-1);

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
