using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostSprite : MonoBehaviour {
    SpriteRenderer sprite;
    float timer = 0.2f;

	// Use this for initialization
	void Start () {
        //link sprite to sprite renderer
        sprite = GetComponent<SpriteRenderer>();

        //set position of ghost equal to player's position in the Start function
        transform.position = PlayerController.Instance.transform.position;
        //do same for localScale so ghost will look in same direction as player
        transform.localScale = PlayerController.Instance.transform.localScale;

        //set sprite of the ghost equal to player sprite 
        sprite.sprite = PlayerController.Instance.playerSprite.sprite;
        //change color of ghost
        sprite.color = new Vector4(50, 50, 50, 0.2f);
		
	}
	
	// Update is called once per frame
	void Update () {
        //decrease timer, if falls to 0 destroy the ghost
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
		
	}
}
