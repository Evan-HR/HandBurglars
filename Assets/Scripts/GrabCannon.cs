using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCannon : MonoBehaviour
{


    float circleRad = 10;

    public const float MAX_ANGLE = 30;

    public float targetAngle;
    public float currentAngle;

    public float difference;
    public const float MAX_HAND_DISTANCE = 10;
    public float handCannonDist;

    public float handXAngle;
    public float cannonPointingAngle;

    public float a, o;//x,y components of point on circle(circumference)
    public Vector2 circPoint;

    public bool isAttached = false;
    public GameObject player;

    public GameObject playerHand;
    public Vector2 playerHandLocalPos;

    public GameObject cannonStem;
    public GameObject cannonEnd;

    Rigidbody2D myRigidBody;




    public void SetPlayer(GameObject p)
    {
        player = p;
    }
    public void SetPlayerHand(GameObject h)
    {
        playerHand = h;
    }

    public bool GetIsAttached()
    {
        return isAttached;
    }
    public void SetIsAttached(bool a)
    {
        isAttached = a;
    }
    public void ConnectHand()
    {
        playerHandLocalPos = (((Vector2)playerHand.transform.position) - (Vector2)transform.position);
        handXAngle = Vector2.Angle(playerHandLocalPos, new Vector2(1, 0));
        a = circleRad * Mathf.Cos(handXAngle);
        o = circleRad * Mathf.Sin(handXAngle);
        //circPoint = cannonRotatePos + new Vector2(a,0) + new Vector2(0,o);

        handCannonDist = Vector2.Distance((Vector2)transform.position, (Vector2)playerHand.transform.position);

        isAttached = true;

    }

    public void DisconnectHand()
    {
        player = null;
        playerHand = null;
        isAttached = false;
    }

    public void MakeVisible(){
        gameObject.layer = LayerMask.NameToLayer("Cannon");

        cannonStem.layer = LayerMask.NameToLayer("Cannon");
        cannonEnd.layer = LayerMask.NameToLayer("Cannon");
    }

    private void Start()
    {
        //cannonRotatePos = new Vector2(gameObject.transform.position.x + gameObject.GetComponent<CircleCollider2D>().offset.x, gameObject.transform.position.y + gameObject.GetComponent<CircleCollider2D>().offset.y);
        //transform.Rotate(Vector3.forward * Time.deltaTime*2);
        cannonStem = gameObject.transform.Find("cannonStem").gameObject;
        cannonEnd = gameObject.transform.Find("cannonEnd").gameObject;
     
        currentAngle = 0;

    }

    private void Update()
    {
        //transform.Rotate(Vector3.forward * Time.deltaTime*10);
        //gameObject.transform.GetComponent<TargetJoint2D>().target.Set(playerHand.transform.position.x, playerHand.transform.position.y);
        handCannonDist = playerHandLocalPos.magnitude;

        if (handCannonDist > MAX_HAND_DISTANCE)
        {
            DisconnectHand();
        }
        if (isAttached)
        {

            playerHandLocalPos = (((Vector2)playerHand.transform.position) - (Vector2)transform.position);
            handXAngle = Vector2.Angle(new Vector2(1, 0), playerHandLocalPos);

            if (playerHand.transform.position.y > gameObject.transform.position.y)
            {
                if (handXAngle > MAX_ANGLE)
                {
                    targetAngle = MAX_ANGLE;

                }
                else
                {
                    targetAngle = handXAngle;


                }
            }
            else
            {
                if (handXAngle > MAX_ANGLE)
                {
                    targetAngle = -MAX_ANGLE;


                }
                else
                {
                    targetAngle = -handXAngle;


                }
            }
            difference = targetAngle - currentAngle;


            if (!(difference == 0))
            {
                transform.Rotate(Vector3.forward, difference);
                currentAngle += difference;
            }




        }
        else

        {

            isAttached = false;
        }
    }
}
