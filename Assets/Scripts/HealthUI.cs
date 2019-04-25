using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthUI : MonoBehaviour
{
    public Scene scene;

    //storing Sprites in sprite array
    public Sprite[] HeartSprites;

    public Image HeartUIPlayer1;
    public Image HeartUIPlayer2;
    public Image HeartUIGlobal;
    //access health of player

    //player1
    private PlayerManager playerManager1;
    //player2
    private PlayerManager playerManager2;



    private void Start()
    {
        playerManager1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerManager>();
        playerManager2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        if(scene.name != "GameOver"){
        HeartUIPlayer1.sprite = HeartSprites[playerManager1.health];
        HeartUIPlayer2.sprite = HeartSprites[playerManager2.health];
        HeartUIGlobal.sprite = HeartSprites[GameManager.globalLives];
    }
    }
}
