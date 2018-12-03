using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player1;       //Public variable to store a reference to the player game object

    public GameObject player2;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private float distance;

    private float zoomSpeed = 1;

    private float targetOrtho;

    private float smoothSpeed = 2.0f;

    private float zoomCoef = 2.0f;

    private float minOrtho = 15.0f;
    private float maxOrtho = 23.0f;

    // Use this for initialization
    void Start()
    {
        distance = Vector2.Distance(player1.transform.position, player2.transform.position);
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player1.transform.position;
        //Debug.Log("The distance between two player is: " + distance);
        targetOrtho = Camera.main.orthographicSize;

        Debug.Log("Original Ortho at start step: " + targetOrtho);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player1.transform.position + offset;
    }

    void Update()
    {
        distance = Vector2.Distance(player1.transform.position, player2.transform.position);
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