using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player1;       //PLayer1 

    public GameObject player2;       //Player2

    private float distance;         //Distance between two players

    private float positionx;        //New Camera Center x position
    private float positiony;        //New Camera Center y position
    private static float positionz = -10f;
    private Vector3 oldCameraCenter;
    private Vector3 newCameraCenter;

    private float zoomSpeed; //Combined with zoomCoef, decide the new camera size
    private float cameraCenterMovingSpeed;
    private float smoothSpeed;
    public float zoomSpeedAdjust; //Combined with zoomCoef, decide the new camera size
    public float cameraCenterMovingSpeedAdjust;
    public float smoothSpeedAdjust;

    private float targetOrtho;      //New camera size
    private float zoomCoef = 2.0f;  

    private float minOrtho = 15.0f; //The minimum camera size
    private float maxOrtho = 26.0f; //The maximum camera size

    // Use this for initialization
    void Start()
    {

            zoomSpeed = zoomSpeedAdjust; 
    cameraCenterMovingSpeed = cameraCenterMovingSpeedAdjust;
    smoothSpeed = smoothSpeedAdjust;

    targetOrtho = Camera.main.orthographicSize;

        Debug.Log("Original Ortho at start step: " + targetOrtho);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the center of two players
        positionx = (player1.transform.position.x + player2.transform.position.x) / 2;

        positiony = (player1.transform.position.y + player2.transform.position.y) / 2;

        newCameraCenter = new Vector3(positionx, positiony, positionz);

        oldCameraCenter = transform.position;

        float distCovered = Time.deltaTime * cameraCenterMovingSpeed;

        float distanceBetweenTwoCenters = Vector3.Distance(transform.position, newCameraCenter);

        transform.position = Vector3.Lerp(oldCameraCenter, newCameraCenter, distCovered / distanceBetweenTwoCenters);
    }

    void Update()
    {
        //Change camera size based on two players distance
        distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        Debug.Log("Please let me know the distance between two players: " + distance);

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