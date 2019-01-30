using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//used for accessing UI 
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public int health;

    public static bool death;

    public static int sharedLives;
    public int numOfHearts;
    public static int numOfSharedHearts;

    public Image[] hearts;
    public static Image[] sharedLivesHearts;
    public Image[] sharedLivesHeartsInspector;
    public Sprite fullHeart;
    public Sprite emptyHeart;


void Start(){
sharedLivesHearts = sharedLivesHeartsInspector;
        sharedLives = 3;
    }
     void Update()
    {
        if(health > numOfHearts)
        {
            health = numOfHearts;
        }

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
        if(death){
            sharedLives--;
            if(sharedLives == 2)
            {
                FullHealth();
                print("after fullHealth function, player health is: " + health);
                sharedLivesHearts[2].sprite = emptyHeart;

            }else if(sharedLives == 1)
            {
                FullHealth();
                sharedLivesHearts[1].sprite = emptyHeart;
                //death, restart level
            }else if (sharedLives == 0)
            {
                sharedLivesHearts[0].sprite = emptyHeart;
            }
            
            
            //health = numOfHearts;
            death = false;
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

}
