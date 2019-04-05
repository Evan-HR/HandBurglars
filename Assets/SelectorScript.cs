using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;


public class SelectorScript : MonoBehaviour {

    public class CharacterSelect
    {
        public string PlayerName;
        public string ControllerType;
        public int PlayerNum;

        public CharacterSelect(string name, string control, int num){
                PlayerName = name;
                ControllerType = control;
                PlayerNum = num;
        }
    }

    public GameObject Bru;
    public GameObject Jotun;

    private List<string> characterList = new List<string>(new string[] { "Bru", "Jotun" });
    //private List<string> deviceList = new List<string>(new string[] { "Keyboard", "Controller" });
    private List<PlayerData.ControlDevice> deviceList = new List<PlayerData.ControlDevice>(new PlayerData.ControlDevice[] 
    {
        PlayerData.ControlDevice.KEYBOARD,
        PlayerData.ControlDevice.CONTROLLER_1,
        PlayerData.ControlDevice.CONTROLLER_2,
        PlayerData.ControlDevice.CONTROLLER_3,
        PlayerData.ControlDevice.CONTROLLER_4
    });
    private int characterIndex = 0;
    private int deviceIndex = 0;


    private int SelectorInt = 1;
    //gets the component, does it once upon start 
    private SpriteRenderer JotunRender, BruRender, keyboardRender, controllerRender;
    public GameObject ControllerIcon;
    public GameObject KeyboardIcon;


    
    private CharacterSelect[] characterInfo;

    
	public int changeTextCounter = 0;
	public GameObject topText;
    //TRUE if on player select, false if on CONTROLLER select
    public bool playerSelectBool = true;

    //stores current character showning
    private string currentCharacter = "Bru";
    private PlayerData.ControlDevice currentDevice = PlayerData.ControlDevice.KEYBOARD;

    //stores number of free keyboard and controllers
    private int freeKeyboard = 1;
    private int freeController;
    private Dictionary<int, PlayerData> playerDataDicTemp;

    private string nextCharacter()
    {
        characterIndex++; // increment index
        characterIndex %= characterList.Count; // clip index (turns to 0 if index == items.Count)
                                      // as a one-liner:
                                      /* index = (index + 1) % items.Count; */

        return characterList[characterIndex];
    }

    private string previousCharacter()
    {
        characterIndex--; // decrement index
        if (characterIndex < 0)
        {
            characterIndex = characterList.Count - 1; 
        }


        return characterList[characterIndex];
    }

    private PlayerData.ControlDevice nextDevice()
    {
        deviceIndex++; // increment index
        deviceIndex %= deviceList.Count;

        return deviceList[deviceIndex];
    }

    private PlayerData.ControlDevice previousDevice()
    {
        deviceIndex--; // decrement index
        if (deviceIndex < 0)
        {
            deviceIndex = deviceList.Count - 1;
        }

        return deviceList[deviceIndex];
    }

    //this controls visuals of keyboard/gamepad icons too 
    //pass in variables
    public void selectButton(){
		if(changeTextCounter == 0){
            //FindObjectOfType<AudioManager>().Play("tick");
            displayKeyboard();
            //playerSelectBool = false;
            // playerDataDicTemp = GameManager.Instance.getPlayerDic();

            // playerDataDicTemp.Add(1, playerData);
            // Debug.Log("playerDic playerDataDicTemp Count: " + playerDataDicTemp.Count);
            // //playerDataDicTemp.Add(1, new PlayerData(currentDevice, currentCharacter));
            // GameManager.Instance.setPlayerDic(playerDataDicTemp);
            //GameManager.Instance.playerDataDict.Add(1, new PlayerData(currentDevice, currentCharacter));
            characterList.Remove(currentCharacter);
			topText.GetComponent<Text>().text = "Player 1\nSelect Controller";
			changeTextCounter++;
            currentCharacter = nextCharacter();
		}
		else if (changeTextCounter == 1)
		{
            //FindObjectOfType<AudioManager>().Play("tick");
            //GameManager.Instance.player1ControlDevice = 

            //displayBru();
            //playerSelectBool = true;
            Debug.Log("currentDevice: " + currentDevice);
            Debug.Log("currentCharacter: " + currentCharacter);
            PlayerData playerData = new PlayerData(currentDevice, currentCharacter);
            GameManager.playerDataDict.Add(1, playerData);
            topText.GetComponent<Text>().text = "Player 2\nSelect Character";

            if (currentDevice == PlayerData.ControlDevice.KEYBOARD)
            {
                freeKeyboard -= 1;
            }

            currentDevice = nextDevice();
            changeTextCounter++;
		}
		else if (changeTextCounter == 2)
		{
            //FindObjectOfType<AudioManager>().Play("tick");
            //displayKeyboard();
            //playerSelectBool = false;
            // playerDataDicTemp = GameManager.Instance.getPlayerDic();
            // PlayerData playerData = new PlayerData(currentDevice, currentCharacter);
            // playerDataDicTemp.Add(2, playerData);
            // Debug.Log("playerDic playerDataDicTemp Count: " + playerDataDicTemp.Count);
            // //playerDataDicTemp.Add(1, new PlayerData(currentDevice, currentCharacter));
            // GameManager.Instance.setPlayerDic(playerDataDicTemp);

            //GameManager.Instance.playerDataDict.Add(2, new PlayerData(currentDevice, currentCharacter));
            topText.GetComponent<Text>().text = "Player 2\nSelect Controller";
			changeTextCounter++;
		}
        else if(changeTextCounter == 3)
        {
            //FindObjectOfType<AudioManager>().Stop("mainMenuMusic");
            //FindObjectOfType<AudioManager>().Play("playAgain");
            Debug.Log("currentDevice: " + currentDevice);
            Debug.Log("currentCharacter: " + currentCharacter);
            PlayerData playerData = new PlayerData(currentDevice, currentCharacter);
            GameManager.playerDataDict.Add(2, playerData);
            Debug.Log("playerDic SelectorScript: " + GameManager.playerDataDict.Count);
            if (currentDevice == PlayerData.ControlDevice.KEYBOARD)
            {
                freeKeyboard -= 1;
            }

            //currentDevice = nextDevice();

            Initiate.Fade("Level2", Color.white, 0.6f);
        }

        if (freeKeyboard <= 0)
        {
            deviceList.Remove(PlayerData.ControlDevice.KEYBOARD);
            Debug.Log("changeTextCounter: " + changeTextCounter);
        }

        // Debug.Log("changeTextCounter: " + changeTextCounter);
        // Debug.Log("freeKeyboard: " + freeKeyboard);
        // Debug.Log("currentDevice: " + currentDevice);
        // Debug.Log("currentCharacter: " + currentCharacter);
    }




    //characterInfo[0] = CharacterSelect("Bru", "Controller", 1);
    //characterInfo[1] = CharacterSelect("Jotun", "Keyboard", 2);


    private void Awake()
    {
        
        JotunRender = Jotun.GetComponent<SpriteRenderer>();
        BruRender = Bru.GetComponent<SpriteRenderer>();
        keyboardRender = KeyboardIcon.GetComponent<SpriteRenderer>();
        controllerRender = ControllerIcon.GetComponent<SpriteRenderer>();
        displayBru();
        topText = GameObject.FindGameObjectWithTag("TopInfoText");
        //public TileData[] tiles;
    }

    private void Start()
    {
        freeController = InputManager.Devices.Count;
    }

    private void Update()
    {
        Debug.Log("SelectorScript Number of Devices: " + InputManager.Devices.Count);
        if (changeTextCounter == 0 || changeTextCounter == 2)
        {
            switch (currentCharacter)
            {
                case "Bru":
                    displayBru();
                    break;
                case "Jotun":
                    displayJotun();
                    break;
            }
        } else
        {
            switch (currentDevice)
            {
                case PlayerData.ControlDevice.KEYBOARD:
                    displayKeyboard();
                    break;
                default:
                    displayController();
                    break;
            }
        }
    }

    public void NextButton()
    {
        if (changeTextCounter == 0 || changeTextCounter == 2)
        {
            currentCharacter = nextCharacter();
        }
        else
        {
            currentDevice = nextDevice();
        }


        //switch (SelectorInt)
        //{
        //    case 1:
        //        if (playerSelectBool == true)
        //        {
        //            displayJotun();
        //            SelectorInt++;
                    
        //        }
        //        else
        //        {
        //            displayKeyboard();
        //            SelectorInt++;
        //        }

        //        break;


        //    case 2:
        //        if(playerSelectBool == true)
        //        {
        //            displayBru();
        //            SelectorInt++;
        //            ResetInteger();
                    
        //        }
        //        else
        //        {
        //            displayController();
        //            SelectorInt++;
        //            ResetInteger();
        //        }
        //        break;
        //    default:
        //        ResetInteger();
        //        break;
        //}

    }


    public void PreviousButton()
    {
        if (changeTextCounter == 0 || changeTextCounter == 2)
        {
            currentCharacter = previousCharacter();
        }
        else
        {
            currentDevice = previousDevice();
        }

        //switch (SelectorInt)
        //{
        //    case 1:
        //        if(playerSelectBool == true)
        //        {
        //            displayJotun();
        //            SelectorInt--;
        //            ResetInteger();
        //        }
        //        else
        //        {
        //            displayKeyboard();
        //            SelectorInt--;
        //            ResetInteger();
        //        }

        //        break;
        //    case 2:
        //        if (playerSelectBool == true)
        //        {
        //            displayBru();
        //            SelectorInt--;
        //        }
        //        else
        //        {
        //            displayController();
        //            SelectorInt--;
        //        }


        //        break;
        //    default:
        //        ResetInteger();
        //        break;
        //}
    }


    private void displayBru()
    {
        JotunRender.enabled = false;
        BruRender.enabled = true;
        controllerRender.enabled = false;
        keyboardRender.enabled = false;
    }
    private void displayJotun()
    {
        BruRender.enabled = false;
        JotunRender.enabled = true;
        controllerRender.enabled = false;
        keyboardRender.enabled = false;
    }

    private void displayKeyboard()
    {
        JotunRender.enabled = false;
        BruRender.enabled = false;
        controllerRender.enabled = false;
        keyboardRender.enabled = true;

    }

    private void displayController()
    {
        JotunRender.enabled = false;
        BruRender.enabled = false;
        keyboardRender.enabled = false;
        controllerRender.enabled = true;
    }

    private void ResetInteger()
    {
        if (SelectorInt >= 2)
        {
            SelectorInt = 1;
        }
        else
        {
            SelectorInt = 2;
        }
    }

    

}
