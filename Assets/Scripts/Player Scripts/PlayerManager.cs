using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    Vector2 mousePos, handPos, bodyMouseVector;
    float bodyMouseDir, bodyMouseMag; // angle of the hand, magnitude of the hand

    public float handRadius;
    Transform handTransform, bodyTransform;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
		handTransform = this.gameObject.transform.GetChild(0);
        bodyTransform = this.gameObject.transform;
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        // HAND POSITION UPDATE
		mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bodyMouseVector = mousePos - (Vector2) gameObject.transform.position;
        print(bodyMouseVector.x + ", " + bodyMouseVector.y);
        bodyMouseDir = Mathf.Rad2Deg * Mathf.Atan2(bodyMouseVector.x, bodyMouseVector.y);
        bodyMouseMag = bodyMouseVector.magnitude;
        handTransform.rotation = Quaternion.Euler(0, 0, 180 -bodyMouseDir);
        if (bodyMouseMag <= handRadius){
            handTransform.position = new Vector2(bodyTransform.position.x + bodyMouseVector.x, bodyTransform.position.y + bodyMouseVector.y);
        } else {
            handTransform.position = new Vector2(bodyTransform.position.x + handRadius  * Mathf.Sin(Mathf.Deg2Rad * bodyMouseDir), bodyTransform.position.y + handRadius * Mathf.Cos(Mathf.Deg2Rad * bodyMouseDir));
        }
        

	}
}
