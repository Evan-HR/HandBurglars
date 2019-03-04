using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandBehavior : MonoBehaviour
{
    //Player States
    //Walking
    //Idle
    //Jumping
    //Climbing
    //Hurt(maybe)
    //Dead(disable hand)
    //Disabled




    //Hand States
    //Free
    //Holding
    //Disabled(can't grab)

    SpringJoint2D mySpringJoint;
    GameObject handle;



    //public Transform hand;

    private GameObject player;
    public GameObject hand;
    private GameObject baseInput;
    private GameObject chosenObject;
    private Vector2 handPos;//Hand position in screen coordinates
    private Vector2 handPosLocal;
    private Vector2 directionVector;//Direction hand should point to from the character body
    private Vector2 playerPos;//Position of player relative to screen coords
    private Vector2 playerPosLocal;
    private Vector2 mousePos;//mouse position relative to screen coords
    private Vector2 mousePosLocal;
    private float diff;//for rotation of hand
    private float tempAngle;//for rotation of hand
    private float bottom_Angle;//For rotating hand
    private float top_Angle;//For rotating hand
    private float currentHandAngle;//Current rotation of the hand
    private float handBodyDistance;//distance from hand to player
    private float mouseBodyDistance;//distance from mouse to player
    private bool toggleGrabMode = false; //Toggle for if hand is grabbing or not; 0 -open hand, 1-grab mode
    private bool isHolding = false;
    private List<GameObject> grabbableObjects;
    private int numOfTouchingObjects;

    Sprite grabbingHand;
    bool dragging;

    private Transform tempTransform;//For grabbing objects and making them children of hand object

    private float handRadius = 2.5f;
    private float mouseRadRatio;

    private Vector2 tempPoint;

    public float speed = 0.01f;



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


    void Start()
    {
        dragging = false;
        grabbableObjects = new List<GameObject>();
        chosenObject = null;
        numOfTouchingObjects = 0;
        transform.localPosition = new Vector2(0.0f, 0.0f);//localPosition - local to the parent(player)
        currentHandAngle = 0.0f;
        //baseInput = GameObject.Find("BaseInputController");
        player = GameObject.FindWithTag("Player");
        hand = GameObject.FindWithTag("Hand");

        grabbingHand = Resources.Load("hand04", typeof(Sprite)) as Sprite;

    }

    void checkGrab()
    {
        bool isRMBDown = Input.GetMouseButtonDown(0);
        bool isRMBUp = Input.GetMouseButtonUp(0);
        int indexOfMin = 0;
        float objectDistance;
        if (isHolding == true)
        {
            if (isRMBUp)
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

        if (isRMBDown & !isHolding)
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
                    objectDistance = Vector2.Distance(player.transform.position, (Vector2)grabbableObjects[i].transform.position);
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
        //#TODO restructure the handbehaviour file to this format 

        //Get state of player
        // if player is dead, hand state is set to disabled.

        //Get state of hand
        //IF NOT HOLDING
        //if shift key is down, checkGrab(), and if object match, set hand to holding and attach object to hand
        //we'll need to update isHolding to true, and either isRMBbing isDragging etc.
        //IF GRABBING
        //update position of grabbable with reference to hand, do other shit
        //if shiftUp then set to NOT HOLDING, release obj.
        //IF DRAGGING
        // update anchor position of draggable or whatever
        //if shiftUp then set to NOT HOLDING, release obj.



        checkGrab();




        handPos = (Vector2)gameObject.transform.position;
        handPosLocal = (Vector2)gameObject.transform.localPosition;

        playerPos = (Vector2)player.transform.position;


        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosLocal = mousePos - playerPos;

        handBodyDistance = Vector2.Distance(playerPos, handPos);
        mouseBodyDistance = Vector2.Distance(playerPos, mousePos);


        directionVector = mousePos - playerPos;

        float step = speed * Time.deltaTime;

        //For when hand is outside of player reach
        if (mousePosLocal.magnitude > handRadius)
        {

            if (directionVector.magnitude != 0)
            {
                mouseRadRatio = handRadius / directionVector.magnitude;
            }


            directionVector = directionVector * mouseRadRatio;

            bottom_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, -1.0f));
            top_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, 1.0f));


            //tempPoint = playerPos + directionVector;
            transform.localPosition = directionVector;

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
        else
        {
            bottom_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, -1.0f));
            top_Angle = Vector2.Angle(directionVector, new Vector2(0.0f, 1.0f));

            //tempPoint = Camera.main.ScreenToWorldPoint(mousePos);
            transform.localPosition = directionVector;

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
    }
}
