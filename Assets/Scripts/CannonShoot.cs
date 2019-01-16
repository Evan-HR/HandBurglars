﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour {
    public GameObject cannonBall;
    public float firePower;
    private BossFollow bossFollow;

    private void Start()
    {
        bossFollow = GameObject.FindObjectOfType<BossFollow>();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void ShootCannon()
    {
        cannonBall = Instantiate(cannonBall, transform.position, transform.rotation);
        cannonBall.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right*firePower,ForceMode2D.Impulse);
        bossFollow.CanDuck();
        Debug.Log("ShootCannon");

    }

    public GameObject getCannonballGameObject()
    {
        return cannonBall;
    }

}
