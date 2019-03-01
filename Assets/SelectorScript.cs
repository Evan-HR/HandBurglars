using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	

//this controls visuals of keyboard/gamepad icons too 
//pass in variables
	public void selectButton(){
		if(changeTextCounter == 0){
            FindObjectOfType<AudioManager>().Play("tick");
            displayKeyboard();
            playerSelectBool = false;
			topText.GetComponent<Text>().text = "Player 1\nSelect Controller";
			changeTextCounter++;
		}
		else if (changeTextCounter == 1)
		{
            FindObjectOfType<AudioManager>().Play("tick");
            displayBru();
            playerSelectBool = true;
            topText.GetComponent<Text>().text = "Player 2\nSelect Character";
				changeTextCounter++;
		}
		else if (changeTextCounter == 2)
		{
            FindObjectOfType<AudioManager>().Play("tick");
            displayKeyboard();
            playerSelectBool = false;
            topText.GetComponent<Text>().text = "Player 2\nSelect Controller";
			changeTextCounter++;
		}
			
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
        //public TileData[] tiles;
    }



    public void NextButton()
    {
        switch (SelectorInt)
        {
            case 1:
                if (playerSelectBool == true)
                {
                    displayJotun();
                    SelectorInt++;
                    
                }
                else
                {
                    displayKeyboard();
                    SelectorInt++;
                }

                break;


            case 2:
                if(playerSelectBool == true)
                {
                    displayBru();
                    SelectorInt++;
                    ResetInteger();
                    
                }
                else
                {
                    displayController();
                    SelectorInt++;
                    ResetInteger();
                }
                break;
            default:
                ResetInteger();
                break;
        }

    }


    public void PreviousButton()
    {
        switch (SelectorInt)
        {
            case 1:
                if(playerSelectBool == true)
                {
                    displayJotun();
                    SelectorInt--;
                    ResetInteger();
                }
                else
                {
                    displayKeyboard();
                    SelectorInt--;
                    ResetInteger();
                }

                break;
            case 2:
                if (playerSelectBool == true)
                {
                    displayBru();
                    SelectorInt--;
                }
                else
                {
                    displayController();
                    SelectorInt--;
                }

                
                break;
            default:
                ResetInteger();
                break;
        }
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
