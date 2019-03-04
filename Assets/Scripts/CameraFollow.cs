using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player1;              //Player1 
    
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
    private float maxOrtho = 48.0f;         //The maximum camera size

    private float backgroundMinX;
    private float backgroundMinY;
    private float backgroundMaxX;
    private float backgroundMaxY;

    // edges of the camera
    private float cameraLeft;
    private float cameraRight;
    private float cameraTop;
    private float cameraBottom;

    // bounds of the scene
    private float leftBound;
    private float rightBound;
    private float bottomBound;
    private float topBound;

    // flags for offscreen camera edges

    private bool offLeft;
    private bool offRight;
    private bool offTop;
    private bool offBottom;

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
        // Set the position of the new camera's transform to be the center of two players
        float tempX = (player1.transform.position.x + player2.transform.position.x) / 2;
        float tempY = (player1.transform.position.y + player2.transform.position.y) / 2;

        //left bound and right bound are calculated based on new camera size
        camHalfWidth = Camera.main.aspect * targetOrtho;

        leftBound = backgroundMinX;
        rightBound = backgroundMaxX;
        bottomBound = backgroundMinY;
        topBound = backgroundMaxY;

        positionx = Mathf.Clamp(tempX, leftBound, rightBound);
        
        positiony = Mathf.Clamp(tempY, bottomBound, topBound);

        oldCameraCenter = transform.position;
        newCameraCenter = new Vector3(positionx, positiony, positionz);

        float distCovered = Time.deltaTime * cameraCenterMovingSpeed;

        float distanceBetweenTwoCenters = Vector3.Distance(oldCameraCenter, newCameraCenter);


        cameraLeft = newCameraCenter.x - camHalfWidth;
        cameraRight = newCameraCenter.x + camHalfWidth;
        cameraBottom = newCameraCenter.y - targetOrtho;
        cameraTop = newCameraCenter.y + targetOrtho;

        // if (cameraLeft < leftBound)
        // {
        //     newCameraCenter.x += (leftBound - cameraLeft );
        // } else if (cameraRight > rightBound)
        // {
        //     newCameraCenter.x -= (cameraRight - rightBound);
        // } else if (cameraTop > topBound)
        // {
        //     newCameraCenter.y -= (cameraTop - topBound);
        // } else if (cameraBottom < bottomBound)
        // {
        //     newCameraCenter.y += (bottomBound - cameraBottom);
        // }

        // calculate and move the camera
        // if the camera is now out of bounds, move it back within bounds
        // if the camera is not out of bounds, the camera can be expanded. 


        

        //For each frame, camera center position is located where moving towards new camera center
        //So distCovered is used to calculate interpolize point
        transform.position = Vector3.Lerp(transform.position, newCameraCenter, distCovered / distanceBetweenTwoCenters);

        //Debug.Log("Camera position is: " + transform.position);
        //Debug.Log("Half height of camera is: " + Camera.main.orthographicSize);
        //Debug.Log("Half width of camera is: " + Camera.main.orthographicSize * Camera.main.aspect);

        //Change camera size based on two players distance
        distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        //distanceX = Vector3.Distance(new Vector3(player1.transform.position.x,0,0), new Vector3(player2.transform.position.x,0,0));
        //Debug.Log("Please let me know the distance between two players: " + distance);

        if (distance > 16)
        {
            targetOrtho += zoomCoef * zoomSpeed;
        } else if (distance < 2)
        {
            targetOrtho -= zoomCoef * zoomSpeed;
        }
        distance = Mathf.Clamp(distance, minOrtho, maxOrtho);
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, distance, smoothSpeed * Time.deltaTime);
        //distance = Mathf.Clamp(distance, minOrtho, maxOrtho);
        //Camera.main.orthographicSize = distance;
    }

    void Update()
    {
        
    }
}