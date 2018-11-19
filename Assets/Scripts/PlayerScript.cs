using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public Vector2 currentPos;
    public Vector2 playerVelocity;
	public float forcetoAdd=100;
    public int health;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Dangerous")){

            playerDamaged(10);
            print("Health" + health.ToString());
            GetComponent<Rigidbody2D>().velocity = new Vector2(-20, 20);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }

    public void playerDamaged(int damageAmt){
        health -= damageAmt;
    }

    void Start () {
        //gives it force
        health = 100;
        currentPos = transform.position;
        
		GetComponent<Rigidbody2D> ().velocity = Vector2.up * 10;

	}


	void Update () {

        playerVelocity = (Vector2)transform.position - currentPos;
        //print("Velocity X: " + playerVelocity.x.ToString() +"\nVelocity Y: " + playerVelocity.y.ToString());

		if (Input.GetKey (KeyCode.A))  
			GetComponent<Rigidbody2D> ().AddForce(-Vector2.right*forcetoAdd);

		if (Input.GetKey (KeyCode.D))  
			GetComponent<Rigidbody2D> ().AddForce(Vector2.right*forcetoAdd);
	}			
}
