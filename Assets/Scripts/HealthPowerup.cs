using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    // Start is called before the first frame update

    public float health;

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Player"){
            Player player = col.gameObject.GetComponent<Player>();
            player.Heal(health);
            Destroy(gameObject);
        }
    }

}
