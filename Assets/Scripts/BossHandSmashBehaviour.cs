using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandSmashBehaviour : MonoBehaviour
{

    public float speed;
    public float recoverSpeed;
    public float smashSpeed;
    public float holdTimer;
    public float shakeTimer;
    public float shakeSpeed;
    public float shakeMagnitude;
    private Vector2 shakeSpot;
    public float sitTimer;
    private float waitTime;

    private Transform player1Transform;
    private Transform player2Transform;
    private Vector2 player1Vector2;
    private Vector2 player2Vector2;
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
    private float bossSmashHandStateTime = 5;
    private BossFollow bossFollow;

    // Desired behaviour is follow, shake, smash, sit, recover, hold
    public enum HandState
    {
        DISABLED,
        FOLLOW,
        SHAKE,
        SMASH,
        SIT,
        RECOVER,
        HOLD
    }

    private HandState handState = HandState.DISABLED;

    // Use this for initialization
    void Start()
    {


        bossFollow = GameObject.FindObjectOfType<BossFollow>();
        smashHandInitYPos = transform.position.y;
        bossSmashHandGameObject = GameObject.FindGameObjectWithTag("BossSmashHand");
        player1Transform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player2Transform = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
        player1Vector2 = new Vector2(player1Transform.position.x, player1Transform.position.y);
        player2Vector2 = new Vector2(player2Transform.position.x, player2Transform.position.y);


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


        //if (handState != HandState.DISABLED)
        //{
        //    bossSmashHandStateTime -= Time.deltaTime;
        //    Debug.Log("bossSmashHandStateTime: " + bossSmashHandStateTime);


        //    if (bossSmashHandStateTime < 0)
        //    {
        //        handState = HandState.DISABLED;
        //        bossSmashHandStateTime = 5;
        //    }
        //}

        if (handState == HandState.DISABLED)
        {
            Debug.Log("HandState = DISABLED");
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;

        }
        else
        {
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = true;

        }

        //if (Vector2.Distance(handVector2XPos, targetVector2XPos) > 0.5 || handState == HandState.FOLLOW)
        if (handState == HandState.FOLLOW)
        {
            //Check which player is closer to hand
            if (Vector2.Distance(transform.position, player1Vector2) >= Vector2.Distance(transform.position, player2Vector2))
            {
                target = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
                Debug.Log("target Player2");

            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                Debug.Log("target Player1");

            }

            targetVector2XPos = new Vector2(target.position.x, 0);
            targetVector2 = new Vector2(target.position.x, smashHandInitYPos);
            transform.position = Vector2.MoveTowards(transform.position, targetVector2, speed * Time.deltaTime);
            Debug.Log("HandState FOLLOW position: " + Vector2.Distance(handVector2XPos, targetVector2XPos));

            if (Vector2.Distance(handVector2XPos, targetVector2XPos) <= 0.5)
            {
                handState = HandState.SHAKE;
                waitTime = shakeTimer;
                shakeSpot = transform.position;
            }
        }
        // ------------------------------------------------ RECOVER
        else if ((Vector2.Distance(transform.position, handVector2XPos) == 0) || handState == HandState.RECOVER)
        //else if (handState == HandState.RECOVER)
        {
            //Debug.Log("HandState = RECOVER");
            handState = HandState.RECOVER;
            transform.position = Vector2.MoveTowards(transform.position, handRecoverVector2, recoverSpeed * Time.deltaTime);
            Debug.Log(Vector2.Distance(transform.position, handRecoverVector2));

            if (Vector2.Distance(transform.position, handRecoverVector2) == 0)
            {
                handState = HandState.FOLLOW;
            }
        }
        // ------------------------------------------------ SHAKE
        else if (handState == HandState.SHAKE)
        {
            transform.position = new Vector2(shakeSpot.x + Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude, shakeSpot.y);

            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, shakeSpot, smashSpeed * Time.deltaTime);
                handState = HandState.SMASH;
            }
        }
        else if (((Vector2.Distance(handVector2XPos, targetVector2XPos) <= 5) && (Vector2.Distance(handVector2XPos, targetVector2XPos) >= 0)) || handState == HandState.SMASH)
        //else if (handState == HandState.SMASH)
        {
            //Debug.Log("HandState = SMASH");
            handState = HandState.SMASH;
            handSmashVector2 = new Vector2(transform.position.x, 0);
            transform.position = Vector2.MoveTowards(transform.position, handSmashVector2, smashSpeed * Time.deltaTime);
            Debug.Log("HandState SMASH position: " + Vector2.Distance(transform.position, handVector2XPos));
        }
        //else if {}
    }

    public void SetHandState(HandState handState)
    {
        this.handState = handState;
    }

    public HandState GetHandState()
    {
        return handState;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collider.name " + other.name);

        if (other.name == "Spike")
        {
            bossFollow.SetIsDuck(false);
            //Destroy(other.gameObject);

        }
    }
}
