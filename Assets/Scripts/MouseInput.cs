using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : BaseInputController {

    private Vector2 prevMousePos;
    private Vector2 mouseDelta;

    private float speedX = 0.05f;
    private float speedY = 0.1f;

    public void Start(){
        prevMousePos = Input.mousePosition;
    }

    public override void CheckInput()
    {
        float horz = Input.GetAxis("Mouse X");
        float vert = Input.GetAxis("Mouse Y");

        float scalarX = 100f / Screen.width;
        float scalarY = 100f / Screen.height;

        float mouseDeltaY = Input.mousePosition.y - prevMousePos.y;
        float mouseDeltaX = Input.mousePosition.x - prevMousePos.x;

        vert += (mouseDeltaY * speedY) * scalarY;

        horz += (mouseDeltaX * speedX) * scalarX;


        prevMousePos = Input.mousePosition;

        Up = (vert > 0);
        Down = (vert < 0);
        Left = (horz < 0);
        Right = (horz > 0);
    }

    public override float GetHorizontal(){
        return horz;
    }

    public void LateUpdate()
    {
        CheckInput();
    }
}
