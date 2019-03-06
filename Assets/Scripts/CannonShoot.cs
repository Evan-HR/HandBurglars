using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour {
    public GameObject cannonBall;
    public float firePower;
    private BossFollow bossFollow;

    public float cannonTimer;

    private void Start()
    {
        bossFollow = GameObject.FindObjectOfType<BossFollow>();
    }

    // Update is called once per frame
    void Update ()
    {
		cannonTimer -= Time.deltaTime; 
	}

    public void ShootCannon()
    {
        if(cannonTimer<=0){
        FindObjectOfType<AudioManager>().Play("cannon");
        cannonBall = Instantiate(cannonBall, transform.position, transform.rotation);
        cannonBall.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right*firePower,ForceMode2D.Impulse);
        bossFollow.Duck();
        cannonTimer = 2.0f;
        }
        

    }

    public GameObject getCannonballGameObject()
    {
        return cannonBall;
    }

}
