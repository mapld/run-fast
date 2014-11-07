using UnityEngine;
using System.Collections;

/*
 * Script determines the control and physics of the player-controlled unit
 * To be added to a Sprite with a rigidbody / 2D box collider
*/

public class PlayerScript : MonoBehaviour {

	// JUMPING SHIT
	public int jumpheight; 		 // Original upward velocity of the jump
	public int forceToAdd;		 // The amount of upward force added while the jump button is held down
	public int forceToAddDown;   // The amount of downward force added after the jump button is let go
	bool jumping = false;
	bool addMoreForce = false;
	bool nextJump = false;

	// CROUCHING SHIT -- TEMPORARY maybe
	public int squishSpeed;		 // Speed of the crouching animation
	bool squishing = false;
	Vector3 squishScale;
	Vector3 fullScale;

	// WEAPON SHIT
	public GameObject weapon; 
	public GameObject currentWeapon;

	// Animation
	Animator animator;

	// Various internal stuff
	bool detectCollision = false; // Determines when a jump ends and another can begin. See below
	float centerXPos; 			  // Takes the x value of the player character

	// Initialization
	void Start () {
		fullScale = transform.localScale;
		squishScale = new Vector3(transform.localScale.x,transform.localScale.y / 2,transform.localScale.y);

		renderer.material.color = Color.green;
		centerXPos = transform.position.x;

		animator = GetComponent<Animator>();
		
	}
	
	
	// Update is called once per frame
	void Update () {

	 // Instructions to the animator:
		if(jumping)
			animator.SetBool("jumping",true);
		else
			animator.SetBool("jumping",false);

  
	 // Jumping instructions:
		if(Input.GetKeyDown ("up")){

			if(jumping == false){
				
				Jump();

			}else{
				nextJump = true;
			}
		}
		if(Input.GetKeyUp ("up")){
			addMoreForce = false;
			detectCollision = true;
		}
		if(Input.GetKey("up")){

			if(nextJump && (jumping == false)){
				Jump ();
				nextJump = false;
			}
		}

	 // Squishing instructions
		if(Input.GetKeyDown("down")){
			squishing = true;
			
		}
		if(Input.GetKeyUp("down")){
			
			squishing = false;
		}
		if(squishing)
			transform.localScale = Vector3.Lerp(transform.localScale , squishScale, Time.deltaTime * squishSpeed);
		else
			transform.localScale = Vector3.Lerp(transform.localScale , fullScale, Time.deltaTime * squishSpeed);

	 // Keeps the player character on the same x value
		if(transform.position.x != centerXPos)
			transform.position = new Vector3(centerXPos,transform.position.y,transform.position.z);


	 // Temporary weapon spawn command
		if(Input.GetKeyUp("w")){
			if(currentWeapon == null)
				spawnNewWeapon (weapon);
			
		}
		
	}

	// Starts the player character's jump
	void Jump(){

		rigidbody2D.velocity = new Vector2(0,jumpheight);
	
		jumping = true;
		addMoreForce = true;
		detectCollision = false;
	}

	// Called on a fixed interval
	void FixedUpdate(){
		
		if(jumping){
			if(Input.GetKey("up") && addMoreForce){
				rigidbody2D.AddForce(new Vector2(0,forceToAdd));
			}else
				rigidbody2D.AddForce(new Vector2(0,-forceToAddDown));
		}
	}
	
	void OnCollisionEnter2D(Collision2D col){
		// Resets the jump if the player character collides with something tagged as floor 
		// and the jump button has been let go
		if(col.gameObject.tag.Contains ("Floor")  && detectCollision){
			jumping = false;
			
		}


		

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag.Contains ("Enemy") || col.gameObject.tag.Contains ("Spikes")){
			Debug.Log("Game over");
			Application.LoadLevel("GameOver");
			
		}

	}

	// Spawns the given object and attaches it to the player character
	void spawnNewWeapon(GameObject newWeapon){
		currentWeapon = (GameObject)Instantiate(newWeapon, transform.position, transform.rotation);
		currentWeapon.transform.parent = transform;
	}
}
