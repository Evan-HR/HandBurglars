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
            sharedLives -= 1;
            health = numOfHearts;
            death = false;

            for (int i = 0; i < sharedLivesHearts.Length; i++)
            {
                if (i < health)
                {
                    sharedLivesHearts[i].sprite = fullHeart;
                }
                else
                {
                    sharedLivesHearts[i].sprite = emptyHeart;
                }

                if (i < numOfSharedHearts)
                {
                    sharedLivesHearts[i].enabled = true;
                }
                else
                {
                    sharedLivesHearts[i].enabled = false;
                }
            }

        }
    }

}
