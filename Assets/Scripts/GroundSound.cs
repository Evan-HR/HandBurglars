using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSound : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "boulder")
        {

            FindObjectOfType<AudioManager>().Play("Lvl1ThirdCrackGround");
        }
    }


}