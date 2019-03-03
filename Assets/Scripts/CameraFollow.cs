using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player1;              //PLayer1 
    
    public GameObject player2;              //Player2


    private float distance;                 //Distance between two players
    private float distanceX;
    private float positionx;                //New Camera Center x position
    private float positiony;                //New Camera Center y position
    private static float positionz = -10;
    private Vector3 oldCameraCenter;
    private Vector3 newCameraCenter;

    private float zoomSpeed;                //Combined with zoomCoef, decide the target ortho/camera size
    private float cameraCenterMovingSpeed;  //Moving speed from old center to new center
    private float smoothSpeed;              //The speed of moving from old camera size to new camera size
    public float zoomSpeedAdjust;          
    public float cameraCenterMovingSpeedAdjust;
    public float smoothSpeedAdjust;

    private float targetOrtho;              //Camera size which is half of camera height
    private float camHalfWidth;             //Camera size which is half of camera width
    private float zoomCoef = 2.0f;  

    private float minOrtho = 20.0f;         //The minimum camera size
    private float maxOrtho = 28.0f;         //The maximum camera size

    private float backgroundMinX;
    private float backgroundMinY;
    private float backgroundMaxX;
    private float backgroundMaxY;
    private float leftBound;
    private float rightBound;
    private float bottomBound;
    private float topBound;

    public float heightBuffer;

    private SpriteRenderer spriteBounds;

    // Use this for initialization
    void Start()
    {
        zoomSpeed = zoomSpeedAdjust; 
        cameraCenterMovingSpeed = cameraCenterMovingSpeedAdjust;
        smoothSpeed = smoothSpeedAdjust;
        //This is actually size half of the height
        targetOrtho = Camera.main.orthographicSize;
        spriteBounds = GameObject.FindGameObjectWithTag("Background").GetComponentInChildren<SpriteRenderer>();

        backgroundMinX = spriteBounds.bounds.min.x;
        backgroundMinY = spriteBounds.bounds.min.y;
        backgroundMaxX = spriteBounds.bounds.max.x;
        backgroundMaxY = spriteBounds.bounds.max.y;
    }

    // LateUpdate is called after Update method is called
    void LateUpdate()
    {
        float tempX = (player1.transform.position.x + player2.transform.position.x) / 2;
        float tempY = (player1.transform.position.y + player2.transform.position.y) / 2;

        //left bound and right bound are calculated based on new camera size
        camHalfWidth = Camera.main.aspect * targetOrtho;

        leftBound = backgroundMinX + camHalfWidth;
        rightBound = backgroundMaxX - camHalfWidth;
        bottomBound = backgroundMinY + targetOrtho - heightBuffer;
        topBound = backgroundMaxY - targetOrtho;

        // Set the position of the camera's transform to be the center of two players
        positionx = Mathf.Clamp(tempX, leftBound, rightBound);
        
        positiony = Mathf.Clamp(tempY, bottomBound, topBound);

        newCameraCenter = new Vector3(positionx, positiony, positionz);

        oldCameraCenter = transform.position;

        float distCovered = Time.deltaTime * cameraCenterMovingSpeed;

        float distanceBetweenTwoCenters = Vector3.Distance(transform.position, newCameraCenter);

        //For each frame, camera center position is located where moving towards new camera center
        //So distCovered is used to calculate interpolize point
        transform.position = Vector3.Lerp(oldCameraCenter, newCameraCenter, distCovered / distanceBetweenTwoCenters);

        //Debug.Log("Camera position is: " + transform.position);
        //Debug.Log("Half height of camera is: " + Camera.main.orthographicSize);
        //Debug.Log("Half width of camera is: " + Camera.main.orthographicSize * Camera.main.aspect);
    }

    void Update()
    {
        //Change camera size based on two players distance
        distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        distanceX = Vector3.Distance(new Vector3(player1.transform.position.x,0,0), new Vector3(player2.transform.position.x,0,0));
        //Debug.Log("Please let me know the distance between two players: " + distance);

        if (distance > 16)
        {
            targetOrtho += zoomCoef * zoomSpeed;
        } else if (distance < 2)
        {
            targetOrtho -= zoomCoef * zoomSpeed;
        }
        targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }
}