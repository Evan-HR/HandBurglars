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
    private Transform bossBodyTransform;
    private Vector2 player1Vector2;
    private Vector2 player2Vector2;
    private Vector2 bossBodyVector2;
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

    //Fade Hand Upon Death
    public SpriteRenderer bossHandSpriteRenderer;
    private float fade;

    // Desired behaviour is follow, shake, smash, sit, recover, hold
    public enum HandState
    {
        START,
        FOLLOW,
        SHAKE,
        SMASH,
        SIT,
        RECOVER,
        HOLD,
        HIT_SPIKE,
        DEAD
    }

    private HandState handState = HandState.START;

    // Use this for initialization
    void Start()
    {

        // initialize all the hand variables
        Debug.Log("BOSSHANDBE: ");

        bossHandSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        bossFollow = GameObject.FindObjectOfType<BossFollow>();
        smashHandInitYPos = transform.position.y;
        bossSmashHandGameObject = GameObject.FindGameObjectWithTag("BossSmashHand");
        player1Transform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player2Transform = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
        bossBodyTransform = GameObject.FindGameObjectWithTag("BossBody").GetComponent<Transform>();


        if (handState == HandState.START)
        {
            //bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        bossBodyVector2 = new Vector2(bossBodyTransform.position.x, bossBodyTransform.position.y);
        player1Vector2 = new Vector2(player1Transform.position.x, player1Transform.position.y);
        player2Vector2 = new Vector2(player2Transform.position.x, player2Transform.position.y);
        handVector2XPos = new Vector2(transform.position.x, 0);
        //playerVector2XPos = new Vector2(target.position.x, 0);
        handRecoverVector2 = new Vector2(transform.position.x, smashHandInitYPos);


        //if (handState != HandState.START)
        //{
        //    bossSmashHandStateTime -= Time.deltaTime;
        //    Debug.Log("bossSmashHandStateTime: " + bossSmashHandStateTime);


        //    if (bossSmashHandStateTime < 0)
        //    {
        //        handState = HandState.START;
        //        bossSmashHandStateTime = 5;
        //    }
        //}

        if (handState == HandState.START)
        {
            //Debug.Log("HandState = START");
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;

        }
        else
        {
            bossSmashHandGameObject.GetComponent<Renderer>().enabled = true;

        }


        // ------------------------------------------------ FOLLOW
        if (handState == HandState.FOLLOW)
        {
            //Check which player is closest to hand, assign closest player 
            if (Vector2.Distance(bossBodyVector2, player1Vector2) >= Vector2.Distance(bossBodyVector2, player2Vector2))
            {
                target = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
                //Debug.Log("target Player2");

            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                //Debug.Log("target Player1");

            }

            targetVector2XPos = new Vector2(target.position.x, 0);
            targetVector2 = new Vector2(target.position.x, smashHandInitYPos);
            transform.position = Vector2.MoveTowards(transform.position, targetVector2, speed * Time.deltaTime);
            //Debug.Log("HandState FOLLOW position: " + Vector2.Distance(handVector2XPos, targetVector2XPos));

            if (Vector2.Distance(handVector2XPos, targetVector2XPos) <= 0.5)
            {
                handState = HandState.SHAKE;
                waitTime = shakeTimer;
                shakeSpot = transform.position;
            }
        }














        // ------------------------------------------------ SHAKE
        else if (handState == HandState.SHAKE)
        {
            transform.position = new Vector2(shakeSpot.x + Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude, shakeSpot.y);
            //Debug.Log("HandState SHAKE position: " + Vector2.Distance(bossBodyVector2, handRecoverVector2));

            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, shakeSpot, smashSpeed * Time.deltaTime);
                handState = HandState.SMASH;
            }
        }


        // ------------------------------------------------ SMASH
        else if (handState == HandState.SMASH)
        //else if (handState == HandState.SMASH)
        {
            //Debug.Log("HandState = SMASH");
            handState = HandState.SMASH;
            handSmashVector2 = new Vector2(transform.position.x, 0);
            transform.position = Vector2.MoveTowards(transform.position, handSmashVector2, smashSpeed * Time.deltaTime);
            //Debug.Log("HandState SMASH position: " + Vector2.Distance(transform.position, handVector2XPos));
            if (Vector2.Distance(handSmashVector2, transform.position) == 0)
            {
                handState = HandState.RECOVER;
            }
        }
        // ------------------------------------------------ RECOVER
        else if ((Vector2.Distance(bossBodyVector2, handVector2XPos) == 0) || handState == HandState.RECOVER)
        //else if (handState == HandState.RECOVER)
        {
            // Debug.Log("HandState = RECOVER");
            handState = HandState.RECOVER;
            transform.position = Vector2.MoveTowards(transform.position, handRecoverVector2, recoverSpeed * Time.deltaTime);
            // Debug.Log("HandState RECOVER position: " + Vector2.Distance(bossBodyVector2, handRecoverVector2));

            if (Vector2.Distance(transform.position, handRecoverVector2) == 0)
            {
                handState = HandState.FOLLOW;
            }
        }

        else if (handState == HandState.HIT_SPIKE)
        {
            transform.position = new Vector2(transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude, transform.position.y);
            //Debug.Log("HandState SHAKE position: " + Vector2.Distance(bossBodyVector2, handRecoverVector2));

            waitTime -= Time.deltaTime;
            //Debug.Log("WaitTime: " + waitTime);
            if (waitTime <= 0)
            {
                handState = HandState.FOLLOW;
                bossFollow.SetCanDuck(BossFollow.BossState.CAN_DUCK);
            }
        }

        else if (handState == HandState.DEAD)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (fade >= 0) {
                fade -= 0.01f;
                bossHandSpriteRenderer.color = new Color(0.55f, 0, 0, fade);
            }
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

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("BOSS Collider.name " + other.name);

    //    if (other.name == "Spike")
    //    {
    //        bossFollow.SetCanDuck(false);
    //        //Destroy(other.gameObject);

    //    }
    //}

    void OnCollisionStay2D(Collision2D col)
    {
        //Debug.Log("BossHandBehaviour Collider.name " + col.gameObject.name);

        if (col.gameObject.name == "bossSpike")
        {
            //Destroy(other.gameObject);
            if (handState == HandState.SMASH)
            {
                handState = HandState.HIT_SPIKE;
                waitTime = 5;
                bossFollow.SetCanDuck(BossFollow.BossState.CANNOT_DUCK);
            }
        }

    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Boss Collider.name " + collision.gameObject.name);

    //}

}
