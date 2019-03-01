using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {
    public float distance;
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public float enemyDetectionSpeed;
    public Transform[] moveSpots;
    private int randomSpot;
    private Vector3 playerPosition;
    public int playerHandLayer;
    private PlayerController playerController;
    public enum DetectionState{
        NOT_DETECTED,
        DETECTED
    }
    public DetectionState detectionState = DetectionState.NOT_DETECTED;

	// Use this for initialization
	void Start () {
        playerHandLayer = (LayerMask.GetMask("PlayerBody"));
        playerController = GameObject.FindObjectOfType<PlayerController>();
        waitTime = startWaitTime;
        randomSpot = 0;
        //randomSpot = Random.Range(0, moveSpots.Length);
	}
	
	// Update is called once per frame
	void Update () {
        if (!playerController.getHideStatus())
        {
            RaycastHit2D detectionRaycast = Physics2D.Raycast(transform.position, transform.right, distance, playerHandLayer);

            //Debug.Log("HandState = DISABLED");
            //controls direction of raycast based on which MoveSpot Critter is moving towards
            if (randomSpot == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (randomSpot == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }


            if (detectionRaycast.collider != null)
            {
                //Debug.DrawLine(transform.position, detectionRaycast.point, Color.red);
                detectionState = DetectionState.DETECTED;
                playerPosition = detectionRaycast.collider.gameObject.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, playerPosition, enemyDetectionSpeed * Time.deltaTime);
            }
            else
            {
                detectionState = DetectionState.NOT_DETECTED;
                //Debug.DrawLine(transform.position, transform.position + transform.right * distance, Color.green);

            }
        } else {
            detectionState = DetectionState.NOT_DETECTED;
        }
        

        MoveToMoveSpot();    
    }

    public bool HasDetected()
    {
        if (detectionState == DetectionState.NOT_DETECTED){
            return false;
        } else {
            return true;
        }
    }

    private void MoveToMoveSpot()
    {
        transform.position = Vector2.MoveTowards(transform.position, (moveSpots[randomSpot].position), speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                if (randomSpot == 0)
                {
                    randomSpot = 1;
                }
                else
                {
                    randomSpot = 0;
                }
                //randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
