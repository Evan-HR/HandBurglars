using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_Scene1Manager : MonoBehaviour
{
    public GameObject throwBoxPrefab;
    public Transform spawnPoint;
    MovingButton button1;
    MovingButton button2;
    SliderJoint2D BridgeSlideJoint;
    GameObject throwBox;


    // Start is called before the first frame update
    void Start()
    {
        button1 = GameObject.Find("Button1").GetComponent<MovingButton>();
        button2 = GameObject.Find("Button2").GetComponent<MovingButton>();

        BridgeSlideJoint = GameObject.Find("Bridge").GetComponent<SliderJoint2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (button1.justPressed){
            if (throwBox != null){ 
                Destroy(throwBox);
            }
            throwBox = Instantiate(throwBoxPrefab, spawnPoint.position, spawnPoint.rotation);
        }


        if (button2.wasPressed){
            BridgeSlideJoint.useMotor = true;
        }
    }
}
