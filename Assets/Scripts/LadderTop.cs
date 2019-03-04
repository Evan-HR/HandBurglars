using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTop : MonoBehaviour {
    private PlatformEffector2D effector;
    public float waitTime;
	// Use this for initialization
	void Start () {
        effector = GetComponent<PlatformEffector2D>();
		
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.S)|| Input.GetAxisRaw("LeftJoystickVertical") < 0)
        {
            //print("DOWN IS PRESSED!");
            waitTime = 0.1f;
            //effector.rotationalOffset = 0;
        }
        if (Input.GetKey(KeyCode.S)|| Input.GetAxisRaw("LeftJoystickVertical") < 0)
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.1f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.W)|| Input.GetAxisRaw("LeftJoystickVertical") > 0)
        {
            //print("UP IS PRESSED!");
            effector.rotationalOffset = 0;
        }

	}
}
