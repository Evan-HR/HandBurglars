using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public Scene scene;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();

        if (scene.name == "Menu")
        {
            FindObjectOfType<AudioManager>().Play("mainMenuMusic");
            FindObjectOfType<AudioManager>().Play("birds");

        }
    }


    public static void GameOver()
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
        if (scene.name == "Menu")
        {
            FindObjectOfType<AudioManager>().Stop("birds");
            FindObjectOfType<AudioManager>().Stop("mainMenuMusic");
        }

        FindObjectOfType<AudioManager>().Play("playAgain");
        Initiate.Fade("POC_Boss", Color.white, 0.6f);
        //FindObjectOfType<AudioManager>().Play("bossBattle");
    }
}
