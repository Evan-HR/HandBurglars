using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //public enum ControlDevice
    //{
    //    NONE,
    //    KEYBOARD,
    //    CONTROLLER_1,
    //    CONTROLLER_2,
    //    CONTROLLER_3,
    //    CONTROLLER_4
    //}

    public enum ControlDevice
    {
        NONE,
        KEYBOARD,
        CONTROLLER
    }

    public ControlDevice controlDevice { get; set; }

    public string characterName { get; set; }

    public PlayerData(ControlDevice controlDevice, string characterName)
    {
        this.controlDevice = controlDevice;
        this.characterName = characterName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
