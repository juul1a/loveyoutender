using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    void Start(){
		//Grab the first animator it finds 
		Animator[] animators = GetComponentsInChildren<Animator>();
		if(animators.Length>0){
			anim = animators[0];
		}
    }

	override public void Die(){
		anim.SetTrigger("Die");
		//anim.SetBool("Moving", false);
		dead = true;
		//enemyRB.simulated = false;
	}
}
