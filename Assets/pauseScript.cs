using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseScript : MonoBehaviour {

public static bool GameIsPaused = false;
public GameObject pauseMenuUI;
	
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
		pauseMenuUI.SetActive(false);
        FindObjectOfType<AudioManager>().Volume("bossBattle", 0.26f);
        Time.timeScale = 1f;
		GameIsPaused = false;

	}
	void Pause(){
		pauseMenuUI.SetActive(true);

        //lower music volume
        FindObjectOfType<AudioManager>().Volume("bossBattle", 0.06f);
        //freeze game
        Time.timeScale = 0f;
		GameIsPaused = true;
		
	}
}
