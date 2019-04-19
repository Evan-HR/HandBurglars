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
    private float tempBurnTimer;
    public float burnTimer;
    public float shakeSpeed;
    public float shakeMagnitude;
    private Vector2 shakeSpot;
    public float sitTimer;
    private float waitTime;
    public float stunTimer;



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
    private bool isHandSmash = true;

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
        BURNT,
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
        player1Transform = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
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

        //if (handState == HandState.START)
        //{
        //    //Debug.Log("HandState = START");
        //    //bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;
        //    Debug.Log("BossHand enabled");
        //    bossSmashHandGameObject.GetComponent<Renderer>().enabled = true;
        //    handState = HandState.FOLLOW;


        //}
        //else
        //{
        //    //bossSmashHandGameObject.GetComponent<Renderer>().enabled = true;
        //    bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;

        //}


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
                target = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
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
            Debug.Log("BossHandSmashBehaviour HandState.SHAKE");

            if (!isHandSmash)
            {
                this.GetComponent<CircleCollider2D>().enabled = true;
                this.GetComponent<BoxCollider2D>().enabled = true;
            }

            transform.position = new Vector2(transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude, transform.position.y);
            //Debug.Log("HandState SHAKE position: " + Vector2.Distance(bossBodyVector2, handRecoverVector2));

            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                //transform.position = Vector2.MoveTowards(transform.position, handSmashVector2, smashSpeed * Time.deltaTime);

                if (isHandSmash)
                {
                    handState = HandState.SMASH;
                }
                else
                {
                    handState = HandState.RECOVER;
                }

            }
        }


        // ------------------------------------------------ SMASH
        else if (handState == HandState.SMASH)
        //else if (handState == HandState.SMASH)
        {
            //Debug.Log("HandState = SMASH");
            handState = HandState.SMASH;
            handSmashVector2 = new Vector2(transform.position.x, 2);
            transform.position = Vector2.MoveTowards(transform.position, handSmashVector2, smashSpeed * Time.deltaTime);
            //Debug.Log("HandState SMASH position: " + Vector2.Distance(transform.position, handVector2XPos));
            if (Vector2.Distance(handSmashVector2, transform.position) == 0)
            {
                waitTime = sitTimer;
                isHandSmash = false;
                handState = HandState.SIT;
                tempBurnTimer = burnTimer;
            }
        }

        // ------------------------------------------------ SIT
        else if (handState == HandState.SIT)
        {
            Debug.Log("BossHandSmashBehaviour HandState.SIT");

            if (!isHandSmash)
            {
                this.GetComponent<CircleCollider2D>().enabled = true;
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                    handState = HandState.RECOVER;
            }
        }

        // ------------------------------------------------ RECOVER
        //else if ((Vector2.Distance(bossBodyVector2, handVector2XPos) == 0) || handState == HandState.RECOVER)
        else if (handState == HandState.RECOVER)
        {
            // Debug.Log("HandState = RECOVER");
            handState = HandState.RECOVER;
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            transform.position = Vector2.MoveTowards(transform.position, handRecoverVector2, recoverSpeed * Time.deltaTime);
            // Debug.Log("HandState RECOVER position: " + Vector2.Distance(bossBodyVector2, handRecoverVector2));

            if (Vector2.Distance(transform.position, handRecoverVector2) == 0)
            {
                isHandSmash = true;
                handState = HandState.FOLLOW;
            }
        }

         // ------------------------------------------------ BURNT
        else if (handState == HandState.BURNT)
        {
            //transform.position = new Vector2(transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude, transform.position.y);
            Debug.Log("HandState BURNT position: " + Vector2.Distance(bossBodyVector2, handRecoverVector2));

            this.GetComponent<SpriteRenderer>().color = new Color32(255, 64, 64,255);
            waitTime -= Time.deltaTime;
            
            // reel upwards
            transform.position = Vector2.MoveTowards(transform.position, handRecoverVector2, recoverSpeed * Time.deltaTime);

            if (waitTime <= 0)
            {
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255,255);
                handState = HandState.FOLLOW;
                bossFollow.SetCanDuck(BossFollow.BossState.CAN_MOVE);
            }
        }

        else if (handState == HandState.DEAD)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
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

    public void EnableBossHand()
    {
        Debug.Log("BossHand enabled");
        bossSmashHandGameObject.GetComponent<Renderer>().enabled = true;

    }

    public void DisableBossHand()
    {
        Debug.Log("BossHand disabled");
        bossSmashHandGameObject.GetComponent<Renderer>().enabled = false;
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    string colliderName = col.gameObject.name;
    //    Debug.Log("BossHandBehaviour Collider.name " + colliderName);
    //    if (name == "Torch")
    //    {
    //        handState = HandState.BURNT;


    //        //if (other.name == "Spike")
    //        //{
    //        //    bossFollow.SetCanDuck(false);
    //        //    //Destroy(other.gameObject);

    //        //}
    //    }
    //}

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject other = col.gameObject;
        if (other.tag == "TorchFlame" && handState == HandState.SIT)
        {
            tempBurnTimer = burnTimer;

            //destroy(other.gameobject);
            FindObjectOfType<AudioManager>().Play("fireWhoosh");
            FindObjectOfType<AudioManager>().Play("monsterGrowl");
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        GameObject other = col.gameObject;

        if (other.tag == "TorchFlame" && handState == HandState.SIT)
        {
            tempBurnTimer -= Time.deltaTime;
            if (tempBurnTimer <= 0){
                bossFollow.SetCanDuck(BossFollow.BossState.HAND_STUCK);
                waitTime = stunTimer;
                handState = HandState.BURNT;
                FindObjectOfType<AudioManager>().Play("fireWhoosh");
            }
            

            //destroy(other.gameobject);
            
            FindObjectOfType<AudioManager>().Play("monsterGrowl");
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        GameObject other = col.gameObject;

        if (other.tag == "torchFlame" && handState == HandState.SIT)
        {
            tempBurnTimer = burnTimer;
        }

    }

}
