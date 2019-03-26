using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should be only used once when initialize players
//And is only used for keyboard players
public class KeyboardInput : MonoBehaviour
{
    const int maxkeyboardPlayers = 1;
    const string JUMP = "jump";
    const string GRAB = "grab";
    const string USE = "use";
    const string MOVELADDERUP = "moveLadderUp";
    const string MOVElADDERDOWN = "moveLadderDown";
    const string HIDE = "hide";

    static int numOfKeyboardPlayers = 0;

    //Dictionary to contains all inputs mapping
    IDictionary<string, KeyCode> collectInputs = new Dictionary<string, KeyCode>();
    string controlType;

    public KeyboardInput()
    {
        numOfKeyboardPlayers++;
        if (numOfKeyboardPlayers <= maxkeyboardPlayers)
        {
            setupKeyboardPlayers();
        }
    }

    public void setupKeyboardPlayers()
    {
        collectInputs[JUMP] = KeyCode.Space;
        collectInputs[HIDE] = KeyCode.Q;
        collectInputs[MOVELADDERUP] = KeyCode.W;
        collectInputs[MOVElADDERDOWN] = KeyCode.D;
        collectInputs[GRAB] = KeyCode.Mouse0;
        collectInputs[USE] = KeyCode.E;
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

    public float getHorizontalMovement() { return Input.GetAxisRaw("Horizontal"); }


    public float getVerticalMovement() { return Input.GetAxisRaw("Vertical"); }

}
