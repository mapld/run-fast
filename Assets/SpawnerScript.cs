using UnityEngine;
using System.Collections;

/* 
 * Script takes care of spawning all game objects.
 * To be placed on an empty GameObject
 * Objects will be spawned at the current x value of the spawner 
*/

public class SpawnerScript : MonoBehaviour {

	// Prefab folders
	public GameObject[] platforms;
	public GameObject[] enemies;

    public float minCoin;
    public float maxCoin;

	// Spikes
	public float chanceToSpawnSpikes = 55;
    public float chanceToSpawnCoin = 33;
	protected bool spawnSpikes;
    protected bool spawnCoins;
	public GameObject spikes;
	public float spikeHeight;

	// Temporary objects for spawning
	public GameObject platform; 
	public GameObject enemy;
    public GameObject coin;

	// Time in seconds before any spawned object will be destroyed
	protected int timeBeforeDestroy; 

	// Acceptable spawning time between different objects
	protected float minTimeBetweenPlatforms;
	protected float maxTimeBetweenPlatforms;
	protected float minTimeBetweenEnemies;
	protected float maxTimeBetweenEnemies;

	// Internal numbers for random generation of Objects 
	protected float timeCounter;
	protected float maxTimeCounter;
	protected int maxFrameCounter;

	// Previous and next game objects being spawned
	protected GameObject lastObject;
	protected GameObject nextObject;
	protected float lastPlatformHeight;
	protected float additionalPlatformHeight;


	// Rarities of various objects
	public int platformRarity = 100;
	public int enemyRarity = 100;
	public int normalPlatformChance = 32;

	
	// Speed
	public GameObject speedController;
	protected float baseSpeed;
	protected float totalSpeedDifference;
	protected float maxSpeed;
	public float endDifference;			// Final percentage of initial max difference to be used
	protected float spaceToRemove;

	void Start () {


		// Temporary maybe
		timeBeforeDestroy = 5;
		minTimeBetweenPlatforms = 1.1f;
		maxTimeBetweenPlatforms = 1.85f;
		minTimeBetweenEnemies = 0.65f;
		maxTimeBetweenEnemies = 1.5f;
		endDifference = 0.5f;



		platforms = Resources.LoadAll<GameObject>("Platforms/");

		// Permanent
		nextObject = enemy;
		determineNextObject();



		if(nextObject.tag.Contains("Platform")){
			timeCounter = minTimeBetweenPlatforms;
			maxTimeCounter = maxTimeBetweenPlatforms - minTimeBetweenPlatforms;
		}else if(nextObject.tag.Contains("Enemy")){
			timeCounter = minTimeBetweenEnemies;
			maxTimeCounter = maxTimeBetweenEnemies - minTimeBetweenEnemies;
		}


		baseSpeed = GlobalSpeed.globalSpeed;
		spaceToRemove = 1 - endDifference;
		maxSpeed = speedController.GetComponent<GlobalSpeed>().maxGlobalSpeed;
		totalSpeedDifference = maxSpeed - baseSpeed;



	}
	
	// Update is called once per frame
	void Update () {

		if(timeCounter > 0){
			timeCounter -= Time.deltaTime;
			maxFrameCounter = (int)((maxTimeCounter - timeCounter) / Time.deltaTime);

		}else{
			if(maxTimeCounter > 0){
				maxTimeCounter -= Time.deltaTime;
				maxFrameCounter -= 1;
				if(Random.Range (1,maxFrameCounter) == 1 && spawnSpikes){
					spawnSpike();
				}
                if (Random.Range(1, maxFrameCounter-10) == 1 && spawnCoins)
                {
                    spawnCoin();
                }
				if(Random.Range (1,maxFrameCounter) == 1){
						makeNextObject();
				}
			}else{
				makeNextObject();
			}
		}


	}

	public void spawnSpike(){
		Spawn (spikes,spikeHeight);
		spawnSpikes = false;
	}

    public void spawnCoin()
    {
        float tempHeight;
        if (lastObject.tag.Contains("Platform"))
        {
            tempHeight = Random.Range(minCoin, maxCoin);
            Spawn(coin, tempHeight);
        }
           
        spawnCoins = false;
    }

	public void makeNextObject(){

		float timeBetweenDifference = maxTimeBetweenPlatforms - minTimeBetweenPlatforms;
		float heightLimitThreshold = timeBetweenDifference / 3;
		float extraHeightThreshold = timeBetweenDifference - heightLimitThreshold;
		
		float maxBonusHeight = 0;
		float minBonusHeight = 0;
		lastPlatformHeight = lastObject.GetComponent<HeightScript>().addedHeight;
		
		if(maxTimeCounter > extraHeightThreshold && lastObject.tag.Contains("Platform"))
			for(int i = 1;i < 7; i++){
				float iFloat = i + 0.65f;
				if(lastPlatformHeight > iFloat){
					maxBonusHeight += 1;
					minBonusHeight += Random.Range(0.0f,1.0f);
				}
			}
		else
			if(maxTimeCounter < heightLimitThreshold)
				maxBonusHeight = lastPlatformHeight;

		float currentSpeedDifferential = GlobalSpeed.globalSpeed - baseSpeed;
		float multiplier = (currentSpeedDifferential/totalSpeedDifference) * spaceToRemove;
		

		additionalPlatformHeight = Random.Range(minBonusHeight,maxBonusHeight);
		Spawn (nextObject, transform.position.y + additionalPlatformHeight);
		determineNextObject();
		determineSpikes();
        determineCoin();
		
		if(nextObject.tag.Contains("Platform")){
			if(lastObject.tag.Contains("Platform")){
				timeCounter = minTimeBetweenPlatforms - ((minTimeBetweenPlatforms * multiplier) /2);
				maxTimeCounter = maxTimeBetweenPlatforms - minTimeBetweenPlatforms;
				maxTimeCounter = maxTimeCounter - (maxTimeCounter * multiplier); 
			}else{
				timeCounter = minTimeBetweenEnemies + ((minTimeBetweenPlatforms - minTimeBetweenEnemies)/2);
				maxTimeCounter = maxTimeBetweenEnemies - minTimeBetweenEnemies;
			}
		}else{
			timeCounter = minTimeBetweenEnemies;
			maxTimeCounter = maxTimeBetweenEnemies - minTimeBetweenEnemies;
		}

		//Debug.Log("Time counter:" + timeCounter);
		//Debug.Log("Max time counter:" + maxTimeCounter);
	}

	// Probably temporary
	public void determineNextObject(){
		lastObject = nextObject;

		int platformRarityR = platformRarity;
		int enemyRarityR = enemyRarity;

		if(lastObject.tag.Contains("Platform"))
			platformRarityR = (int)(1.3*platformRarityR);

		if(lastObject.tag.Contains("Enemy"))
			enemyRarityR = (int)(1.5*enemyRarityR);


		int totalPool = platformRarityR + enemyRarityR;
		int randomNumber = Random.Range (0,totalPool);

		if(randomNumber <= platformRarityR){
			int nextObjectNumber = Random.Range (0, platforms.Length);
			if ( Random.Range(0,100) < normalPlatformChance)
				nextObject = platform;
			else{
				nextObject = platforms[nextObjectNumber];
			}
		}else{
			nextObject = enemy;
		}
	}

	public void determineSpikes(){
		if(Random.Range(0,100) < chanceToSpawnSpikes)
			spawnSpikes = true;
	}

    public void determineCoin()
    {
        if (Random.Range(0, 100) < chanceToSpawnCoin)
            spawnCoins = true;
    }

	public void Spawn(GameObject objectToSpawn){
		GameObject tempObject = (GameObject)Instantiate(objectToSpawn, transform.position, transform.rotation);
		Destroy (tempObject,timeBeforeDestroy);
	}

	public void Spawn(GameObject objectToSpawn,float height){
		Vector3 additionalTransform = new Vector3(0,height,0);
		GameObject tempObject = (GameObject)Instantiate(objectToSpawn, transform.position + additionalTransform, transform.rotation);
		Destroy (tempObject,timeBeforeDestroy);
	}


}
