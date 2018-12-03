using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollow : MonoBehaviour {

    public float speed;

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
    private float bossStartYPos;
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
    private bool isHandAttacking = false;

    // Use this for initialization
    void Start () {
        bossStartYPos = transform.position.y;
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

        if (Vector2.Distance(bossCurrentVector2, player1Vector2) >= Vector2.Distance(bossCurrentVector2, player2Vector2))
        {
            bossMoveVector2 = new Vector2(player2Transform.position.x, bossStartYPos);
            transform.position = Vector2.MoveTowards(transform.position, bossMoveVector2, speed * Time.deltaTime);
            targetPlayerVector2 = new Vector2(player2Transform.position.x, player2Transform.position.y);
        }
        else
        {
            bossMoveVector2 = new Vector2(player1Transform.position.x, bossStartYPos);
            transform.position = Vector2.MoveTowards(transform.position, bossMoveVector2, speed * Time.deltaTime);
            targetPlayerVector2 = new Vector2(player1Transform.position.x, player1Transform.position.y);
        }


        //timer to initiate bossSmashHand to attack
        bossHandStateTime -= Time.deltaTime;
        Debug.Log("bossHandStateTime: " + bossHandStateTime);
        Debug.Log("playerContains: " + bossGroundBoxBoxCollider2D.bounds.Contains(targetPlayerVector2));
        //if (bossHandStateTime < 0 && !isHandAttacking && (bossGroundBoxBoxCollider2D.bounds.Contains(targetPlayerVector2)))
        if (bossHandStateTime < 0 && !isHandAttacking)
        {
            Debug.Log("Set HandState to Follow");
            isHandAttacking = true;
            bossHandStateTime = 5;
            handSmashState = BossHandSmashBehaviour.HandState.FOLLOW;
            bossHandBehaviourScript.SetHandState(handSmashState);
        }
        else if (isHandAttacking)
        {

        }

    }

}
