using UnityEngine;
using System.Collections;

// Global speed script. To be placed on an empty gameobject somewhere
public class GlobalSpeed : MonoBehaviour {

	public float baseGlobalSpeed;		// Starting global speed (increases from here) 
	public float maxGlobalSpeed;
	public float globalAcceleration;    // Rate at which speed increases
	static public float globalSpeed;    // Current global base speed of objects

	void Awake () {
		GlobalSpeed.globalSpeed = baseGlobalSpeed; 
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(globalSpeed < maxGlobalSpeed)
			globalSpeed += Time.deltaTime * globalAcceleration;
	    
	}
}
