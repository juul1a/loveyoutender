using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WalkingEnemy : Enemy {

	public bool revive = false;
	public Transform[] GroundPoints;
	public LayerMask whatIsGround;
	private float groundRadius = 2;
	private float flipTimer;
	
	


	// Use this for initialization
	void Start () {
		// Debug.Log("Set direction to 1");
		direction = 1; //positive is right negative is left
		enemyRB =  GetComponent<Rigidbody2D> ();
		anim = GetComponentsInChildren<Animator>()[0];
		cam =  GameObject.Find ("Main Camera").GetComponent<Camera>();
		StayWithinBounds();
		attacking = false;
		dead = false;
		lastPos = transform.position;
	}
	
	void FixedUpdate () {
		if(!dead && !attacking && IsGrounded()){
				Move();	
			if(boundedCam && flipTimer <= 0){
				if(transform.position.x > maxX || transform.position.x < minX){
					Flip();
					flipTimer = 1.0f;
				}
			}
			if(flipTimer >0){
				flipTimer -= Time.deltaTime;
			}
		}
	}

	void Update(){
		DetectFloor();
		DetectPlayer();
		// Animate();
		
	}

	void Move() {
		enemyRB.velocity = transform.right * movementSpeed;
		anim.SetBool("Moving", true);
		if(transform.position.x == lastPos.x){
			//we are stuck!
			// Debug.Log("Flipping because of x");
			Flip();
		}
		lastPos = transform.position;
	}


	void DetectFloor(){
		Vector2 edgeDirection = new Vector2(direction, -2.1f);
		RaycastHit2D floorHit = Physics2D.Raycast(transform.position, edgeDirection, Mathf.Infinity, whatIsGround); //12 is ground
		if(!floorHit && IsGrounded()){
			// Debug.Log("Flipping because of floor");
			Flip();
		}
	}


	void DetectPlayer() {
		// Debug.Log("vision distance = "+visionDistance);
		Collider2D[] playerHit = Physics2D.OverlapCircleAll(transform.position, visionDistance);
		// Debug.Log("playerhit business = " + playerHit.transform.gameObject.name);
		if(playerHit.Length > 0) {
			foreach(Collider2D hit in playerHit){
				if(hit.transform.tag == "Player"){
					// Debug.Log("Player detected");
					// bool behindBaddieLeft = direction>0 && (hit.transform.position.x - transform.position.x ) < 0 ;
					// bool behindBaddieRight = direction<0 && (transform.position.x - hit.transform.position.x) < 0;
					if(IsPlayerBehind(hit.transform)){
						// Debug.Log("Player is behind baddie");
						//player is behind baddie
						if(!dead || attacking){
							// Debug.Log("Flipping");
							Flip();
						}				
					}
				}
			}
			
		}
	}

	private bool IsGrounded(){
		foreach (Transform point in GroundPoints) {
			//Make a circle collider over every ground point
			Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
			for (int i = 0; i < colliders.Length; i++) {
				//If collider != player, so player does not collide with itself
				if(colliders[i].gameObject != gameObject){
					return true;
				}
			}
		}
		return false;
	}

	override protected void Attack(){
		if((!dead || revive) && !attacking){
			anim.SetBool("Attacking", true);
			anim.SetBool("Moving", false);
			attacking = true;
		}
		// if(dead && revive){
		// 	enemyRB.simulated = true;
		// }
	}

	override protected void NotAttacking(){
		// if(attacking){
			anim.SetBool("Attacking", false);
			attacking = false;
			if(dead && revive){
				//die again
				Die();
			}
		// }
	}

	override public void TakeDamage(float damageTaken) {
		health -= damageTaken;
		if(!dead && health <= 0){
			Die();
		}
	}

	override public void Die(){
		anim.SetTrigger("Die");
		anim.SetBool("Moving", false);
		dead = true;
		enemyRB.simulated = false;
	}

	IEnumerator wait(float waitTime){
		yield return new WaitForSeconds(waitTime);
	}

	// void OnDrawGizmos(){
	// 	Handles.color = Color.red;
	// 	UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, visionDistance);
	// }
}

