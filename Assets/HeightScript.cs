using UnityEngine;
using System.Collections;

public class HeightScript : MonoBehaviour {
	public float maxHeight;
	public float minHeight;
	public float addedHeight;


	// Use this for initialization
	void Start () {
		addedHeight = Random.Range(minHeight,maxHeight);
		transform.Translate(0,addedHeight,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
