using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//used for accessing UI 
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public int health;

    //public static bool death;

    public static int sharedLives;
    public int numOfHearts;
    public static int numOfSharedHearts;

    public Image[] hearts;
    public static Image[] sharedLivesHearts;
    public Image[] sharedLivesHeartsInspector;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private GameManager healthGameManager;


    void Start(){
        healthGameManager = GetComponent<GameManager>();
        sharedLivesHearts = sharedLivesHeartsInspector;
        sharedLives = 3;
    }
     void Update()
    {
        //EDGE CASE TEST 
        if(health > numOfHearts)
        {
            health = numOfHearts;
        }

        //UI STUFF WITH IMAGE FILES 
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
        
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

    }

    //call this to set fullhealth
    void FullHealth()
    {
        health = 3;
        hearts[2].sprite = fullHeart;
        hearts[1].sprite = fullHeart;
        hearts[0].sprite = fullHeart;
    }

   public void Death()
    {
        sharedLives--;
        if (sharedLives == 2)
        {
            print("1 DEATH: PLAYER HEALTH BEFORE FNC = " + health);
            FullHealth();
            print("1 DEATH: PLAYER HEALTH AFTER FNC = " + health);
            print("1 DEATH: SHARED LIVES = " + sharedLives);
            sharedLivesHearts[2].sprite = emptyHeart;

        }
        else if (sharedLives == 1)
        {

            FullHealth();
            print("2 DEATH: PLAYER HEALTH AFTER FNC = " + health);
            print("2 DEATH: SHARED LIVES = " + sharedLives);
            sharedLivesHearts[1].sprite = emptyHeart;

        }
        else if (sharedLives == 0)
        {
            sharedLivesHearts[0].sprite = emptyHeart;
            GameManager.GameOver();
            
        }

    }

}
