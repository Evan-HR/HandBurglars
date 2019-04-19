using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollow : MonoBehaviour {

    public float speed;
    public float bossDuckSpeed;

    private Transform player1Transform;
    private Transform player2Transform;
    private Vector2 player1Vector2X;
    private Vector2 player2Vector2X;
    private Vector2 player1Vector2;
    private Vector2 player2Vector2;
    private Vector2 bossCurrentVector2;
    private Vector2 bossMoveVector2;
    private Vector2 handVector2XPos;
    private Vector2 playerVector2XPos;
    private Vector2 handSmashVector2;
    private Vector2 handRecoverVector2;
    private Vector2 targetPlayerVector2;
    private Rigidbody2D bossGroundRigidBody2D;
    private float distanceToTargetYPos;
    private float bossBodyStartYPos;
    private float bossBodyYPos;
    public float bossHandStateTime;
    private Transform target;
    private Vector3 targetPos;
    private Vector2 targetVector2XPos;
    private Transform bossSmashHandTransform;
    private GameObject bossSmashHandGameObject;
    private GameObject bossGroundBoxGameObject;
    private BoxCollider2D bossGroundBoxBoxCollider2D;
    private BossHandSmashBehaviour bossHandBehaviourScript;
    private BossHandSmashBehaviour.HandState handSmashState;
    private HandStage handStage = HandStage.START;
    private bool isDucking = false;
    private bool canDuck = true;
    private float duckMaxScale = 0.1f;
    private Vector3 bossBodyScaleOriginalVector3;
    private Vector3 bossBodyPosOrignalVector3;
    private float bossBodyYScale;
    private float bossBodyYScaleOriginal;
    private float bossBodyXPosOriginal;
    private float bossBodyYPosOriginal;
    private float bossBodyZPosOriginal;
    private int bossHealth = 2;
    private BossState bossState = BossState.CAN_DUCK;
    public float shakeSpeed;
    public float shakeMagnitude;
    private Vector2 deathVector;
    public float deathSpeed;
    public float hpStage2Speed;
    public float hpStage3Speed;
    private SpriteRenderer bossFollowSpriteRender;
    private SpriteRenderer bossHandSpriteRenderer;
    public Sprite BossMouthOpenSprite;
    public Sprite BossMouthCloseSprite;
    private Vector2 leftEndVector2;
    private Vector2 rightEndVector2;
    private bool isMoveLeft;

    private GameObject bossSceneManager;


    //private bool isHandAttacking = false;

    public enum HandStage
    {
		START,
        SMASH,
        SWIPE,
        STUCK
    }

    public enum BossState
    {
        CAN_DUCK,
        CAN_MOVE,
        CANNOT_DUCK,
        DEAD,
        HAND_STUCK
    }

    // Use this for initialization
    void Start () {
        bossSceneManager = GameObject.FindGameObjectWithTag("LevelManager");
        bossBodyScaleOriginalVector3 = transform.localScale;
        bossBodyYScaleOriginal = transform.localScale.y;
        bossBodyYScale = bossBodyYScaleOriginal;
        bossBodyStartYPos = transform.position.y;
        bossBodyYPos = bossBodyStartYPos;
        bossSmashHandGameObject = GameObject.FindGameObjectWithTag("BossSmashHand");
        bossHandBehaviourScript = bossSmashHandGameObject.GetComponent<BossHandSmashBehaviour>();
        handSmashState = bossHandBehaviourScript.GetHandState();
        bossGroundBoxGameObject = GameObject.FindGameObjectWithTag("BossGroundBox");
        bossGroundBoxBoxCollider2D = bossGroundBoxGameObject.GetComponent<BoxCollider2D>();
        player1Transform = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
        player2Transform = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
        bossFollowSpriteRender = this.GetComponent<SpriteRenderer>();
        bossHandSpriteRenderer = bossSmashHandGameObject.GetComponent<SpriteRenderer>();
        leftEndVector2 = new Vector2(-50f, bossBodyYPos);
        rightEndVector2 = new Vector2(11f, bossBodyYPos);
    }

    // Update is called once per frame
    void Update () {
		player1Vector2X = new Vector2(player1Transform.position.x, 0);
        player2Vector2X = new Vector2(player2Transform.position.x, 0);
        player1Vector2 = new Vector2(player1Transform.position.x, player1Transform.position.y);
        player2Vector2 = new Vector2(player2Transform.position.x, player2Transform.position.y);
        bossCurrentVector2 = new Vector2(transform.position.x, transform.position.y);
        //Vector3 v3Scale = transform.localScale;
        //if CannonShoots and BossHand is not on Spike, BossBody will duck

        if (bossState == BossState.CAN_MOVE)
        {
            bossFollowSpriteRender.sprite = BossMouthCloseSprite;

            this.GetComponent<CircleCollider2D>().enabled = false;

            //Expand this when you want to handle boss ducking
            //if (isDucking && (bossBodyYScale >= duckMaxScale))
            //{
            //    Debug.Log("Boss Duck");
            //    bossBodyYScale -= 0.3f;
            //    bossBodyYPos -= 5f;
            //    transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScale, bossBodyScaleOriginalVector3.z);
            //}
            //else if (isDucking && (bossBodyYScale < duckMaxScale))
            //{
            //    isDucking = false;
            //}
            //else if (!isDucking && (bossBodyYScale <= bossBodyYScaleOriginal))
            //{
            //    bossBodyYScale += 0.3f;
            //    bossBodyYPos += 5f;
            //    transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScale, bossBodyScaleOriginalVector3.z);
            //}
            //else if (!isDucking && (bossBodyYScale > bossBodyYScaleOriginal))
            //{
            //    bossBodyYPos = bossBodyStartYPos;
            //    transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScaleOriginal, bossBodyScaleOriginalVector3.z);
            //}

        }
        else if (bossState == BossState.DEAD)
        {
            transform.position = new Vector2(transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, deathVector, deathSpeed * Time.deltaTime);
        }
        else if (bossState == BossState.HAND_STUCK)
        {
            this.GetComponent<CircleCollider2D>().enabled = true;
            bossFollowSpriteRender.sprite = BossMouthOpenSprite;


            switch (bossHealth)
            {
                case 1:
                    BossStunMovementControl(hpStage2Speed);
                    break;
                case 0:
                    BossStunMovementControl(hpStage3Speed);
                    break;
            }
            
        }
        else
        {

            //Uncomment below when handling ducking
            //bossBodyYPos = bossBodyStartYPos;
            //transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScaleOriginal, bossBodyScaleOriginalVector3.z);

        }

        //BossBody will follow the closest player from it's position
        if (bossState != BossState.DEAD && bossState != BossState.HAND_STUCK)
        {
            if (Vector2.Distance(bossCurrentVector2, player1Vector2) >= Vector2.Distance(bossCurrentVector2, player2Vector2))
            {
                bossMoveVector2 = new Vector2(player2Transform.position.x, bossBodyYPos);
                transform.position = Vector2.MoveTowards(transform.position, bossMoveVector2, speed * Time.deltaTime);
                targetPlayerVector2 = new Vector2(player2Transform.position.x, player2Transform.position.y);
            }
            else
            {
                bossMoveVector2 = new Vector2(player1Transform.position.x, bossBodyYPos);
                transform.position = Vector2.MoveTowards(transform.position, bossMoveVector2, speed * Time.deltaTime);
                targetPlayerVector2 = new Vector2(player1Transform.position.x, player1Transform.position.y);
            }
        }


        //timer to initiate bossSmashHand to attack
        bossHandStateTime -= Time.deltaTime;
        //Debug.Log("bossHandStateTime: " + bossHandStateTime);
        //Debug.Log("playerContains: " + bossGroundBoxBoxCollider2D.bounds.Contains(targetPlayerVector2));
        //if (bossHandStateTime < 0 && !isHandAttacking && (bossGroundBoxBoxCollider2D.bounds.Contains(targetPlayerVector2)))
        //if (bossHandStateTime < 0 && !isHandAttacking && handStage == HandStage.START)
        if (bossHandStateTime < 0 && handStage == HandStage.START)
        {
            //Debug.Log("Set HandState to Follow");
            //isHandAttacking = true;
            //bossHandStateTime = 5;
            handSmashState = BossHandSmashBehaviour.HandState.FOLLOW;
            bossHandBehaviourScript.SetHandState(handSmashState);           
            handStage = HandStage.SMASH;
        }

       // else if (bossHandStateTime < 0 && handStage == HandStage.SMASH)
       // {

            //     bossHandStateTime = 5;
            //     handSmashState = BossHandSmashBehaviour.HandState.START;
            //    bossHandBehaviourScript.SetHandState(handSmashState);
            //    handStage = HandStage.START;
            // }



    }

    void OnCollisionStay2D(Collision2D col)
    {
        string colliderName = col.gameObject.name;
        Debug.Log("BossFollow colliderName " + colliderName);

        if (colliderName == "throwableBomb" && bossState != BossState.DEAD && bossSceneManager.GetComponent<SceneManagerBoss>().bombLit)
        {
           bossSceneManager.SendMessage("explodeBomb");
            FindObjectOfType<AudioManager>().Play("monsterCannonHurt");
            FindObjectOfType<AudioManager>().Play("monsterBallHurt");


            //Destroy(other.gameObject);
            Debug.Log("BossFollow bossHealth " + bossHealth);
            if (bossHealth == 2)
            {
                bossFollowSpriteRender.color = new Color32(255, 112, 112,255);
                bossHandSpriteRenderer.color = new Color32(255, 112, 112,255);
                bossState = BossState.CAN_MOVE;
                bossHandBehaviourScript.SetHandState(BossHandSmashBehaviour.HandState.RECOVER);
            }
            else if (bossHealth == 1)
            {
                bossFollowSpriteRender.color = new Color32(255, 64, 64,255);
                bossHandSpriteRenderer.color = new Color32(255, 64, 64,255);
                bossState = BossState.CAN_MOVE;
                bossHandBehaviourScript.SetHandState(BossHandSmashBehaviour.HandState.RECOVER);
            }
            else if (bossHealth == 0)
            {
                bossFollowSpriteRender.color = new Color32(255, 0, 0,255);
                bossHandSpriteRenderer.color = new Color32(255, 0, 0,255);
                deathVector = new Vector2(transform.position.x, -100);
                this.GetComponent<CircleCollider2D>().enabled = false;
                bossHandBehaviourScript.SetHandState(BossHandSmashBehaviour.HandState.DEAD);
                bossState = BossState.DEAD;
                FindObjectOfType<AudioManager>().Play("victory");
                Initiate.Fade("Victory", Color.white, 0.6f);

            }

            bossHealth -= 1;
            
        }

    }

    private void BossStunMovementControl(float speed)
    {
        if (isMoveLeft && transform.position.x > leftEndVector2.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftEndVector2, speed * Time.deltaTime);
        }
        else if (isMoveLeft && transform.position.x == leftEndVector2.x)
        {
            isMoveLeft = false;
            transform.position = Vector2.MoveTowards(transform.position, rightEndVector2, speed * Time.deltaTime);
        }
        else if (!isMoveLeft && transform.position.x < rightEndVector2.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightEndVector2, speed * Time.deltaTime);
        }
        else if (!isMoveLeft && transform.position.x == rightEndVector2.x)
        {
            isMoveLeft = true;
            transform.position = Vector2.MoveTowards(transform.position, leftEndVector2, speed * Time.deltaTime);
        }
    }
    
    public void Duck()
    {
        if (bossState == BossState.CAN_DUCK)
        {
            isDucking = true;
        }
    }

    public void SetCanDuck(BossState bossState)
    {
        if (this.bossState != BossState.DEAD)
        {
            this.bossState = bossState;
            Debug.Log("BossFollow bossState: " + this.bossState);

            if (bossState == BossState.CANNOT_DUCK)
            {
                isDucking = false;
            }
        }
    }
}
