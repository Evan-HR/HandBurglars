using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartGame : MonoBehaviour
{
    public void StartGame()
    {
        print("got here?");
        //FindObjectOfType<AudioManager>().Play("weebooSound");
        Initiate.Fade("Menu", Color.black, 1.2f);
    }
}
