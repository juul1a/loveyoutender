using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
	Rigidbody2D rb;
    void Start(){
		//Grab the first animator it finds 
		Animator[] animators = GetComponentsInChildren<Animator>();
		if(animators.Length>0){
			anim = animators[0];
		}
		rb = gameObject.GetComponent<Rigidbody2D>();
    }

	override public void Die(){
		anim.SetTrigger("Die");
		//anim.SetBool("Moving", false);
		dead = true;
		rb.gravityScale = 20;
		//enemyRB.simulated = false;
	}
}
