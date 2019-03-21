using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyStrokeInputs : MonoBehaviour
{
    //To do, cannon/spike grab and cannon shoot
    const int maxkeyboardPlayers = 1;
    const string JUMP            = "jump";
    const string GRAB            = "grab";
    const string USE             = "use";
    const string MOVELADDERUP    = "moveLadderUp";
    const string MOVElADDERDOWN  = "moveLadderDown";
    const string HIDE            = "hide";

    static int numOfKeyboardPlayers = 0;
    
    //Dictionary to contains all inputs mapping
    IDictionary<string, KeyCode> collectInputs = new Dictionary<string, KeyCode>();
    string controlType;
    //Default input mappings
    //Assuming palyers by default uses keyboard
    public PlayerKeyStrokeInputs()
    {
        numOfKeyboardPlayers++;
        if (numOfKeyboardPlayers <= maxkeyboardPlayers)
        {
            setupKeyboardPlayers();
        }
    }

    public PlayerKeyStrokeInputs(string controlType)
    {
        //Support two joystick at this time
        if (controlType != "joystick1" && controlType != "joystick2")
        {
            this.controlType = controlType;
            if (controlType == "joystick1")
            {
                collectInputs[JUMP]  = KeyCode.Joystick1Button1;
                collectInputs[HIDE]  = KeyCode.Joystick1Button2;
                collectInputs[GRAB]  = KeyCode.Joystick1Button7;
                collectInputs[USE]   = KeyCode.Joystick1Button6;
            }
            else
            {
                collectInputs[JUMP] = KeyCode.Joystick2Button1;
                collectInputs[HIDE] = KeyCode.Joystick2Button2;
                collectInputs[GRAB] = KeyCode.Joystick2Button7;
                collectInputs[USE]  = KeyCode.Joystick2Button6;
            }
        }
    }

    public void setupKeyboardPlayers()
    {
        this.controlType              = "keyboard";
        collectInputs[JUMP]           = KeyCode.Space;
        collectInputs[HIDE]           = KeyCode.Q;
        collectInputs[MOVELADDERUP]   = KeyCode.W;
        collectInputs[MOVElADDERDOWN] = KeyCode.D;
        collectInputs[GRAB]           = KeyCode.Mouse0;
        collectInputs[USE]            = KeyCode.E;
    }

    public string getKeyStatus(string key)
    {
        if (Input.GetKeyDown(collectInputs[key])) return "down";
        else if (Input.GetKeyUp(collectInputs[key])) return "up";
        else if (Input.GetKey(collectInputs[key])) return "hold";
        else return "Nothing";
    }

    public void setKeyCode(string keyName, KeyCode keycode)
    {
        collectInputs[keyName] = keycode;
        Debug.Log(keyName + " is set to keyboard stroke" + keycode.ToString());
    }

    //These only works only these variable name is set in the input manager
    //which is not different with single keystroke
    public float getHorizontalMovement(string axis="none")
    {
        switch (controlType)
        {
            case "keyboard":
                return Input.GetAxisRaw("Horizontal");
            //It should work no matter it is a ps4 controller or a xbox controller
            case "joystick1":
                if (axis == "left") return Input.GetAxisRaw("joystick1LeftHorizontal");
                else if (axis == "right") return Input.GetAxisRaw("joystick1RightHorizontal");
                return 0f;
            case "joystick2":
                if (axis == "left") return Input.GetAxisRaw("joystick2LeftHorizontal");
                else if (axis == "right") return Input.GetAxisRaw("joystick2RightHorizontal");
                return 0f;
            default:
                Debug.LogError("Controller type is not supported!");
                //Don't move if controller type is not supported
                return 0;
        }
    }

    public float getVerticalMovement(string axis = "none")
    {
        switch (controlType)
        {
            case "keyboard":
                return Input.GetAxisRaw("Vertical");
            case "joystick1":
                if (axis == "left") return Input.GetAxisRaw("joystick1LeftVertical");
                else if (axis == "right") return Input.GetAxisRaw("joystick1RightVertical");
                return 0f;
            case "joystick2":
                if (axis == "left") return Input.GetAxisRaw("joystick2LeftVertical");
                else if (axis == "right") return Input.GetAxisRaw("joystick2RightVertical");
                return 0f;
            default:
                Debug.LogError("Controller type is not supported!");
                //Don't move if controller type is not supported
                return 0;
        }
    }
}
