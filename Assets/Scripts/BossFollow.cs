﻿using System.Collections;
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
    private float bossHandStateTime = 5;
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
    private int bossHealth = 3;

    //private bool isHandAttacking = false;

    public enum HandStage
    {
		START,
        SMASH,
        SWIPE,
        STUCK
    }

    // Use this for initialization
    void Start () {
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
        player1Transform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player2Transform = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();

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

        if (canDuck)
        {
            if (isDucking && (bossBodyYScale >= duckMaxScale))
            {
                Debug.Log("Boss Duck");
                bossBodyYScale -= 0.3f;
                bossBodyYPos -= 5f;
                transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScale, bossBodyScaleOriginalVector3.z);
            }
            else if (isDucking && (bossBodyYScale < duckMaxScale))
            {
                isDucking = false;
            }
            else if (!isDucking && (bossBodyYScale <= bossBodyYScaleOriginal))
            {
                bossBodyYScale += 0.3f;
                bossBodyYPos += 5f;
                transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScale, bossBodyScaleOriginalVector3.z);
            }
            else if (!isDucking && (bossBodyYScale > bossBodyYScaleOriginal))
            {
                bossBodyYPos = bossBodyStartYPos;
                transform.localScale = new Vector3(bossBodyScaleOriginalVector3.x, bossBodyYScaleOriginal, bossBodyScaleOriginalVector3.z);
            }
        }

        //BossBody will follow the closest player from it's position
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
            bossHandStateTime = 5;
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

    
    public void Duck()
    {
        if (canDuck)
        {
            isDucking = true;
        }
    }

    public void SetCanDuck(bool canDuck)
    {
        this.canDuck = canDuck;
        Debug.Log("BossFollow canDuck: " + this.canDuck);

    }
}