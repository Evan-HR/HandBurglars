using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingButton : MonoBehaviour
{
    public int id; 
    public bool wasPressed;
    public bool isPressed;

    public bool justPressed;

    GameObject button;
    BoxCollider2D buttonCol;
    GameObject trigger;
    // Start is called before the first frame update
    void Start()
    {
        wasPressed = false;
        isPressed = false;
        justPressed = false;
        button = transform.GetChild(0).gameObject;
        buttonCol = button.GetComponent<BoxCollider2D>();
        trigger = transform.GetChild(1).gameObject;
    }

    public void Update() {
        justPressed = false;

    }
    // called when the button enters the trigger
    public void press() {
        wasPressed = true;
        isPressed = true;
        justPressed = true;
        print("pressButton");

    }

    // called when button leaves the trigger
    public void reset() {
        isPressed = false;
        justPressed = false;
        print("releaseButton");
    }
}
