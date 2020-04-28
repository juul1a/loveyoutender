using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy {

		public Vector3 origin;
		public float moveDistance;
		// public float damage;
		public float frequency, magnitude;
		public string state;
		private Vector3 returnTo;
		private Vector3 diveTo;
		private Vector3 lerpTo;
		private float journeyLength;
		private float startTime;
		public float diveSpeed = 1f;
		public float attackCountdown = 3f;
		private bool lerpDone = false;
		private bool attacked = false;
		private Player target;
		private Rigidbody2D rb;
		public LayerMask whatIsGround;
		//Vector3 pos;

	// Use this for initialization
	void Start () {
		cam =  GameObject.Find ("Main Camera").GetComponent<Camera>();
		StayWithinBounds();
		direction = 1; //positive is right negative is left
		// enemyRB =  GetComponent<Rigidbody2D> ();
		anim = GetComponentsInChildren<Animator>()[0];
		attacking = false;
		dead = false;
		origin = transform.position;
		lastPos = transform.position;
		state = "moving";
		startTime = 0f;
		rb = gameObject.GetComponent<Rigidbody2D>();
		Physics.IgnoreLayerCollision(15, 11);
		MoveToIdealYPos();
	}

	void FixedUpdate(){
		if(!dead){
			if(state == "moving"){
				Move();
				Bob();
			}
			else if(state == "diving"){
				Lerp();
			}
			else if(state == "returning"){
				if(transform.position.y < origin.y){
					transform.position += new Vector3(0,1,0); 
				}
				else{
					origin = new Vector3 (transform.position.x, origin.y, transform.position.z);
					state = "moving";
				}
			}
			if(state == "moving" && ((transform.position.x - origin.x >= moveDistance) || transform.position.x - origin.x < 0)){
				Flip();
			}
			if(state == "diving" && (lerpDone)){
				NotAttacking();
			}
			if(boundedCam){
				if(transform.position.x > maxX || transform.position.x < minX){
					Flip();
				}
			}
		}
		else{
			if(Mathf.Abs(rb.velocity.y) >200f){
				gameObject.SetActive(false);
			}	
		}
		
	}
	void Lerp(){
		lerpDone = false;
		// Debug.Log("startTime = "+startTime+" and time = "+Time.time);
		float distCovered = (Time.time - startTime) * diveSpeed;
		float fracJourney = distCovered/journeyLength;
		transform.position = Vector3.Lerp(this.transform.position, lerpTo, fracJourney);
		if(!attacked && Vector3.Distance(this.transform.position, lerpTo) <= 10){
			Attack();
			attacked = true;
		}
		else if(Vector3.Distance(this.transform.position, lerpTo) <= 0.1){
			lerpDone = true;
		}
	}

	void Move() {
		transform.position = transform.position + (transform.right * movementSpeed);
		lastPos = transform.position;
	}

	void Bob(){
		transform.position = transform.position + (transform.up * Mathf.Sin(Time.time * frequency) * magnitude);
	}

	override protected void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
				startTime = 0f;
				Dive(col);
		}
	}
	override protected void OnTriggerStay2D(Collider2D col){
		if(col.tag == "Player"){
			if(state == "diving" && Time.time - startTime <= attackCountdown){
				Dive(col);
			}
		}
	}
	override protected void OnTriggerExit2D(Collider2D col){
		if(col.tag == "Player"){
			NotAttacking();
		}
	}

	override protected void Attack(){
			anim.SetTrigger("Attack");
			target.TakeDamage(damage);
	}

	void Dive(Collider2D col){
		GameObject playerObj = col.gameObject;
		target = playerObj.GetComponent<Player>();
		if(state != "returning"){
			if(state == "moving"){
				returnTo = transform.position;
				//origin = transform.position;
			}
			if(startTime == 0){
				startTime = Time.time;
			}
			diveTo = col.gameObject.transform.position; //+ new Vector3(5,5,5);
			lerpTo = new Vector3(diveTo.x, diveTo.y, transform.position.z);
			if(state != "diving"){
				journeyLength = Vector3.Distance(this.transform.position, lerpTo);
			}
			state = "diving";
		}
	}

	// See how close the ground is and move the bat to the most ideal Y position to start
	void MoveToIdealYPos(){
		Vector2 floorDirectionDown = new Vector2(0,-1);
		RaycastHit2D floorHitDown = Physics2D.Raycast(transform.position, floorDirectionDown, Mathf.Infinity, whatIsGround); //12 is ground
		if(floorHitDown){
			Debug.Log("Raycast found floor obj down "+floorHitDown.collider.gameObject.name);
			transform.position = new Vector3(transform.position.x, floorHitDown.collider.gameObject.transform.position.y + 85, transform.position.z);
		}
		else{
			Vector2 floorDirectionUp = new Vector2(0,1);
			RaycastHit2D floorHitUp = Physics2D.Raycast(transform.position, floorDirectionUp, Mathf.Infinity, whatIsGround); //12 is ground
			if(floorHitUp){
				Debug.Log("Raycast found floor obj "+floorHitUp.collider.gameObject.name);
				transform.position = new Vector3(transform.position.x, floorHitUp.collider.gameObject.transform.position.y + 85, transform.position.z);
			}
		}
	}

	override protected void NotAttacking(){
		startTime = 0f;
		attacked = false;
		state = "returning";
		
	}

	// override public void TakeDamage(float damageTaken) {
    //     // Debug.Log("Taking damage");
	// 	// health -= damageTaken;
	// 	// if(!dead && health <= 0){
	// 	// 	Die();
	// 	// }
	// }

	override public void Die(){
		anim.SetTrigger("Die");
		// anim.SetBool("Moving", false);
		dead = true;
		rb.gravityScale = 20;
		// enemyRB.simulated = false;
	}
}

