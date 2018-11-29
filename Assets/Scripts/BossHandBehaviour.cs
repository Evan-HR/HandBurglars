using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandBehaviour : MonoBehaviour {

    public float speed;

    private Transform target;
    private Vector3 targetPos;
    private Vector2 targetVector2XPos;
    private Vector2 targetVector2;
    private Vector2 handVector2XPos;
    private Vector2 playerVector2XPos;
    private Vector2 handSmashVector2;
    private Vector2 handRecoverVector2;
    private GameObject bossSmashHandGameObject;
    private float distanceToTargetYPos;
    private float smashHandInitYPos;
    public enum HandState
    {
        DISABLED,
        FOLLOW,
        SMASH,
        RECOVER
    }

    private HandState handState = HandState.DISABLED;

    // Use this for initialization
    void Start()
    {
        smashHandInitYPos = transform.position.y;
        bossSmashHandGameObject = GameObject.FindGameObjectWithTag("BossSmashHand");
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targetPos = transform.position;


        if (handState == HandState.DISABLED)
        {
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;


        }
    }

    // Update is called once per frame
    void Update()
    {
        handVector2XPos = new Vector2(transform.position.x, 0);
        //playerVector2XPos = new Vector2(target.position.x, 0);
        handRecoverVector2 = new Vector2(transform.position.x, smashHandInitYPos);
        distanceToTargetYPos = target.position.y - transform.position.y;

        if (handState == HandState.DISABLED)
        {
            Debug.Log("HandState = DISABLED");
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;

        }
        else
        {
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = true;

        }

        if (Vector2.Distance(handVector2XPos, targetVector2XPos) > 0.5 || handState == HandState.FOLLOW)
        {
            Debug.Log("HandState = FOLLOW");
            targetVector2XPos = new Vector2(target.position.x, 0);
            targetVector2 = new Vector2(target.position.x, smashHandInitYPos);
            transform.position = Vector2.MoveTowards(transform.position, targetVector2, speed * Time.deltaTime);
            Debug.Log("HandState FOLLOW position: " + Vector2.Distance(handVector2XPos, targetVector2XPos));

            if (Vector2.Distance(handVector2XPos, targetVector2XPos) <= 0.5)
            {
                handState = HandState.SMASH;
            }
        }
        else if ((Vector2.Distance(transform.position, handVector2XPos) == 0) || handState == HandState.RECOVER)
        {
            Debug.Log("HandState = RECOVER");
            handState = HandState.RECOVER;
            transform.position = Vector2.MoveTowards(transform.position, handRecoverVector2, speed * Time.deltaTime);
            Debug.Log(Vector2.Distance(transform.position, handRecoverVector2));

            if (Vector2.Distance(transform.position, handRecoverVector2) == 0)
            {
                handState = HandState.FOLLOW;
            }
        }
        else if (((Vector2.Distance(handVector2XPos, targetVector2XPos) <= 5) && (Vector2.Distance(handVector2XPos, targetVector2XPos) >= 0)) || handState == HandState.SMASH)
        {
            Debug.Log("HandState = SMASH");
            handState = HandState.SMASH;
            handSmashVector2 = new Vector2(transform.position.x, 0);
            transform.position = Vector2.MoveTowards(transform.position, handSmashVector2, speed * Time.deltaTime);
            Debug.Log("HandState SMASH position: " + Vector2.Distance(transform.position, handVector2XPos));
        }
    }

    public void SetHandState(HandState handState)
    {
        this.handState = handState;
    }

    public HandState GetHandState()
    {
        return handState;
    }
}
