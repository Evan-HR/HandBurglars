using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnCollisionEnter2D(Collision collision)
    //{
    //    Debug.Log("Collider.name " + collision.gameObject.tag);
    //    if (collision.gameObject.tag == "Floor")
    //    {
    //        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), this.GetComponent<BoxCollider2D>());
    //    }
    //}

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("Collider.name " + col.gameObject.tag);

        if (col.gameObject.tag == "Floor")
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<BoxCollider2D>(), this.GetComponent<BoxCollider2D>());
        }
    }
}
