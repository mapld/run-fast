using UnityEngine;
using System.Collections;

public class CeilingSpawnerScipt : MonoBehaviour {

	public GameObject ceilingSpikes;

	protected int counter;
	public int baseTime;

	// Use this for initialization
	void Start () {
		counter = (int) (baseTime / GlobalSpeed.globalSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		counter--;
		if (counter <= 0) {
			counter = (int) (baseTime / GlobalSpeed.globalSpeed);
			if(Random.Range(0,100) < 50){
				GameObject curSpikes = (GameObject)Instantiate (ceilingSpikes);
				Destroy (curSpikes,25/(int)GlobalSpeed.globalSpeed);
			}
		}
	}
}
