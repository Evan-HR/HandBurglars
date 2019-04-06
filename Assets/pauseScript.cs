using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseScript : MonoBehaviour {

public static bool GameIsPaused = false;
public GameObject pauseMenuUI;
    public Text text;
    private Scene scene;


    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        //GameObject hintText = GameObject.Find("Hint");
        //text = hintText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(GameIsPaused){
				Resume();
			}else{
				Pause();
			}
		}
		
	}

	public void Resume(){
        text.text = "";
		pauseMenuUI.SetActive(false);
        //FindObjectOfType<AudioManager>().Volume("bossBattle", 0.26f);
        Time.timeScale = 1f;
		GameIsPaused = false;

	}
	void Pause(){
		pauseMenuUI.SetActive(true);

        //lower music volume
        //FindObjectOfType<AudioManager>().Volume("bossBattle", 0.06f);
        //freeze game
        Time.timeScale = 0f;
		GameIsPaused = true;		
	}

    public void Hint()
    {
        if(scene.name == "Level1")
        {
            text.text = "Both players must survive the boulder's wrath! Seek shelter underground and hide behind tall structures!";
        }
        else if(scene.name == "Level2")
        {
            text.text = "Handits can climb ropes and push rusty old mine carts!";
        }
    }
}
