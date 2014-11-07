using UnityEngine;
using System.Collections;

// Basic script that pushes an object left based on global speed
public class SolidObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = new Vector2(-GlobalSpeed.globalSpeed,0);
	}
}
