using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour {
    public GameObject cannonBall;
    public float firePower;

	// Update is called once per frame
	void Update () {
		
	}

    public void ShootCannon()
    {
        GameObject thisCannonBall = Instantiate(cannonBall, transform.position, transform.rotation);
        thisCannonBall.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.back*firePower);
    }
}
