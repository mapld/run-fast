using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

    public int framesBetween = 15;
    protected int counter;
    protected int initWait;

	// Use this for initialization
	void Start () {
        counter = framesBetween;
        initWait = framesBetween * 4;
	}

    void OnGUI()
    {
        int wM = Screen.width / 20;
        int lM = Screen.height / 20;
        GUI.Label(new Rect(3*wM, 7*lM, 40, 100), GlobalData.Coins.ToString());
        GUI.Label(new Rect(3 * wM, 10 * lM, 40, 100), GlobalData.Distance.ToString());
        GUI.Label(new Rect(17 * wM, 11 * lM, 40, 100), GlobalData.Highscore.ToString());
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown ("r"))
			Application.LoadLevel("MainScene");

        if (GlobalData.Coins > 0){
            if (initWait <= 0)
                counter--;
            else
                initWait--;
        }
        else
        {
            if(GlobalData.Distance > GlobalData.Highscore)
                GlobalData.Highscore = GlobalData.Distance;
        }
            
        if (counter <= 0 && GlobalData.Coins > 0)
        {
            GlobalData.Coins -= 1;
            GlobalData.Distance += 10;
            framesBetween--;
            counter = framesBetween;
        }

	}
}
