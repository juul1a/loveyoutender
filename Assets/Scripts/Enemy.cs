using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class Enemy : MonoBehaviour {

	public float health;
	public float movementSpeed;
	public float visionDistance;
	public float damage;
	public int direction;
	public bool attacking;
	public Color hurtColor;
	[SerializeField]
	protected bool dead;
	protected Rigidbody2D enemyRB;
	protected Animator anim;
	protected Vector3 lastPos;
	protected bool boundedCam;
	protected float maxX, minX;
	protected Camera cam;


	virtual protected void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
				Attack(col);
		}
	}
	virtual protected void OnTriggerStay2D(Collider2D col){
		if(col.tag == "Player"){
				Attack(col);
		}
	}
	virtual protected void OnTriggerExit2D(Collider2D col){
		if(col.tag == "Player"){
			NotAttacking();
		}
	}

	protected void Flip(){
		//Debug.Log("Flipped");
		transform.Rotate(0f, 180f, 0f);
		direction = direction * -1;
	}

	virtual protected void Attack(){}
	virtual protected void Attack(Collider2D col){
		Attack();
	}

	virtual protected void NotAttacking(){}

	virtual public void TakeDamage(float damageTaken){
		if(!dead){
			health -= damageTaken;
			StartCoroutine ("HurtColor");
			if(health <= 0){
				Die();
			}
		}
	}

	protected bool IsPlayerBehind(Transform player){
		bool behindBaddieLeft = direction>0 && (player.position.x - transform.position.x ) < 0 ;
		bool behindBaddieRight = direction<0 && (transform.position.x - player.position.x) < 0;
		if(behindBaddieLeft || behindBaddieRight){
			return true;
		}
		return false;
	}

	IEnumerator HurtColor() {
		for (int i = 0; i < 3; i++) {
			SpriteRenderer[] SPRs = GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer SPR in SPRs){
				SPR.color = hurtColor;
			}
			yield return new WaitForSeconds (.1f);
			foreach(SpriteRenderer SPR in SPRs){
				SPR.color = Color.white; //White is the default "color" for the sprite, if you're curious.
			}
			yield return new WaitForSeconds (.1f);
		}
	} //This IEnumerator runs 3 times, resulting in 3 flashes.

	virtual public void Die(){}

	public bool isDead(){
		return dead;
	}

	protected void StayWithinBounds(){
		CinemachineVirtualCamera vCam = Object.FindObjectOfType<CinemachineVirtualCamera>();
		if(vCam){
			if(vCam.Follow){
				boundedCam = false;
			}
			else{
				boundedCam = true;
				Vector2 camBottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,0));
				Vector2 camTopRight = cam.ViewportToWorldPoint(new Vector3(1,1,0));
				minX = camBottomLeft.x;
				maxX = camTopRight.x;
			}
		}
		else{
			boundedCam = false;
		}
	}

}

