using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour {

    public float speed;

    private Transform target;
    private Vector3 targetPos;
    private Vector2 targetVector2XPos;
    private Vector2 handVector2XPos;
    private Vector2 playerVector2XPos;
    private Vector2 handSmashVector2;
    private Vector2 handRecoverVector2;
    private float distanceToTargetYPos;
    private enum HandState
    {
        FOLLOW,
        SMASH,
        RECOVER
    }

    private HandState handState = HandState.FOLLOW;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        handVector2XPos = new Vector2(transform.position.x, 0);
        //playerVector2XPos = new Vector2(target.position.x, 0);
        handRecoverVector2 = new Vector2(transform.position.x, 5);
        distanceToTargetYPos = target.position.y - transform.position.y;

        if (Vector2.Distance(handVector2XPos, targetVector2XPos) > 5 || handState == HandState.FOLLOW)
        {
            Debug.Log("HandState = FOLLOW");
            targetVector2XPos = new Vector2(target.position.x, 5);
            transform.position = Vector2.MoveTowards(transform.position, targetVector2XPos, speed * Time.deltaTime);
            Debug.Log("HandState FOLLOW position: " + Vector2.Distance(handVector2XPos, targetVector2XPos));

            if (Vector2.Distance(handVector2XPos, targetVector2XPos) <= 5)
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
}
