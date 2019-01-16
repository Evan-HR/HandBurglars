using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    bool gameHasEnded = false;
	public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game OVER");
            //restart game, call the restart method
            GameOver();
        }
        
    }

    public void GameOver()
    {
        FindObjectOfType<AudioManager>().Stop("bossBattle");
        //losing music
        FindObjectOfType<AudioManager>().Play("defeat");
        //death condition
        Initiate.Fade("GameOver", Color.black, 0.6f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("playAgain");
        Initiate.Fade("POC_Boss", Color.white, 0.6f);
        //FindObjectOfType<AudioManager>().Play("bossBattle");
    }
}
