using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    GameObject player;
    
    void Start(){
        direction = 1;
        player = GameObject.FindWithTag("Player");
        if(IsPlayerBehind(player.transform)){
            Flip();
        }
    }

    //Abstract funcs from Enemy
    // protected void NotAttacking(){
        
    // }
    // protected void Die(){

    // }
}
