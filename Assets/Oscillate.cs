 using UnityEngine;
 using System.Collections;
 
 public class Oscillate : MonoBehaviour {
 
     public float delta;  // Amount to move left and right from the start point
     public float speed; 
     private Vector3 startPos;
 
     void Start () {
         startPos = transform.position;
     }
     
     void Update () {
         Vector3 v = startPos;
         v.y += delta * Mathf.Sin (Time.time * speed);
		  //v.x += delta * Mathf.Sin (Time.time * speed);
         transform.position = v;
     }
 }