using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpikeScript : MonoBehaviour
{

    GameObject bossObject;//This is for referencing the bass body when the hand gets stuck on the spike

    public bool isAttached = false;
    public GameObject player;
    public GameObject playerHand;
    GameObject chainLink; // The"handle" instantiated 
    TargetJoint2D myTarget; // Target joint on the temporary handle
    SpringJoint2D mySpring;
    Rigidbody2D myRigidBody; //RigidBody for temporary handle(for Target Joint)

    public bool hasHandle = false;

    // Use this for initialization

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BossSPike ontreigger");
        if (other.gameObject.CompareTag("BossSmashHand")){
            other.transform.gameObject.SendMessage("SetStuck");
            //bossObject.SendMessage("SetState", "STUCK");
        }
    }
    public void SetHasHandle(bool a)
    {
        hasHandle = a;
    }
    public void SetPlayer(GameObject p)
    {
        player = p;
    }
    public void SetPlayerHand(GameObject h)
    {
        playerHand = h;
    }
    public void SetIsAttached(bool a)
    {
        isAttached = a;
    }
    public void AddHandle()
    {


        chainLink = Instantiate(Resources.Load("chainLink", typeof(GameObject))) as GameObject;

        //gameObject.transform.parent = chainLink.transform;
        //chainLink.transform.parent = gameObject.transform;
        myRigidBody = chainLink.GetComponent<Rigidbody2D>();

        mySpring = chainLink.GetComponent<SpringJoint2D>();
        mySpring.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        myTarget = chainLink.GetComponent<TargetJoint2D>();
        myTarget.frequency = 10;
        myTarget.maxForce = 10000;
        myTarget.autoConfigureTarget = false;
        myTarget.transform.position = gameObject.transform.position;
        myTarget.anchor = new Vector2(0, 0);
        myTarget.target = new Vector2(playerHand.transform.position.x, playerHand.transform.position.y);
        //myTarget.connectedBody = playerHand.GetComponent<Rigidbody2D>();
        hasHandle = true;

    }



    void Start()
    {
        //handle = gameObject.transform.Find("RChainEnd").gameObject;
        bossObject = GameObject.FindWithTag("BossBody");

    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.GetComponent<TargetJoint2D>().target = new Vector2(playerHand.transform.position.x, playerHand.transform.position.y);

        //(playerHand.transform.position.x, playerHand.transform.position.y);
        //print("targetPosX"+ gameObject.transform.GetComponent<TargetJoint2D>().target.x);

    }

    private void FixedUpdate()
    {
        //gameObject.transform.GetComponent<TargetJoint2D>().target.Set(playerHand.transform.position.x, playerHand.transform.position.y);
        if (isAttached)
        {
            myTarget.target = new Vector2(playerHand.transform.position.x, playerHand.transform.position.y);
        }
        else
        {
            gameObject.transform.parent = gameObject.transform;
            Destroy(chainLink);
            hasHandle = false;
            isAttached = false;
        }
    }

}
