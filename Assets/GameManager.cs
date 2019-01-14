using UnityEngine;
using UnityEngine.SceneManagement;


public class gameManager : MonoBehaviour {
    bool gameHasEnded = false;
	public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game OVER");
            //restart game, call the restart method
            Restart();
        }
        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
